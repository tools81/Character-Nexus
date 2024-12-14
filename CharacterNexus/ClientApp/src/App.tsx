import { Route, Routes } from 'react-router-dom';
import Home from './pages/Home';
import RulesetDashboard from './pages/RulesetDashboard';
import CharacterEditor from './pages/CharacterEditor';
import { RulesetProvider } from "./components/RulesetContext";
import { Container } from "reactstrap";
import NavMenu from "./components/NavMenu";
import './custom.css'

function App() {
  return (
    <>
      <NavMenu />
      <Container>
        <RulesetProvider>
          <Routes>
            <Route
              path="/"
              element={<Home />}
            />
            {/* <Route path="*">
        <Home rulesets={rulesets} onClick={updateRuleset} />
      </Route> */}
            <Route path="/ruleset" element={<RulesetDashboard />} />
            <Route path="/charactereditor" element={<CharacterEditor />} />
            <Route path="/charactereditor/name/:name" element={<CharacterEditor />} />
          </Routes>
        </RulesetProvider>
      </Container>
    </>
  );
}

export default App;
