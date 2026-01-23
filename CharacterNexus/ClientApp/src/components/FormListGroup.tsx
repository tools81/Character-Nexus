interface Props {
  renderField: (
      component: any,  
      disabledMap: Record<string, boolean>,
      includeLabel?: boolean,
    ) => React.ReactNode;
  items: any
  disabledMap: Record<string, boolean>;
}

const FormListGroup = ({
  renderField,
  items,
  disabledMap
}: Props) => {
  return (
    <ul className="list-group">
      {items.map((childItem: any) => (
        <li
          className="list-group-item"
          id={childItem.component.name}
          key={childItem.component.name}
        >
          {renderField(childItem.component, disabledMap, false)}
          {renderField(childItem.text, disabledMap, false)}
        </li>
      ))}
    </ul>
  );
};

export default FormListGroup;
