import { connect } from "react-redux";
import ImageCard from "./ImageCard";
import { RouteComponentProps } from "react-router";
import React, { Component, useEffect, useState } from "react";
import { ApplicationState } from "../store";
import { Ruleset } from "../store/Ruleset";
import TextCard from "./TextCard";
import { CharacterSegment } from "../store/CharacterSegment";

interface Props {
  ruleset: Ruleset | undefined;
}

interface CharacterSegmentState {
  data: CharacterSegment[];
  loading: boolean;
  error: string | null;
}

function getCharacter() {}
 
function newCharacter() {}

const BASE_URL = `${window.location.protocol}//${window.location.host}`;

class RulesetDashboard extends React.PureComponent<
  Props,
  CharacterSegmentState
> {
  constructor(props: Props) {
    super(props);
    this.state = {
      data: [],
      loading: true,
      error: null,
    };
  }

  componentDidMount(): void {
      this.fetchData();
  }

  fetchData() {
    fetch(
      `${BASE_URL}/api/ruleset/characters?ruleset=${encodeURIComponent(this.props.ruleset ? this.props.ruleset.name.trim() : "")}`
    )
      .then((response) => {
        if (!response.ok) {
          throw new Error("Network response was not ok " + response.statusText);
        }
        return response.json();
      })
      .then((data: CharacterSegment[]) => {
        this.setState({ data, loading: false });
      })
      .catch((error) => {
        this.setState({ error: error.message, loading: false });
      });
  }

  public render() {
    const { data, loading, error } = this.state;

    if (loading) {
      return <div>Loading...</div>;
    }

    if (error) {
      return <div>Error: {error}</div>;
    }

    // const [error, setError] = useState();
    // const [isLoading, setIsLoading] = useState(false);
    // const [characters, setCharacters] = useState<CharacterSegment[]>([]);

    // useEffect(() => {
    //   const fetchCharacters = async () => {
    //     setIsLoading(true);

    //     try {
    //       const response = await fetch(
    //         `${BASE_URL}/api/ruleset/characters?ruleset=` +
    //           encodeURIComponent(this.props.ruleset.name.trim())
    //       );
    //       const characters = (await response.json()) as CharacterSegment[];
    //       setCharacters(characters);
    //     } catch (e: any) {
    //       setError(e);
    //     } finally {
    //       setIsLoading(false);
    //     }
    //   };

    //   fetchCharacters();
    // }, []);

    // if (isLoading) {
    //   return <div>Loading...</div>;
    // }

    // if (error) {
    //   return <div>Something went wrong fetching characters!</div>;
    // }

    return (
      <>
        <div className="main-content">
          <h1>{this.props.ruleset ? this.props.ruleset.name : ""}</h1>
          <div className="row row-cols-1 row-cols-md-4 g-4">
            <TextCard
              ruleset={this.props.ruleset}
              onClick={() => newCharacter()}
            >
              <h1>+</h1>
              <h5>Create New</h5>
            </TextCard>
            {data.map((character) => (
              <ImageCard
                ruleset={character.name}
                imgSrc={character.imageSource}
                onClick={() => getCharacter()}
              />
            ))}
            {/* <ImageCard
              ruleset="Character 1"
              imgSrc="https://i.imgur.com/IIPNowN.png"
              onClick={() => getCharacter()}
            />
            <ImageCard
              ruleset="Character 2"
              imgSrc="https://i.imgur.com/FwyF9h4.png"
              onClick={() => getCharacter()}
            /> */}
          </div>
        </div>
      </>
    );
  }
}

export default connect()(RulesetDashboard);
