import { useRef, useState, useEffect } from "react";

interface Props {
  renderField: (
    component: any,
    disabledMap: Record<string, boolean>,
    visibilityMap: Record<string, boolean>,
    isVisible: (fieldName: string) => boolean,
    includeLabel?: boolean
  ) => React.ReactNode;
  name: string;
  includeLabel: boolean;
  label: string;
  children: any[];
  disabledMap: Record<string, boolean>;
  visibilityMap: Record<string, boolean>;
  isVisible: (fieldName: string) => boolean;
}

const FormGroup = ({
  renderField,
  name,
  includeLabel,
  label,
  children,
  disabledMap,
  visibilityMap,
  isVisible
}: Props) => {
  const containerRef = useRef<HTMLDivElement>(null);
  const [cols, setCols] = useState<number | null>(null);
  const nonBreakCount = children.filter((c: any) => c.type !== "linebreak").length;

  useEffect(() => {
    const el = containerRef.current;
    if (!el) return;

    let rafId: number | null = null;
    let measuring = false;

    const compute = () => {
      if (measuring) return;
      measuring = true;

      const savedDisplay = el.style.display;
      const savedTemplate = el.style.gridTemplateColumns;

      el.style.display = "flex";
      el.style.gridTemplateColumns = "";

      const items = Array.from(el.querySelectorAll<HTMLElement>(":scope > .form-group"));

      let result: number | null = null;
      if (items.length > 0) {
        const firstTop = items[0].offsetTop;
        const itemsOnRow1 = items.filter(item => item.offsetTop === firstTop).length;
        if (itemsOnRow1 < nonBreakCount) {
          const rows = Math.ceil(nonBreakCount / itemsOnRow1);
          result = Math.ceil(nonBreakCount / rows);
        }
      }

      el.style.display = savedDisplay;
      el.style.gridTemplateColumns = savedTemplate;

      measuring = false;
      setCols(prev => prev === result ? prev : result);
    };

    const ro = new ResizeObserver(() => {
      if (rafId !== null) cancelAnimationFrame(rafId);
      rafId = requestAnimationFrame(compute);
    });

    ro.observe(el);
    compute();
    return () => {
      ro.disconnect();
      if (rafId !== null) cancelAnimationFrame(rafId);
    };
  }, [nonBreakCount]);

  const containerStyle = cols !== null
    ? { display: "grid", gridTemplateColumns: `repeat(${cols}, 1fr)` }
    : {};

  return (
    <>
      <label htmlFor={name} className="form-label">
        {label}
      </label>
      <br />

      <div ref={containerRef} className="input-group" style={containerStyle}>
        {children.map((childField: any, i: number) => {
          if (childField.type === "linebreak") {
            return cols !== null
              ? <div key={`lb-${i}`} style={{ gridColumn: "1 / -1" }} />
              : <div key={`lb-${i}`} className="w-100" />;
          }

          return (
            <div
              key={childField.id ?? childField.name}
              className="form-group p-2"
            >
              {includeLabel && childField.type !== "group" && (
                <>
                  <label>{childField.label}</label>
                  <br />
                </>
              )}

              {renderField(
                childField,
                disabledMap,
                visibilityMap,
                isVisible,
                false
              )}
            </div>
          );
        })}
      </div>
    </>
  );
};

export default FormGroup;
