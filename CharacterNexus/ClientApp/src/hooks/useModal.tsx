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
      <div className="choice-modal-backdrop">
        <div className="choice-modal">
          {children({ userChoices: choice, close })}
        </div>
      </div>
    );
  }, [close]); // close is stable (empty deps), so Modal is always the same reference

  return { open, close, Modal };
}

