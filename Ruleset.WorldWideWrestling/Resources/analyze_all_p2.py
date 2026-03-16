import pypdf

gimmicks = ['Anointed', 'Anti-Hero', 'Luchador', 'Technician', 'Provocateur']

for gimmick in gimmicks:
    pdf_file = f'WorldWideWrestling_Character_Sheet_{gimmick}.pdf'
    reader = pypdf.PdfReader(pdf_file)
    page = reader.pages[1]
    page_height = float(page.mediabox.height)

    annots = page.get('/Annots', [])
    all_buttons = []

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
        if ft == '/Btn':
            all_buttons.append((top, left, name))

    all_buttons.sort()
    print(f'\n{gimmick}: ALL page 2 buttons ({len(all_buttons)} total)')
    for top, left, name in all_buttons:
        print(f'  top={top:.0f} left={left:.0f} {name!r}')

    # Also show all page 2 text in left column (x 30-220), top 50-700
    texts = []
    def visitor(text, cm, tm, fd, fs):
        if not text.strip(): return
        x = tm[4]; y = float(page.mediabox.height) - tm[5]
        try: t = text.encode('ascii', 'replace').decode().strip()
        except: t = repr(text)
        if t and 30 <= x <= 220 and 50 <= y <= 700:
            texts.append((y, x, t))
    page.extract_text(visitor_text=visitor)
    texts.sort()
    print(f'  Text (left col):')
    for y, x, t in texts:
        print(f'    top={y:.0f} x={x:.0f} {t!r}')
