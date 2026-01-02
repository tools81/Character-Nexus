interface Props {
  renderField: any;
  name: string;
  includeLabel: boolean;
  label: string;
  children: any;
  disabled?: boolean;
}

const FormGroup = ({
  renderField,
  name,
  includeLabel,
  label,
  children,
  disabled
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
              {renderField(childField, false, !!disabled)}
            </div>
        ))}
      </div>
    </>
  );
};

export default FormGroup;
