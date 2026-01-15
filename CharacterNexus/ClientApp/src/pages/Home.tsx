import React, { useEffect } from 'react';
import ImageCard from '../components/RulesetCard';
import { useAppDispatch, useAppSelector } from '../store/configureStore';
import { setRulesets, fetchRulesets, setCurrentRuleset } from '../store/slices/rulesetSlice';
import { useNavigate } from 'react-router-dom';

const Home: React.FC = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  // Get rulesets and loading/error state from Redux
  const { rulesets, isLoading, error } = useAppSelector((state) => state.ruleset);

  // Fetch rulesets on mount
  useEffect(() => {
    if (rulesets.length === 0) {
      dispatch(fetchRulesets());
    }
  }, [dispatch, rulesets.length]);

  const selectRuleset = (rulesetName: string) => {
    const ruleset = rulesets.find((r : any) => r.name === rulesetName);
    if (ruleset) {
      dispatch(setCurrentRuleset(ruleset));
      navigate('/ruleset', { state: { ruleset } });
    }
  };

  if (isLoading) return <div>Loading rulesets...</div>;
  if (error) return <div>Something went wrong fetching rulesets! {error}</div>;

  return (
    <div className="main-content">
      <h1>Choose a Ruleset</h1>
      <div className="row row-cols-1 row-cols-md-3 g-4">
        {rulesets.map((r : any) => (
          <ImageCard
            key={r.rulesetName}
            ruleset={r.name}
            imgSrc={r.imageSource}
            onClick={() => selectRuleset(r.name)}
          />
        ))}
      </div>
    </div>
  );
};

export default Home;
