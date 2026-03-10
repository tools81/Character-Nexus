import { useEffect } from "react";

export function useRulesetTheme(ruleset: { rulesetName: string; stylesheet: string } | null) {
  useEffect(() => {
    const css = ruleset?.stylesheet;
    if (!css) return;

    const themeClass = "theme-" + (ruleset.rulesetName ?? "")
      .replace("Ruleset.", "")
      .replace(/([A-Z])/g, "-$1")
      .toLowerCase()
      .replace(/^-/, "");

    const styleEl = document.createElement("style");
    styleEl.id = "ruleset-theme";
    styleEl.textContent = css;
    document.head.appendChild(styleEl);
    document.body.classList.add(themeClass);

    return () => {
      styleEl.remove();
      document.body.classList.remove(themeClass);
    };
  }, [ruleset?.stylesheet]);
}
