import { connect } from "react-redux";
import React, { useEffect, useState } from "react";
import { CharacterSegment } from "../store/CharacterSegment";
import { useNavigate } from "react-router-dom";
import CharacterCard from "./CharacterCard";
import { useRulesetContext } from "./RulesetContext";

const BASE_URL = `${window.location.protocol}//${window.location.host}`;

const RulesetDashboard: React.FC = () => {
  const { ruleset, setRuleset } = useRulesetContext();
  const [error, setError] = useState();
  const [isLoading, setIsLoading] = useState(false);
  const [characterSegments, setCharacterSegments] = useState<CharacterSegment[]>([]);

  useEffect(() => {
      const fetchCharacters = async () => {
          setIsLoading(true);

          try {
            const response = await fetch(`${BASE_URL}/api/ruleset/characters?ruleset=${encodeURIComponent(ruleset.name)}`);
            const characters = await response.json() as CharacterSegment[];
            setCharacterSegments(characters);
          } catch (e: any) {
              setError(e);
          } finally {
              setIsLoading(false);
          }
      };

      if (characterSegments.length < 1) {
        fetchCharacters();
      }
  }, []);

  const navigate = useNavigate();

  function getCharacter() {}

  function newCharacter() {
    navigate("/charactereditor");
  }

  function editCharacter(name : string) {
    navigate(`/charactereditor/name/${name}`)
  }

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Something went wrong fetching characters!</div>;
  }

  return (
    <>
      <div className="d-flex mb-3">
        <img src={ruleset.logoSource} alt={ruleset.name} className="p-2" />
        <button
          type="button"
          className="btn btn-outline-secondary ms-auto p-2 h-25"
          onClick={() => newCharacter()}
        >
          + Create New
        </button>
      </div>
      <div className="row row-cols-1 row-cols-md-4 g-4">
        {characterSegments.map((character) => (
          <CharacterCard
            key={character.id}
            id={character.id}
            name={character.name}
            image={character.image}
            level={character.level}
            details={character.details}
            onClick={() => editCharacter(character.name)}
          />
        ))}
      </div>
    </>
  );
};

export default connect()(RulesetDashboard);
