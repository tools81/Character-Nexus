interface Props {
  id: string;
  name: string;
  image: string;
  level: number;
  details: string;
  onClick: () => void;
}

const CharacterCard = ({ id, name, image, level, details, onClick }: Props) => {
  return (
    <>
      <div className="col">
        <div
          className="card text-bg-dark border-secondary mb-3 slide-in"
          onClick={() => onClick()}
        >
          <div className="card-header">{name}</div>
          <img src={image} className="card-img-top" alt="..." />
          <div className="card-body">
            <h5 className="card-title">{details}</h5>
          </div>
          <div className="card-footer text-end">Level: {level}</div>
        </div>
      </div>
    </>
  );
};

export default CharacterCard;
