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
  return (
    <>
      <label htmlFor={name} className="form-label">
        {label}
      </label>
      <br />

      <div className="input-group">
        {children.map((childField: any) => {
          // ─────────────────────────────
          // LINE BREAK FIELD
          // ─────────────────────────────
          if (childField.type === "linebreak") {
            return (
              <br
                key={childField.id ?? childField.name}
              />
            );
          }

          // ─────────────────────────────
          // NORMAL FIELD
          // ─────────────────────────────
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
