interface Props {
  renderField: any;
  items: any
  disabled?: boolean;
}

const FormListGroup = ({
  renderField,
  items,
  disabled
}: Props) => {
  return (
    <ul className="list-group">
      {items.map((childItem: any) => (
        <li
          className="list-group-item"
          id={childItem.component.name}
          key={childItem.component.name}
        >
          {renderField(childItem.component, false, !!disabled || !!childItem.component.disabled)}
          {renderField(childItem.text, false, !!disabled)}
        </li>
      ))}
    </ul>
  );
};

export default FormListGroup;
