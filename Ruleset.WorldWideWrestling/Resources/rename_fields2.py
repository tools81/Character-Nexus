import pypdf
import pypdf.generic as gen
import os

# ---------- DATA ----------

HAILINGS = {
    'Ace':         ['Tokyo, Japan', 'Kagoshima Prefecture, Japan', 'Venice Beach, California'],
    'Anointed':    ['Los Angeles, California', 'Muncie, Indiana', 'Houston, Texas'],
    'Anti-Hero':   ['Detroit, Michigan', 'Brooklyn, New York', 'Alligator, Mississippi'],
    'Call-Up':     ['Seattle, Washington', 'Grand Rapids, Michigan', 'Dubuque, Iowa'],
    'Clown':       ['Kalamazoo, Michigan', 'Pineville, West Virginia', 'Edinburgh, Scotland'],
    'Fighter':     ['Kuwana, Japan', 'Wigan, United Kingdom', 'Rio de Janeiro, Brazil'],
    'Hardcore':    ['Peoria, Illinois', 'Truth or Consequences, New Mexico', 'Union City, New Jersey'],
    'Jobber':      ['Hillsboro, Iowa', 'Rumford, Maine', 'Right Here!'],
    'Luchador':    ['Mexico City, Mexico', 'G\u00f3mez Palacio, Durango, Mexico', 'Boyle Heights, CA'],
    'Luminary':    ['Mexico City, Mexico', 'Venice Beach, California', 'Parts Unknown'],
    'Manager':     ['Memphis, Tennessee', 'New York City', 'Las Vegas, Nevada'],
    'Monster':     ['Parts Unknown', 'Dead Horse, Alaska', 'Kill Devil Hills, North Carolina'],
    'Provocateur': ['Sin City', 'Milan, Italy', 'Tinseltown'],
    'Technician':  ['St. Paul, Minnesota', 'Calgary, Alberta Canada', 'Manchester, England'],
    'Veteran':     ['Hollywood, California', 'Toronto, Ontario Canada', 'Charlotte, North Carolina'],
}

ENTRANCES = {
    'Ace':         ['Licensed Tie-In', 'High Tempo & Exciting', 'High-Concept & Production-Filled'],
    'Anointed':    ['Classic & Impressive', 'Showy & Ostentatious', 'Mild & Unremarkable'],
    'Anti-Hero':   ['Loud & Flashy', 'Downhome & Gritty', 'Silent & Serious'],
    'Call-Up':     ['Reused From A Retired Wrestler', 'Custom Song From a Local Band', 'Fresh & Intriguing'],
    'Clown':       ['Silly & Comical', 'Generic & Forgettable', 'Overly Melodramatic'],
    'Fighter':     ['Heavy & Threatening', 'Explosive & Impulsive', 'Simple & Direct'],
    'Hardcore':    ['Raucous & Aggressive', 'Ironic & Jokey', 'Sudden & Loud'],
    'Jobber':      ['None', 'Silly & Overblown', 'Strangely Sympathetic'],
    'Luchador':    ['American N\u00fc Metal', 'Popular Hit', 'Pulse-Pounding & Frenetic'],
    'Luminary':    ['Custom Composition', 'Pop Music Tie-In', 'Crowd-Pleasing Classic'],
    'Manager':     ['None', 'Generic & Celebratory', 'Uses Client\'s Entrance'],
    'Monster':     ['Thunderous & Impressive', 'Overblown & Odd', 'Weird & Mysterious'],
    'Provocateur': ['Cinematic & Dramatic', 'Dark & Emotional', 'Off-Kilter & Frenetic'],
    'Technician':  ['Generic & Easy', 'Loud & Overblown', 'Raw & Intense'],
    'Veteran':     ['Classic Orchestral', 'Solemn & Dignified', 'Iconic Symbolism'],
}

