import pypdf

gimmicks = ['Ace', 'Anointed', 'Fighter', 'Manager', 'Luchador']

for gimmick in gimmicks:
    pdf_file = f'WorldWideWrestling_Character_Sheet_{gimmick}.pdf'
    reader = pypdf.PdfReader(pdf_file)
    page = reader.pages[0]
    page_height = float(page.mediabox.height)

    print(f'\n=== {gimmick} page 1 - all widgets top 150-450 ===')
    annots = page.get('/Annots', [])
    items = []
    for annot_ref in annots:
        obj = annot_ref.get_object()
        if obj.get('/Subtype') != '/Widget': continue
        ft = str(obj.get('/FT', ''))
        rect = obj.get('/Rect')
        if not rect: continue
        rect = [float(x) for x in rect]
        left = rect[0]; top = page_height - rect[3]
        w = rect[2] - rect[0]; h = rect[3] - rect[1]
        name = str(obj.get('/T', ''))
        if 150 <= top <= 450:
            items.append((top, left, ft, w, h, name))
    items.sort()
    for top, left, ft, w, h, name in items:
        print(f'  ft={ft} top={top:.0f} left={left:.0f} w={w:.0f} h={h:.0f} name={name!r}')
