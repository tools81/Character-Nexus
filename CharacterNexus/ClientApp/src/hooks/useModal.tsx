import { ReactNode, useCallback, useRef, useState } from "react";
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
  // Use a ref for modal state so the Modal component reference stays stable
  const stateRef = useRef({ isOpen: false, choice: null as UserChoices | null });
  const [, forceUpdate] = useState(0);

  const open = useCallback((value: UserChoices) => {
    stateRef.current = { isOpen: true, choice: value };
    forceUpdate(n => n + 1);
  }, []);

  const close = useCallback(() => {
    stateRef.current = { isOpen: false, choice: null };
    forceUpdate(n => n + 1);
  }, []);

  // Modal is memoized so its reference stays stable across parent re-renders.
  // A stable reference means React will NOT unmount/remount Modal children
  // when the parent component re-renders (e.g. from setBonusAdjustments).
  const Modal = useCallback<UseUserChoiceModalReturn["Modal"]>(({ children }) => {
    const { isOpen, choice } = stateRef.current;
    if (!isOpen || choice === null) return null;

    return (
      <div style={backdropStyle}>
        <div style={modalStyle}>
          {children({ userChoices: choice, close })}
        </div>
      </div>
    );
  }, [close]); // close is stable (empty deps), so Modal is always the same reference

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
  overflowY: "auto",
  background: "#110f38ff",
  padding: "1rem",
  borderRadius: "6px",
  minWidth: "320px",
  maxHeight: "400px"
};
