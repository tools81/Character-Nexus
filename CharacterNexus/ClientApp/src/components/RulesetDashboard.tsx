import { connect } from "react-redux";
import ImageCard from "./ImageCard";
import { RouteComponentProps } from "react-router";
import React, { Component, useEffect, useState } from "react";
import { ApplicationState } from "../store";
import { Ruleset, RulesetState } from "../store/Ruleset";
import TextCard from "./TextCard";
import { CharacterSegment } from "../store/CharacterSegment";
import { useLocation } from "react-router-dom";

function getCharacter() {}
 
function newCharacter() {}

const BASE_URL = `${window.location.protocol}//${window.location.host}`;

const RulesetDashboard: React.FC = () => {
  let location = useLocation();
  const { ruleset } = location.state;
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

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Something went wrong fetching rulesets!</div>;
  }

  return (
    <>
      <div className="main-content">
        <h1>{ruleset.name}</h1>
        <div className="row row-cols-1 row-cols-md-4 g-4">
          <TextCard
            ruleset={ruleset}
            onClick={() => newCharacter()}
          >
            <h1>+</h1>
            <h5>Create New</h5>
          </TextCard>
          {characterSegments.map((character) => (
            <ImageCard
              ruleset={character.name}
              imgSrc={character.imageSource}
              onClick={() => getCharacter()}
            />
          ))}
        </div>
      </div>
    </>
  );
};

export default connect()(RulesetDashboard);

function fetchData(rulesetName: any, string: any) {
  throw new Error("Function not implemented.");
}
