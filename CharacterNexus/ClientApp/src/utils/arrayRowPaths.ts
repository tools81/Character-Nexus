// Helpers for field paths that point into a row of a form array,
// e.g. "weapons.1.value", "weapons.1.mods.0" or "choice.skills.weapons.1.value.0".

const escapeRegExp = (s: string) => s.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");

const rowPattern = (arrayName: string) =>
  new RegExp(`(^|\\.)${escapeRegExp(arrayName)}\\.(\\d+)\\.`);

// Row index of arrayName referenced by path, or null if the path isn't inside a row
export const rowIndexIn = (path: string | undefined, arrayName: string): number | null => {
  const match = rowPattern(arrayName).exec(path ?? "");
  return match ? Number(match[2]) : null;
};

// Renumber a path for rows after removedIndex, which move up by one
export const shiftRowPath = (path: string, arrayName: string, removedIndex: number): string =>
  path.replace(rowPattern(arrayName), (full, prefix: string, index: string) =>
    Number(index) > removedIndex ? `${prefix}${arrayName}.${Number(index) - 1}.` : full
  );
