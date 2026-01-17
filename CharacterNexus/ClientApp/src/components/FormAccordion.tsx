import React, { useRef } from 'react';
import 'bootstrap/dist/js/bootstrap.bundle';
import { usePrerequisites } from '../hooks/usePrerequisites';

interface FormAccordionProps {
  includeLabel: boolean;
  field: {
    label: string;
    id: string;
    items: {
      name: string;
      header: string;
      component: any;
    }[];
  };
  // renderField accepts (component, includeLabel?, disabled?)
  renderField: (component: any, includeLabel?: boolean, disabled?: boolean) => React.ReactNode;
  disabled?: boolean;
}

const FormAccordion: React.FC<FormAccordionProps> = ({ includeLabel, field, renderField, disabled }) => {
  const prereqSetRef = useRef<Set<string>>(new Set());

  // Build a schema where each accordion item's component is wrapped with an items array
  // so usePrerequisites can descend into group.items[].component and find prerequisites
  const childSchema = {
    fields: field.items.map((item: any) => ({
      name: item.name,
      items: [
        {
          name: item.name,
          component: item.component
        }
      ]
    }))
  };

  usePrerequisites(childSchema, prereqSetRef.current);

  return (
    <div className="mb-3">
      {includeLabel && (
        <>
          <label>{field.label}</label>
          <br />
        </>
      )}
      <div className="accordion" id={field.id}>
        {field.items.map((childItem: any) => (
          <div className="accordion-item" key={childItem.name}>
            <h2 className="accordion-header">
              <button
                className="accordion-button collapsed"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target={`#${childItem.name}`}
                aria-expanded="false"
                aria-controls={childItem.name}
              >
                {childItem.image && <img src={childItem.image} alt="" />}
                &nbsp;
                {childItem.header}
              </button>
            </h2>
            <div
              id={childItem.name}
              className="accordion-collapse collapse"
              data-bs-parent={`#${field.id}`}
            >
              <div className="accordion-body">
                {renderField(childItem.component, true, !!disabled || !!childItem.component.disabled)}
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default FormAccordion;