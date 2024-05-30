import { BrowserRouter, useNavigate, Route, Routes } from 'react-router-dom';
import { useEffect, useState } from "react";
import Home from './components/Home';
import RulesetDashboard from './components/RulesetDashboard';
import {Ruleset} from './store/Ruleset';
import { Container } from "reactstrap";
import NavMenu from "./components/NavMenu";

import './custom.css'

const BASE_URL = `${window.location.protocol}//${window.location.host}`;

function App() {
  const navigate = useNavigate();
  const [error, setError] = useState();
  const [isLoading, setIsLoading] = useState(false);
  const [rulesets, setRulesets] = useState<Ruleset[]>([]);
  const [selectedRuleset, setSelectedRuleset] = useState<Ruleset>();


  const updateRuleset = (ruleset: Ruleset) => {
    setSelectedRuleset(ruleset);
  };

  useEffect(() => {
    console.log(selectedRuleset);
    if (selectedRuleset !== undefined) {
      navigate("/ruleset", {state: {ruleset: selectedRuleset}});
    }
  }, [selectedRuleset]);

  useEffect(() => {
      const fetchRulesets = async () => {
          setIsLoading(true);

          try {
            const response = await fetch(`${BASE_URL}/api/ruleset/rulesets`);
            const rulesets = await response.json() as Ruleset[];
            setRulesets(rulesets);
          } catch (e: any) {
              setError(e);
          } finally {
              setIsLoading(false);
          }
      };

      if (rulesets.length < 1) {
        fetchRulesets();
      }
  }, []);

  if (isLoading) {
      return <div>Loading...</div>;
  }

  if (error) {
      return <div>Something went wrong fetching rulesets!</div>;
  }

  return (
    <>
      <NavMenu />
      <Container>
        <Routes>
          <Route path="/" element={<Home rulesets={rulesets} onClick={updateRuleset} />} />
          {/* <Route path="*">
        <Home rulesets={rulesets} onClick={updateRuleset} />
      </Route> */}
          <Route path="/ruleset" element={<RulesetDashboard />} />
        </Routes>
      </Container>
    </>
  );
}

export default App;
