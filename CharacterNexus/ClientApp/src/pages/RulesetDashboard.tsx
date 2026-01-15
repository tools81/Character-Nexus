import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import CharacterCard from '../components/CharacterCard';
import AddCharacterCard from '../components/AddCharacterCard';
import { useAppSelector, useAppDispatch } from '../store/configureStore';
import { fetchCharacters, deleteCharacter, CharacterSegment } from '../store/slices/characterSegmentSlice';

const RulesetDashboard: React.FC = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  // Read current ruleset from Redux
  const { currentRuleset } = useAppSelector((state) => state.ruleset);
  const { characterSegments, isLoading, error } = useAppSelector((state) => state.characterSegment);

  // Fetch characters for the current ruleset on mount or when ruleset changes
  useEffect(() => {
    if (currentRuleset && characterSegments.length === 0) {
      dispatch(fetchCharacters(currentRuleset.name));
    }
  }, [dispatch, currentRuleset, characterSegments.length]);

  const newCharacter = () => navigate('/charactereditor');
  const editCharacter = (name: string) => navigate(`/charactereditor?character=${name}`);
  const deleteCharacterHandler = (id: string) =>
    currentRuleset && dispatch(deleteCharacter({ id, rulesetName: currentRuleset.name }));

  if (!currentRuleset) return <div>No ruleset selected.</div>;
  if (isLoading) return <div>Loading characters...</div>;
  if (error) return <div>{error}</div>;

  return (
    <>
      <div className="d-flex mb-3">
        <img src={currentRuleset.logoSource} alt={currentRuleset.rulesetName} className="p-2" />
      </div>
      <div className="row row-cols-1 row-cols-md-4 g-4">
        <AddCharacterCard onClick={newCharacter} />
        {characterSegments.map((character: CharacterSegment) => (
          <CharacterCard
            key={character.id}
            id={character.id}
            name={character.name}
            image={character.image}
            level={character.level}
            levelName={character.levelName}
            details={character.details}
            characterSheet={character.characterSheet}
            onClick={() => editCharacter(character.name)}
            onEdit={() => editCharacter(character.name)}
            onDelete={() => deleteCharacterHandler(character.id)}
          />
        ))}
      </div>
    </>
  );
};

export default RulesetDashboard;
