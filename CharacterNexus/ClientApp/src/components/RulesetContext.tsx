import React, { createContext, useState, ReactNode, useContext, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Ruleset } from "../store/Ruleset";

interface RulesetState {
  ruleset: Ruleset;
  setRuleset: (ruleset: Ruleset) => void;
}

const defaultRuleset: Ruleset = {
  name: "",
  rulesetName: "",
  imageSource: "",
  logoSource: ""
};

const defaultState = {
  ruleset: defaultRuleset,
  setRuleset: () => {},
};

const RulesetContext = createContext<RulesetState>(defaultState);

export const RulesetProvider: React.FC<{ children: ReactNode }> = ({
  children,
}) => {
  const navigate = useNavigate();
  const [ruleset, setRuleset] = useState<Ruleset>(defaultRuleset);

  useEffect(() => {
    if (ruleset !== undefined && ruleset.name !== "") {
      navigate("/ruleset", { state: { ruleset: ruleset } });
    }
  }, [ruleset]);

  return (
    <RulesetContext.Provider value={{ ruleset, setRuleset }}>
      {children}
    </RulesetContext.Provider>
  );
};

export const useRulesetContext = () => useContext(RulesetContext);
