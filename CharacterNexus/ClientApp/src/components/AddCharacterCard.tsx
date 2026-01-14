interface AddCharacterCardProps {
  onClick: () => void;
}

const AddCharacterCard = ({ onClick }: AddCharacterCardProps) => {
  return (
    <>
        <div className="col">
            <div
                className="card text-bg-dark border-secondary mb-3 slide-in pointer"
                onClick={() => onClick()}
            >
                <div className="card-body">
                    <div className="plus-icon">+
                        {/* <h5 className="card-title text-nowrap">+</h5> */}
                    </div>
                </div>
                <div className="card-footer">
                    <span>Add a Character</span>
                </div>
            </div>
        </div>
    </>
  );
};

export default AddCharacterCard;