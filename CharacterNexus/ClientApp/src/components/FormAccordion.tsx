import React from 'react';

interface FormAccordionProps {
  includeLabel: boolean;
  field: {
    label: string;
    id: string;
    items: {
      name: string;
      header: string;
      component: React.ReactNode;
    }[];
  };
  renderField: (component: React.ReactNode) => React.ReactNode;
}

const FormAccordion: React.FC<FormAccordionProps> = ({ includeLabel, field, renderField }) => {
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
                {renderField(childItem.component)}
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default FormAccordion;