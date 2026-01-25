import React from 'react';

interface FormAccordionProps {
  includeLabel: boolean;
  field: any;  
  renderField: (
    component: any,  
    disabledMap: Record<string, boolean>,
    visibilityMap: Record<string, boolean>,
    isVisible: (fieldName: string) => boolean,
    includeLabel?: boolean,
  ) => React.ReactNode;
  disabledMap: Record<string, boolean>;
  visibilityMap: Record<string, boolean>;
  isVisible: (fieldName: string) => boolean;
}

const FormAccordion: React.FC<FormAccordionProps> = ({
  includeLabel,
  field,
  renderField,
  disabledMap,
  visibilityMap,
  isVisible
}) => {

  return (
    <div className="mb-3">
      {includeLabel && <label>{field.label}</label>}

      <div className="accordion" id={field.id}>
        {field.items.map((item: any) => {
          const collapseId = `${field.id}-${item.name}`;

          return (
            <div className="accordion-item" key={item.name}>
              <h2 className="accordion-header">
                <button
                  className="accordion-button collapsed"
                  type="button"
                  data-bs-toggle="collapse"
                  data-bs-target={`#${collapseId}`}
                  aria-controls={collapseId}
                >
                  {item.header}
                </button>
              </h2>

              <div
                id={collapseId}
                className="accordion-collapse collapse"
                data-bs-parent={`#${field.id}`}
              >
                <div className="accordion-body">
                  {
                  renderField(
                    item.component, 
                    disabledMap,
                    visibilityMap,
                    isVisible,
                    true
                  )}
                </div>
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
};

export default FormAccordion;
