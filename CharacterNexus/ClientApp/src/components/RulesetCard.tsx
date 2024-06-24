interface Props {
  ruleset: string;
  imgSrc: string;
  onClick: () => void;
}

const RulesetCard = ({ ruleset, imgSrc, onClick }: Props) => {
  return (
    <>
      <div className="col">
        <div
          className="card text-bg-dark border-secondary mb-3 slide-in"
          onClick={() => onClick()}
        >
          <img src={imgSrc} className="card-img-top" alt="..." />
          <div className="card-body">
            <h5 className="card-title">{ruleset}</h5>
          </div>
        </div>
      </div>
    </>
  );
};

export default RulesetCard;
