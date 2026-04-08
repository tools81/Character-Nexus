import React, { useMemo } from "react";
import { useWatch } from "react-hook-form";

interface PinnedField {
  name: string;
  label: string;
}

interface LeftCollapsiblePaneProps {
  schema: any;
  isOpen: boolean;
  onToggle: () => void;
}

function collectPinnedFields(fields: any[]): PinnedField[] {
  const result: PinnedField[] = [];
  for (const field of fields) {
    if (field.pinnedStat && field.name) {
      result.push({ name: field.name, label: field.label ?? field.name });
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

const HANDLE_SIZE = 32;
const panelWidth = "clamp(140px, 18vw, 240px)";

const LeftCollapsiblePane: React.FC<LeftCollapsiblePaneProps> = ({ schema, isOpen, onToggle }) => {
  const pinnedFields = useMemo(
    () => (schema?.fields ? collectPinnedFields(schema.fields) : []),
    [schema]
  );

  const values = useWatch({ name: pinnedFields.map(f => f.name) });

  if (pinnedFields.length === 0) return null;

  return (
    <div
      style={{
        position: "fixed",
        top: 0,
        left: 0,
        height: "100vh",
        width: `calc(${panelWidth} + ${HANDLE_SIZE}px)`,
        transform: isOpen ? "translateX(0)" : `translateX(calc(-1 * ${panelWidth}))`,
        transition: "transform 0.3s ease",
        zIndex: 1000,
      }}
    >
      {/* Panel */}
      <div
        style={{
          position: "absolute",
          left: 0,
          top: 0,
          width: panelWidth,
          height: "100%",
          backgroundColor: "#1a1a1a",
          display: "flex",
          flexDirection: "column",
          borderRight: "1px solid #ddd",
          boxShadow: "2px 0 6px rgba(0,0,0,0.1)",
        }}
      >
        {/* Header */}
        <div
          style={{
            height: 40,
            padding: "0 12px",
            display: "flex",
            alignItems: "center",
            borderBottom: "1px solid #eee",
            fontWeight: 600,
            fontSize: 14,
          }}
        >
          Stats
        </div>

        {/* Stat tiles */}
        <div style={{ padding: 12, overflowY: "auto", flex: 1 }}>
          {pinnedFields.map((field, i) => (
            <div
              key={field.name}
              style={{
                marginBottom: 16,
                textAlign: "center",
                padding: "8px 4px",
                borderBottom: "1px solid rgba(255,255,255,0.08)",
              }}
            >
              <div
                style={{
                  fontSize: 10,
                  textTransform: "uppercase",
                  letterSpacing: 1,
                  opacity: 0.6,
                  marginBottom: 2,
                }}
              >
                {field.label}
              </div>
              <div style={{ fontSize: 26, fontWeight: 700, lineHeight: 1 }}>
                {values[i] ?? "—"}
              </div>
            </div>
          ))}
        </div>
      </div>

      {/* Handle */}
      <div
        onClick={onToggle}
        style={{
          position: "absolute",
          right: 0,
          top: "50%",
          transform: "translateY(-50%)",
          width: HANDLE_SIZE,
          height: HANDLE_SIZE,
          backgroundColor: "#1a1a1a",
          border: "1px solid #ccc",
          borderLeft: "none",
          borderRadius: "0 6px 6px 0",
          cursor: "pointer",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          userSelect: "none",
          boxShadow: "1px 1px 4px rgba(0,0,0,0.15)",
        }}
        aria-label={isOpen ? "Collapse stats panel" : "Expand stats panel"}
      >
        {isOpen ? "❮" : "❯"}
      </div>
    </div>
  );
};

export default LeftCollapsiblePane;
