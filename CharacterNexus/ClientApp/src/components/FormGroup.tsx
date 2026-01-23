interface Props {
  renderField: (
      component: any,  
      disabledMap: Record<string, boolean>,
      includeLabel?: boolean,
    ) => React.ReactNode;
  name: string;
  includeLabel: boolean;
  label: string;
  children: any;  
  disabledMap: Record<string, boolean>;
}

const FormGroup = ({
  renderField,
  name,
  includeLabel,
  label,
  children,
  disabledMap
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
              {includeLabel && (
                <>
                  <label>{childField.label}</label>
                  <br />
                </>
              )}
              {renderField(childField, disabledMap, false)}
            </div>
        ))}
      </div>
    </>
  );
};

export default FormGroup;
