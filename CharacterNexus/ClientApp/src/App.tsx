import { Route } from 'react-router';
import { Navigate } from 'react-router-dom';
import { useEffect, useState } from "react";
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';
import RulesetDashboard from './components/RulesetDashboard';
import {Ruleset} from './store/Ruleset';

import './custom.css'

const BASE_URL = `${window.location.protocol}//${window.location.host}`;

function App() {
    const [error, setError] = useState();
    const [isLoading, setIsLoading] = useState(false);
    const [rulesets, setRulesets] = useState<Ruleset[]>([]);
    const [selectedRuleset, setSelectedRuleset] = useState<Ruleset>();


    const updateRuleset = (ruleset: Ruleset) => {
      setSelectedRuleset(ruleset);
    };

    useEffect(() => {
      console.log(selectedRuleset);
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
        <Layout>
          <Route exact path="/">
            <Home rulesets={rulesets} onClick={updateRuleset} />
          </Route>
          {/* <Route path="*">
            <Home rulesets={rulesets} onClick={updateRuleset} />
          </Route> */}
          <Route path="/ruleset">
            <RulesetDashboard ruleset={selectedRuleset} />
          </Route>
          <Route path="/counter" component={Counter} />
          <Route path="/fetch-data/:startDateIndex?" component={FetchData} />
        </Layout>
      </>
    );
}

export default App;
