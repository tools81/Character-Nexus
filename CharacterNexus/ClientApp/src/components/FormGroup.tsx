interface Props {
  renderField: any;
  name: string;
  includeLabel: boolean;
  label: string;
  children: any;
}

const FormGroup = ({
  renderField,
  name,
  includeLabel,
  label,
  children
}: Props) => {
  return (
    <>
      <label htmlFor={name} className="form-label">
        {label}
      </label>
      <br />
      <div key={name} className="input-group">
        {children.map((childField: any) => (
          <div className="form-group p-2">{renderField(childField)}</div>
        ))}
      </div>
    </>
  );
};

export default FormGroup;
