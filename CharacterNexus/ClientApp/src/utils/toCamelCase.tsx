export function toCamelCase(str: string): string {
  var camel = str.length === 0
    ? ""
    : str.charAt(0).toLowerCase() + str.slice(1);
  return camel.trim();
}