QUESTIONS = {
    'Ace':         ['Who trained with me in the dojo?', 'Who cares about this company even more than me?', 'Who do I have amazing chemistry with in the ring?'],
    'Anointed':    ['Who did I debut with, then leave behind?', 'Who has taken me under their wing?', 'Who is jealous of my rapid rise?'],
    'Anti-Hero':   ['Who did I kick the shit out of to prove how tough I am?', 'With whom do I have a reluctant alliance against a mutual enemy?', 'Who have I called out as a slave to management?'],
    'Call-Up':     ['Who has sung my praises to get me on the roster?', 'Who thinks I\'m completely overrated?', 'With whom do I have an old rivalry that can now be restarted?'],
    'Clown':       ['Who asks me for tips to add something fresh to their act?', 'Who am I overshadowing with my antics?', 'Who always turns to me to save their segments when they drop the ball?'],
    'Fighter':     ['Who has earned my respect with their legit fighting skills?', 'Who did I beat when they challenged me to a shoot fight?', 'Who wants to add more entertainment to my repertoire?'],
    'Hardcore':    ['Who is jealous of my devoted fan base?', 'Who is willing to make me bleed?', 'Who has returned from an injury I gave them?'],
    'Jobber':      ["Who can't remember who I am?", 'Who was my tag team partner before they made it big?', "Who thinks they're too important to work with me?"],
    'Luchador':    ['Who allied with me on my first team?', 'Who is ripping off my high-flying style?', 'Who thinks I don\'t deserve my mask (or overall look)?'],
    'Luminary':    ['Who was instrumental to my early success?', 'Who wants me to just retire already?', 'Who is honored to be on my team?'],
    'Manager':     ['Who do I manage/accompany to the ring?', 'Who is trying to undermine me with my client?', 'Who turned on me when I was their Manager?'],
    'Monster':     ['Who is legitimately terrified of me?', "Who's made me look weak?", 'Who helps me come up with new directions for my character?'],
    'Provocateur': ['Who saw the potential in me to excel at being such an oddball?', "Who thinks I'm all smoke and no fire?", 'Who is trying to steal my approach?'],
    'Technician':  ['Who was holding me back as my tag team partner?', "Who's learning new skills by watching me in the ring?", "Who's jealous of my technical ability?"],
    'Veteran':     ['Who have I decided is killing the business with their performance?', 'Who is my prot\u00e9g\u00e9?', 'Who has no respect for all the work I\'ve put into this company?'],
}

# Move names: bonus non-FM first, then all choices (in JSON order)
MOVES = {
    'Ace':         ['Carry the Company', 'Fighting Spirit', 'Amazing Entrance', 'Strong Style'],
    'Anointed':    ['Picture Perfect', 'Special Snowflake', 'Always Learning', 'I Am the Future'],
    'Anti-Hero':   ['Rules? What Rules?', 'Anything You Need To Do To Win', 'Mouth Of The People', 'Twitch The Curtain'],
    'Call-Up':     ['10-Year Veteran', 'Seen It All', 'Hungry', 'Reputation'],
    'Clown':       ['Funny is Money', 'Surprisingly Talented', 'Comic Relief', 'Celebrity Promo'],
    'Fighter':     ['Shoot Fighter', 'Martial Arts Training', "You know it's fake right?", 'Stare Down', 'Stretcher'],
    'Hardcore':    ['Tables and Ladder and Chairs, Oh My', 'High Pain Tolerance', 'What a Weirdo', 'Master of Hardcore', 'Red Means Green', 'Are You Not Entertained'],
    'Jobber':      ['Career Wrestler', 'Ham-n-Egger', 'Sympathetic', 'Jobber to the Stars', 'Experienced Hand'],
    'Luchador':    ['Amazing Athleticism', 'El Hifo De...', 'Human Highlight Reel', 'Tradicional', 'Capitan del Parejas'],
    'Luminary':    ["I'm the Draw", 'Merchandising', 'Nemesis', 'High Expectations'],
    'Manager':     ['Meal Ticket', 'Mouth Piece', 'Never Unprepared', 'Brain for the Business', 'Backstage Politics', 'Loyal'],
    'Monster':     ['Prodigious Size', 'Surprising Agility', 'Intimidating', 'Not of this World', 'Shoot Kill'],
    'Provocateur': ['Strangely Captivating', 'Showstopper', 'Mind Games', 'Play to the Crowd'],
    'Technician':  ['Former Amateur Champion', 'Technical Expert', 'Excellence of Execution', 'Tweener', 'Sportsmanship'],
    'Veteran':     ['Ring General', 'Top of the Card', 'Respect the Business', "Bury 'em", "Put 'em Over"],
}

# Number of stat-adjustment checkboxes to skip at top of page 2 move section
MOVE_SKIP = {
    'Ace': 0, 'Anointed': 2, 'Anti-Hero': 1, 'Call-Up': 1, 'Clown': 2,
    'Fighter': 0, 'Hardcore': 0, 'Jobber': 0, 'Luchador': 1, 'Luminary': 0,
    'Manager': 0, 'Monster': 1, 'Provocateur': 2, 'Technician': 1, 'Veteran': 0,
}

# ---------- HELPERS ----------

def set_name(obj, name):
    obj[gen.NameObject('/T')] = gen.create_string_object(name)

