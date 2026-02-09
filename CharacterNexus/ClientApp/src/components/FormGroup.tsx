interface Props {
  renderField: (
      component: any,  
      disabledMap: Record<string, boolean>,
      visibilityMap: Record<string, boolean>,
      isVisible: (fieldName: string) => boolean,
      includeLabel?: boolean,
    ) => React.ReactNode;
  name: string;
  includeLabel: boolean;
  label: string;
  children: any;  
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
      <div key={name} className="input-group">
        {children.map((childField: any) => (            
            <div className="form-group p-2">
              {includeLabel && childField.type !== "group" && (
                <>
                  <label>{childField.label}</label>
                  <br />
                </>
              )}
              {renderField(childField, disabledMap, visibilityMap, isVisible, false)}
            </div>
        ))}
      </div>
    </>
  );
};

export default FormGroup;
