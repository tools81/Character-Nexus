import React, { useMemo, useState } from "react";
import { useWatch } from "react-hook-form";
import { useMediaQuery } from "../hooks/useMediaQuery";

interface PinnedField {
  name: string;
  label: string;
  count?: number;
}

function collectPinnedFields(fields: any[]): PinnedField[] {
  const result: PinnedField[] = [];
  for (const field of fields) {
    if (field.pinnedStat && field.name) {
      result.push({ name: field.name, label: field.label ?? field.name, count: field.count });
    }
    if (Array.isArray(field.items)) {
      collectPinnedFields(field.items).forEach(f => result.push(f));
    }
    if (field.component?.items) {
      collectPinnedFields(field.component.items).forEach(f => result.push(f));
    }
  }
  return result;
}

function resolveDisplayValue(v: any): string | null {
  if (v == null || v === "" || Number.isNaN(v)) return null;
  if (Array.isArray(v)) {
    if (v.length === 0) return null;
    return v.map((item: any) =>
      typeof item === "object" && item !== null ? item.value : item
    ).join(", ");
  }
  if (typeof v === "object") return v.value ?? null;
  return String(v);
}

interface StatsBarProps {
  schema: any;
  onInstructionsToggle?: () => void;
  instructionsOpen?: boolean;
}

const StatsBar: React.FC<StatsBarProps> = ({ schema, onInstructionsToggle, instructionsOpen }) => {
  const isMobile = useMediaQuery("(max-width: 767px)");
  const [mobileExpanded, setMobileExpanded] = useState(false);

  const pinnedFields = useMemo(
    () => (schema?.fields ? collectPinnedFields(schema.fields) : []),
    [schema]
  );

  const values = useWatch({ name: pinnedFields.map(f => f.name) });

  if (pinnedFields.length === 0) return null;

  const statTiles = pinnedFields.map((field, i) => {
    const display = resolveDisplayValue(values[i]);
    if (display === null) return null;
    let valueDisplay: React.ReactNode;
    if (field.count != null) {
      const numVal = Number(display);
      if (!isNaN(numVal)) {
        valueDisplay = (
          <span className="stats-bar__pips">
            {Array.from({ length: field.count }, (_, j) => (
              <span key={j} className={`stats-bar__pip${j < numVal ? " stats-bar__pip--filled" : ""}`} />
            ))}
          </span>
        );
      } else {
        valueDisplay = display;
      }
    } else {
      valueDisplay = display;
    }
    return (
      <div key={field.name} className="stats-bar__tile">
        <span className="stats-bar__label">{field.label}</span>
        <span className="stats-bar__value">{valueDisplay}</span>
      </div>
    );
  }).filter(Boolean);

  // On mobile show only first 4 stats unless expanded
  const visibleTiles = isMobile && !mobileExpanded ? statTiles.slice(0, 4) : statTiles;
  const hasMore = isMobile && statTiles.length > 4;

  return (
    <div className="stats-bar">
      <div className="stats-bar__tiles">
        {visibleTiles}
        {hasMore && (
          <button
            type="button"
            className="stats-bar__expand"
            onClick={() => setMobileExpanded(v => !v)}
            aria-label={mobileExpanded ? "Show fewer stats" : "Show more stats"}
          >
            {mobileExpanded ? "▲" : "▼"}
          </button>
        )}
      </div>
      {onInstructionsToggle && (
        <button
          type="button"
          className={`stats-bar__instructions-btn${instructionsOpen ? " active" : ""}`}
          onClick={onInstructionsToggle}
          aria-label="Toggle ruleset instructions"
        >
          ⓘ
        </button>
      )}
    </div>
  );
};

export default StatsBar;