# ---------- MAIN ----------

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

    changes = []

    # ========== PAGE 1 ==========
    page1 = writer.pages[0]
    ph1 = float(reader.pages[0].mediabox.height)
    annots1 = page1.get('/Annots', [])

    # Collect all widgets on page 1 in hailing/entrance y-range (top 150-285)
    hail_ent_range = []
    for ref in annots1:
        obj = ref.get_object()
        if obj.get('/Subtype') != '/Widget': continue
        rect = obj.get('/Rect')
        if not rect: continue
        rect = [float(x) for x in rect]
        left = rect[0]; top = ph1 - rect[3]
        w = rect[2] - rect[0]; h = rect[3] - rect[1]
        ft = str(obj.get('/FT', ''))
        name = str(obj.get('/T', ''))
        if 150 <= top <= 285:
            hail_ent_range.append((top, left, ft, w, h, name, obj))

    # Find the two "custom text" text fields (Elsewhere / Something Else)
    # They are /Tx fields with 50 < w < 200, sorted by x
    custom_tx = sorted(
        [(left, top, obj) for (top, left, ft, w, h, name, obj) in hail_ent_range
         if ft == '/Tx' and 50 < w < 200],
        key=lambda x: x[0]
    )
    if len(custom_tx) >= 2:
        elsewhere_x = custom_tx[0][0]
        # Rename Elsewhere and Something Else text fields
        set_name(custom_tx[0][2], 'Elsewhere')
        set_name(custom_tx[1][2], 'Something Else')
        changes.append('Elsewhere/SomethingElse')

        # Hailing CBs: /Btn with left < elsewhere_x - 5, sorted by top
        hail_cbs = sorted(
            [(top, obj) for (top, left, ft, w, h, name, obj) in hail_ent_range
             if ft == '/Btn' and left < elsewhere_x - 5],
            key=lambda x: x[0]
        )
        hail_names = HAILINGS.get(gimmick, []) + ['Elsewhere']
        if len(hail_cbs) == len(hail_names):
            for i, (top, obj) in enumerate(hail_cbs):
                set_name(obj, hail_names[i])
            changes.append(f'hailing x{len(hail_cbs)}')
        else:
            changes.append(f'WARN:hailing {len(hail_cbs)} cbs vs {len(hail_names)} names')

        # Entrance CBs: /Btn with left > elsewhere_x + 5, sorted by top
        ent_cbs = sorted(
            [(top, obj) for (top, left, ft, w, h, name, obj) in hail_ent_range
             if ft == '/Btn' and left > elsewhere_x + 5],
            key=lambda x: x[0]
        )
        ent_names = ENTRANCES.get(gimmick, []) + ['Something Else']
        if len(ent_cbs) == len(ent_names):
            for i, (top, obj) in enumerate(ent_cbs):
                set_name(obj, ent_names[i])
            changes.append(f'entrance x{len(ent_cbs)}')
        else:
            changes.append(f'WARN:entrance {len(ent_cbs)} cbs vs {len(ent_names)} names')
    else:
        changes.append(f'WARN:custom_tx count={len(custom_tx)}')

    # --- Question text fields (page 1, /Tx, w>150, top 505-645, not named Name) ---
    q_fields = []
    for ref in annots1:
        obj = ref.get_object()
        if obj.get('/Subtype') != '/Widget': continue
        rect = obj.get('/Rect')
        if not rect: continue
        rect = [float(x) for x in rect]
        top = ph1 - rect[3]
        w = rect[2] - rect[0]
        ft = str(obj.get('/FT', ''))
        name = str(obj.get('/T', ''))
        if ft == '/Tx' and w > 150 and 505 <= top <= 645 and name != 'Name':
            q_fields.append((top, obj))
    q_fields.sort(key=lambda x: x[0])

    q_names = QUESTIONS.get(gimmick, [])
    n_q = min(len(q_fields), 3)
    if n_q >= 1:
        for i in range(n_q):
            set_name(q_fields[i][1], q_names[i])
        changes.append(f'questions x{n_q}')
        if len(q_fields) > 3:
            changes.append(f'WARN:extra_q_fields={len(q_fields)}')
    else:
        changes.append(f'WARN:no question fields found')

    # ========== PAGE 2 ==========
    page2 = writer.pages[1]
    ph2 = float(reader.pages[1].mediabox.height)
    annots2 = page2.get('/Annots', [])

    move_cbs = []
    for ref in annots2:
        obj = ref.get_object()
        if obj.get('/Subtype') != '/Widget': continue
        ft = str(obj.get('/FT', ''))
        rect = obj.get('/Rect')
        if not rect: continue
        rect = [float(x) for x in rect]
        left = rect[0]; top = ph2 - rect[3]
        if ft == '/Btn' and left < 150 and top < 650:
            move_cbs.append((top, obj))
    move_cbs.sort(key=lambda x: x[0])

    skip = MOVE_SKIP.get(gimmick, 0)
    move_names = MOVES.get(gimmick, [])
    effective_cbs = move_cbs[skip:]

    if len(effective_cbs) == len(move_names):
        for i, (top, obj) in enumerate(effective_cbs):
            set_name(obj, move_names[i])
        changes.append(f'moves x{len(move_names)}')
    else:
        changes.append(f'WARN:moves {len(effective_cbs)} cbs vs {len(move_names)} expected (skip={skip}, total={len(move_cbs)})')

    with open(out_file, 'wb') as f:
        writer.write(f)
    os.replace(out_file, pdf_file)
    print(f'{gimmick}: {changes}')

print('\nDone!')
