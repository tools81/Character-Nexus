import { ReactNode, useCallback, useState } from "react";
import { UserChoices } from "../types/UserChoice";

export interface UseUserChoiceModalReturn {
  open: (choice: UserChoices) => void;
  close: () => void;
  Modal: (props: {
    children: (args: {
      userChoices: UserChoices;
      close: () => void;
    }) => ReactNode;
  }) => JSX.Element | null;
}

export function useModal(): UseUserChoiceModalReturn {
  const [isOpen, setIsOpen] = useState(false);
  const [choice, setChoice] = useState<UserChoices | null>(null);

  const open = useCallback((value: UserChoices) => {
    setChoice(value);
    setIsOpen(true);
  }, []);

  const close = useCallback(() => {
    setIsOpen(false);
    setChoice(null);
  }, []);

  const Modal: UseUserChoiceModalReturn["Modal"] = ({ children }) => {
    if (!isOpen || choice === null) return null;

    return (
      <div style={backdropStyle}>
        <div style={modalStyle}>
          {children({ userChoices: choice, close })}
        </div>
      </div>
    );
  };

  return { open, close, Modal };
}

const backdropStyle: React.CSSProperties = {
  position: "fixed",
  inset: 0,
  background: "rgba(129, 127, 127, 0.33)",
  display: "flex",
  justifyContent: "center",
  alignItems: "center",
};

const modalStyle: React.CSSProperties = {
  background: "#110f38ff",
  padding: "1rem",
  borderRadius: "6px",
  minWidth: "320px",
};
