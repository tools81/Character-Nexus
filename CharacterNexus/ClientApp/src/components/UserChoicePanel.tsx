import { ReactNode, useEffect, useState } from "react";
import { UserChoices } from "../types/UserChoice";

interface Props {
  // Choices offered by the origin field; empty/undefined keeps the panel collapsed
  userChoices?: UserChoices;
  // Generated choice fields belonging to this origin
  choiceFields: any[];
  renderChoiceField: (field: any) => ReactNode;
}

/* =======================================================
   UserChoicePanel — collapsible container rendered below
   the field that produces user choices. Stays collapsed
   until that field is set to an option with choices,
   then expands and shows the choice inputs inline.
======================================================= */
const UserChoicePanel = ({ userChoices, choiceFields, renderChoiceField }: Props) => {
  const hasChoices = !!userChoices && userChoices.length > 0;
  const [expanded, setExpanded] = useState(hasChoices);

  // Re-expand whenever the origin field produces a new set of choices
  useEffect(() => {
    setExpanded(hasChoices);
  }, [userChoices, hasChoices]);

  const isOpen = hasChoices && expanded;

  return (
    <div className={`choice-panel${hasChoices ? " choice-panel--populated" : ""}`}>
      {hasChoices && (
        <button
          type="button"
          className="choice-panel__toggle"
          onClick={() => setExpanded(e => !e)}
          aria-expanded={isOpen}
        >
          <span>Choices</span>
          <span aria-hidden="true">{isOpen ? "▲" : "▼"}</span>
        </button>
      )}
      <div className={`choice-panel__collapse${isOpen ? " open" : ""}`}>
        <div className="choice-panel__inner">
          <div className="choice-panel__body">
            {hasChoices && userChoices!.map((item, index) => (
              <div key={`${item.type}-${index}`}>
                <p className="choice-panel__prompt">{item.label ?? `Choose ${item.count}`}</p>

                {choiceFields
                  .filter(field => field.choiceType === item.type)
                  .map(field => (
                    <ChoiceFieldWithDescription
                      key={field.id}
                      field={field}
                      renderChoiceField={renderChoiceField}
                    />
                  ))}

                {index < userChoices!.length - 1 && <hr className="my-3" />}
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

function ChoiceFieldWithDescription({ field, renderChoiceField }: {
  field: any;
  renderChoiceField: (field: any) => ReactNode;
}) {
  const [open, setOpen] = useState(false);

  return (
    <div>
      <div className="d-flex align-items-center gap-2">
        <div className="flex-grow-1">
          {renderChoiceField(field)}
        </div>
        {field.description && (
          <button
            type="button"
            className="btn btn-link btn-sm p-0 flex-shrink-0"
            onClick={() => setOpen(o => !o)}
            aria-expanded={open}
          >
            {open ? "▲" : "▼"}
          </button>
        )}
      </div>
      {field.description && open && (
        <small className="d-block mb-2 mt-1" dangerouslySetInnerHTML={{ __html: field.description }} />
      )}
    </div>
  );
}

export const hasUserChoiceOptions = (options?: any[]) =>
  !!options?.some(o => o.userChoices);

export default UserChoicePanel;
