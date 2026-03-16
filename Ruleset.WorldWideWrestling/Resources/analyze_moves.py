import pypdf
import sys

# Only look at mismatched gimmicks (or all)
gimmicks = [
    'Ace', 'Anointed', 'Anti-Hero', 'Call-Up', 'Clown', 'Fighter',
    'Hardcore', 'Jobber', 'Luchador', 'Luminary', 'Manager', 'Monster',
    'Provocateur', 'Technician', 'Veteran'
]

for gimmick in gimmicks:
    pdf_file = f'WorldWideWrestling_Character_Sheet_{gimmick}.pdf'
    reader = pypdf.PdfReader(pdf_file)

    page = reader.pages[1]  # page 2
    page_height = float(page.mediabox.height)

    annots = page.get('/Annots', [])
    move_cbs = []

    for annot_ref in annots:
        obj = annot_ref.get_object()
        if obj.get('/Subtype') != '/Widget':
            continue
        ft = str(obj.get('/FT', ''))
        rect = obj.get('/Rect')
        if not rect:
            continue
        rect = [float(x) for x in rect]
        left = rect[0]
        top = page_height - rect[3]
        name = str(obj.get('/T', ''))

        if ft == '/Btn' and left < 150 and top < 650:
            move_cbs.append((top, left, name))

    move_cbs.sort()
    print(f'{gimmick}: {len(move_cbs)} move checkboxes')
    for top, left, name in move_cbs:
        print(f'  top={top:.0f} left={left:.0f} name={name!r}')

    # Also extract text from the moves section of page 2
    # Get text with coordinates using visitor
    texts = []
    def visitor(text, cm, tm, font_dict, font_size):
        if not text.strip():
            return
        # tm is the current transformation matrix; tm[4]=x, tm[5]=y
        x = tm[4]
        y = float(page.mediabox.height) - tm[5]
        if 50 < y < 700 and x < 400:
            try:
                texts.append((y, x, text.encode('ascii', errors='replace').decode()))
            except:
                texts.append((y, x, repr(text)))

    try:
        page.extract_text(visitor_text=visitor)
        texts.sort()
        # Only print text in the moves section (top roughly 50-350)
        print(f'  Page 2 text (top 50-450):')
        for y, x, t in texts:
            if 50 <= y <= 450:
                print(f'    top={y:.0f} left={x:.0f} text={t!r}')
    except Exception as e:
        print(f'  Error extracting text: {e}')
    print()
