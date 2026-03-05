interface Props {
  onClick: () => void;
}

const DiceRollButton = ({ onClick }: Props) => (
  <button
    type="button"
    onClick={onClick}
    className="dice-roll-btn"
    title="Random"
    style={{ background: "none", border: "none", cursor: "pointer", fontSize: "1.25rem", padding: "0 4px" }}
  >
    &#127922;
  </button>
);

export default DiceRollButton;
