import { Route, Routes } from 'react-router-dom';
import Home from './pages/Home';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import RulesetDashboard from './pages/RulesetDashboard';
import CharacterEditor from './pages/CharacterEditor';
import Container from 'react-bootstrap/Container';
import NavMenu from './components/NavMenu';
import './custom.css';

function App() {
  return (
    <>
      <NavMenu />
      <Container>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/ruleset" element={<RulesetDashboard />} />
          <Route path="/charactereditor" element={<CharacterEditor />} />
          <Route path="/charactereditor/name/:name" element={<CharacterEditor />} />
        </Routes>
      </Container>
    </>
  );
}

export default App;
