import React, { useState } from "react";

interface RightCollapsiblePaneProps {
  htmlContent: string;
  title?: string;
}

const HANDLE_SIZE = 32;

const RightCollapsiblePane: React.FC<RightCollapsiblePaneProps> = ({
  htmlContent,
  title = "Details",
}) => {
  const [isOpen, setIsOpen] = useState(false);

  const panelWidth = "clamp(320px, 33vw, 100vw)";

  return (
    <div
      style={{
        position: "fixed",
        top: 0,
        right: 0,
        height: "100vh",
        width: `calc(${panelWidth} + ${HANDLE_SIZE}px)`,
        transform: isOpen ? "translateX(0)" : `translateX(${panelWidth})`,
        transition: "transform 0.3s ease",
        zIndex: 1000,
      }}
    >
      {/* Handle (inside sliding container) */}
      <div
        onClick={() => setIsOpen((v) => !v)}
        style={{
          position: "absolute",
          left: 0,
          top: "50%",
          transform: "translateY(-50%)",
          width: HANDLE_SIZE,
          height: HANDLE_SIZE,
          backgroundColor: "#1a1a1a",
          border: "1px solid #ccc",
          borderRight: "none",
          borderRadius: "6px 0 0 6px",
          cursor: "pointer",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          userSelect: "none",
          boxShadow: "-1px 1px 4px rgba(0,0,0,0.15)",
        }}
        aria-label={isOpen ? "Collapse panel" : "Expand panel"}
      >
        {isOpen ? "❯" : "❮"}
      </div>

      {/* Panel */}
      <div
        style={{
          position: "absolute",
          right: 0,
          top: 0,
          width: panelWidth,
          height: "100%",
          backgroundColor: "#1a1a1a",
          display: "flex",
          flexDirection: "column",
          borderLeft: "1px solid #ddd",
          boxShadow: "-2px 0 6px rgba(0,0,0,0.1)",
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
          {title}
        </div>

        {/* Content */}
        <div
          style={{
            padding: 12,
            overflowY: "auto",
            fontSize: 14,
            lineHeight: 1.5,
            flex: 1,
          }}
          dangerouslySetInnerHTML={{ __html: htmlContent }}
        />
      </div>
    </div>
  );
};

export default RightCollapsiblePane;
