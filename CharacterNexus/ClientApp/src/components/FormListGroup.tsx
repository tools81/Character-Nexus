interface Props {
  renderField: any;
  items: any
}

const FormListGroup = ({
  renderField,
  items
}: Props) => {
  return (
    <ul className="list-group">
      {items.map((childItem: any) => (
        <li
          className="list-group-item"
          id={childItem.component.name}
          key={childItem.component.name}
        >
          {renderField(childItem.component)}
          {renderField(childItem.text)}
        </li>
      ))}
    </ul>
  );
};

export default FormListGroup;
