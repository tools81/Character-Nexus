import { connect } from 'react-redux';
import ImageCard from './ImageCard';
import { Ruleset } from '../store/Ruleset';

interface Props {
  rulesets: Ruleset[];
  onClick: (ruleset: Ruleset) => void;
}

const Home: React.FC<Props> = ({ rulesets, onClick }) => {
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
              onClick={() => onClick(ruleset)}
            />
          );
        })}
      </div>
    </div>
  );
};

export default connect()(Home);

