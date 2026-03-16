import pypdf

gimmicks = ['Ace', 'Anointed', 'Fighter', 'Manager']

for gimmick in gimmicks:
    pdf_file = f'WorldWideWrestling_Character_Sheet_{gimmick}.pdf'
    reader = pypdf.PdfReader(pdf_file)
    page = reader.pages[0]
    page_height = float(page.mediabox.height)

    print(f'\n=== {gimmick} page 1 ===')
    annots = page.get('/Annots', [])
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
        # Show hailing area (top 185-260, left 20-60) and entrance area (left 200-240)
        if (185 <= top <= 260 and (15 <= left <= 60 or 200 <= left <= 240)) or \
           (500 <= top <= 660 and ft == '/Tx' and w > 100):
            print(f'  ft={ft} top={top:.0f} left={left:.0f} w={w:.0f} h={h:.0f} name={name!r}')
