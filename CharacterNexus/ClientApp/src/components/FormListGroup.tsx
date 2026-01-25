interface Props {
  renderField: (
      component: any,  
      disabledMap: Record<string, boolean>,
      visibilityMap: Record<string, boolean>,
      isVisible: (fieldName: string) => boolean,
      includeLabel?: boolean,
    ) => React.ReactNode;
  items: any
  disabledMap: Record<string, boolean>;
  visibilityMap: Record<string, boolean>;
  isVisible: (fieldName: string) => boolean;
}

const FormListGroup = ({
  renderField,
  items,
  disabledMap,
  visibilityMap,
  isVisible
}: Props) => {
  return (
    <ul className="list-group">
      {items.map((childItem: any) => (
        <li
          className="list-group-item"
          id={childItem.component.name}
          key={childItem.component.name}
        >
          {renderField(childItem.component, disabledMap, visibilityMap, isVisible, false)}
          {renderField(childItem.text, disabledMap, visibilityMap, isVisible, false)}
        </li>
      ))}
    </ul>
  );
};

export default FormListGroup;
