export function toCamelCase(str: string): string {
  if (typeof str !== "string") {
    return str;
  }
  
  var camel = str.length === 0
    ? ""
    : str.charAt(0).toLowerCase() + str.slice(1);
  return camel.trim();
}