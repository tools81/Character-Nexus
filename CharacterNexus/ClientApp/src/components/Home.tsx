import { connect } from 'react-redux';
import ImageCard from './RulesetCard';
import { Ruleset } from '../store/Ruleset';
import { useEffect, useState } from 'react';
import { useRulesetContext } from './RulesetContext';

const Home: React.FC = () => {
  const BASE_URL = `${window.location.protocol}//${window.location.host}`;

  const [error, setError] = useState();
  const [isLoading, setIsLoading] = useState(false);
  const [rulesets, setRulesets] = useState<Ruleset[]>([]);

  const { ruleset, setRuleset } = useRulesetContext();

  const updateRuleset = (ruleset: Ruleset) => {
    setRuleset(ruleset);
  };

  useEffect(() => {
    const fetchRulesets = async () => {
      setIsLoading(true);

      try {
        const response = await fetch(`${BASE_URL}/api/ruleset/rulesets`);
        const rulesets = (await response.json()) as Ruleset[];
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
    <div className="main-content">
      <h1>Choose a Ruleset</h1>
      <div className="row row-cols-1 row-cols-md-3 g-4">
        {rulesets.map((ruleset) => {
          return (
            <ImageCard
              key = {ruleset.rulesetName}
              ruleset={ruleset.name}
              imgSrc={ruleset.imageSource}
              onClick={() => updateRuleset(ruleset)}
            />
          );
        })}
      </div>
    </div>
  );
};

export default connect()(Home);

