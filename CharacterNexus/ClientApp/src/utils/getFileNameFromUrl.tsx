export function getFileNameFromUrl(url: string): string {
    // Use URL class to parse the URL
    const parsedUrl = new URL(url);
    // Extract the pathname and get the last segment as the file name
    return parsedUrl.pathname.split("/").pop() || "";
  };