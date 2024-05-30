import { Ruleset } from "../store/Ruleset";

interface Props {
  ruleset: Ruleset | undefined;
  children: React.ReactNode;
  onClick: () => void;
}

const TextCard = ({ ruleset, children, onClick }: Props) => {
  return (
    <>
      <div className="col">
        <div
          className="card text-bg-dark border-secondary text-center mb-3 slide-in"
          onClick={() => onClick}
        >
          <div className="card-body">
            {children}            
          </div>
        </div>
      </div>
    </>
  );
};

export default TextCard;
