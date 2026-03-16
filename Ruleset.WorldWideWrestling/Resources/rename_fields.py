import pypdf
import pypdf.generic as gen
import os

# Gimmick wants order: general + specific
WANTS = {
    'Ace':          ['A New Championship Title','Join a Group','Revenge','Domination','Adulation of the Crowd','A Record-Breaking Title Run'],
    'Anointed':     ['A New Championship Title','Join a Group','Revenge','Domination','Adulation of the Crowd','Validation from a Mentor'],
    'Anti-Hero':    ['A New Championship Title','Join a Group','Revenge','Domination','Adulation of the Crowd','Take Down the Boss'],
    'Call-Up':      ['A New Championship Title','Join a Group','Revenge','Domination','Adulation of the Crowd','Prove You Belong on this Roster'],
    'Clown':        ['A New Championship Title','Join a Group','Revenge','Domination','Adulation of the Crowd','To Overshadow the Main Event'],
    'Fighter':      ['A New Championship Title','Join a Group','Revenge','Domination','Adulation of the Crowd','A Legit Fight with a Worthy Opponent'],
    'Hardcore':     ['A New Championship Title','Join a Group','Revenge','Domination','Adulation of the Crowd','Raise the Bar for Violence'],
    'Jobber':       ['A New Championship Title','Join a Group','Revenge','Domination','Adulation of the Crowd','A Win Over a Main Event Star'],
    'Luchador':     ['A New Championship Title','Join a Group','Revenge','Domination','Adulation of the Crowd','Unmask My Nemesis'],
    'Luminary':     ['A New Championship Title','Join a Group','Revenge','Domination','Adulation of the Crowd','Mainstream Recognition'],
    'Manager':      ['A New Championship Title','Join a Group','Revenge','Domination','Adulation of the Crowd','Compete in the Ring, Win or Lose'],
    'Monster':      ['A New Championship Title','Join a Group','Revenge','Domination','Adulation of the Crowd','Achieve an Impossible Feat'],
    'Provocateur':  ['A New Championship Title','Join a Group','Revenge','Domination','Adulation of the Crowd','Bring an Opponent into Your World'],
    'Technician':   ['A New Championship Title','Join a Group','Revenge','Domination','Adulation of the Crowd','Make a Superior Opponent Tap Out'],
    'Veteran':      ['A New Championship Title','Join a Group','Revenge','Domination','Adulation of the Crowd','To Cement Your Legacy'],
}

# Name fields to rename (Text Field NNN -> Name)
NAME_FIELDS = {
    'Ace':         'Text Field 3',
    'Anointed':    'Text Field 15',
    'Anti-Hero':   'Text Field 26',
    'Call-Up':     'Text Field 37',
    'Clown':       'Text Field 48',
    'Fighter':     'Text Field 59',
    'Hardcore':    'Text Field 70',
    'Jobber':      'Text Field 81',
    'Luminary':    'Text Field 103',
    'Manager':     'Text Field 114',
    'Monster':     'Text Field 125',
    'Provocateur': 'Text Field 136',
    'Technician':  'Text Field 147',
    'Veteran':     'Text Field 158',
    # Luchador already has 'Name'
}

STAT_NAMES = ['Body', 'Look', 'Real', 'Work']

gimmicks = [
    'Ace', 'Anointed', 'Anti-Hero', 'Call-Up', 'Clown', 'Fighter',
    'Hardcore', 'Jobber', 'Luchador', 'Luminary', 'Manager', 'Monster',
    'Provocateur', 'Technician', 'Veteran'
]

for gimmick in gimmicks:
    pdf_file = f'WorldWideWrestling_Character_Sheet_{gimmick}.pdf'
    out_file = pdf_file + '.tmp'

    reader = pypdf.PdfReader(pdf_file)
    writer = pypdf.PdfWriter()
    writer.clone_document_from_reader(reader)

    page_height = float(reader.pages[0].mediabox.height)
    changes = []

    page = writer.pages[0]
    annots = page.get('/Annots', [])
    if not annots:
        print(f'{gimmick}: no annotations on page 0')
        continue

    # --- Stats ---
    stat_boxes = []
    existing_stats = set()
    for annot_ref in annots:
        obj = annot_ref.get_object()
        if obj.get('/Subtype') != '/Widget':
            continue
        ft = str(obj.get('/FT', ''))
        rect = obj.get('/Rect')
        if not rect:
            continue
        rect = [float(x) for x in rect]
        top = page_height - rect[3]
        width = rect[2] - rect[0]
        height = rect[3] - rect[1]
        current_name = str(obj.get('/T', ''))

        if current_name in STAT_NAMES:
            existing_stats.add(current_name)
        elif ft == '/Tx' and 35 < width < 45 and 15 < height < 22:
            stat_boxes.append((top, obj))

    stat_boxes.sort(key=lambda x: x[0])
    missing_stats = [s for s in STAT_NAMES if s not in existing_stats]

    if len(stat_boxes) == len(missing_stats):
        for i, (top, obj) in enumerate(stat_boxes):
            obj[gen.NameObject('/T')] = gen.create_string_object(missing_stats[i])
            changes.append(f'stat->{missing_stats[i]}')
    elif stat_boxes:
        changes.append(f'WARN:stats {len(stat_boxes)} boxes vs {len(missing_stats)} missing')

    # --- Name field ---
    if gimmick in NAME_FIELDS:
        old_name = NAME_FIELDS[gimmick]
        for annot_ref in annots:
            obj = annot_ref.get_object()
            if str(obj.get('/T', '')) == old_name:
                obj[gen.NameObject('/T')] = gen.create_string_object('Name')
                changes.append(f'Name')
                break

    # --- Want checkboxes ---
    wants_list = WANTS.get(gimmick, [])
    unnamed_wants = []
    named_in_group = set()

    for annot_ref in annots:
        obj = annot_ref.get_object()
        if obj.get('/Subtype') != '/Widget':
            continue
        t = obj.get('/T')
        parent = obj.get('/Parent')
        rect = obj.get('/Rect')
        if not rect:
            continue
        rect = [float(x) for x in rect]
        top = page_height - rect[3]

        if parent:
            parent_obj = parent.get_object()
            parent_name = str(parent_obj.get('/T', ''))
            if 'Radio Button 2' in parent_name:
                if t is None:
                    unnamed_wants.append((top, obj))
                else:
                    named_in_group.add(str(t))

    unnamed_wants.sort(key=lambda x: x[0])
    missing_wants = [w for w in wants_list if w not in named_in_group]

    if len(unnamed_wants) == len(missing_wants):
        for i, (top, obj) in enumerate(unnamed_wants):
            obj[gen.NameObject('/T')] = gen.create_string_object(missing_wants[i])
            changes.append(f'want->{missing_wants[i][:20]}')
    elif unnamed_wants:
        changes.append(f'WARN:wants {len(unnamed_wants)} unnamed vs {len(missing_wants)} missing')

    with open(out_file, 'wb') as f:
        writer.write(f)
    os.replace(out_file, pdf_file)
    print(f'{gimmick}: {changes}')

print('\nDone!')
