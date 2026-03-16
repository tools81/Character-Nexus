import pypdf

# For each gimmick, extract page 2, find move checkboxes,
# then find text directly to the right of each checkbox (same y range, x 40-200)
gimmicks = ['Anointed', 'Anti-Hero', 'Call-Up', 'Clown', 'Luchador', 'Monster', 'Provocateur', 'Technician']

for gimmick in gimmicks:
    pdf_file = f'WorldWideWrestling_Character_Sheet_{gimmick}.pdf'
    reader = pypdf.PdfReader(pdf_file)

    page = reader.pages[1]
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

    # Extract text with positions
    all_texts = []
    def visitor(text, cm, tm, font_dict, font_size):
        if not text.strip():
            return
        x = tm[4]
        y = float(page.mediabox.height) - tm[5]
        try:
            t = text.encode('ascii', errors='replace').decode().strip()
        except:
            t = repr(text)
        if t:
            all_texts.append((y, x, t))

    page.extract_text(visitor_text=visitor)

    print(f'\n=== {gimmick} ({len(move_cbs)} checkboxes) ===')
    for cb_top, cb_left, cb_name in move_cbs:
        # Only look at text in the LEFT column (x: 40 to 200) near the checkbox (y +/- 12px)
        nearby = [(y, x, t) for (y, x, t) in all_texts
                  if abs(y - cb_top) < 14 and 40 <= x <= 210]
        nearby.sort(key=lambda v: v[1])
        label = ' '.join(t for _, _, t in nearby)
        print(f'  CB {cb_top:.0f} ({cb_name}): {label!r}')
