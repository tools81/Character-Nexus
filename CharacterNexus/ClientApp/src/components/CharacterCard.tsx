import { FaRegEdit } from "react-icons/fa";
import { FaRegFilePdf, FaRegTrashCan } from "react-icons/fa6";
import { SiFoundryvirtualtabletop } from "react-icons/si";
import { GiCharacter } from "react-icons/gi";
import { openInNewTab } from "../utils/openInNewTab";

const BASE_URL = `${window.location.protocol}//${window.location.host}`;

interface Props {
  id: string;
  name: string;
  image: string;
  level: number;
  levelName: string;
  details: string;
  characterSheet: string;
  onClick: () => void;
  onDelete: (name: string) => void;
  onEdit: (name: string) => void;
}

const CharacterCard = ({ id, name, image, level, levelName, details, characterSheet, onClick, onDelete, onEdit }: Props) => {
  return (
    <>
      <div className="col">
        <div
          className="card text-bg-dark border-secondary mb-3 slide-in"
          //onClick={() => onClick()}
        >
          <div className="card-header text-nowrap">{name}</div>
          <img src={image} className="card-img-top" alt="..." />
          <div className="card-body">
            <h5 className="card-title text-nowrap">{details}</h5>
          </div>
          <div className="card-footer">
            <div className="container">
              <div className="row">
                <div className="col-1 pointer">
                  <GiCharacter />
                </div>
                <div className="col-1 pointer">
                  <td onClick={() => onEdit(name)}>
                    <FaRegEdit />
                  </td>
                </div>
                <div className="col-1 pointer">
                  <td onClick={() => openInNewTab(characterSheet)}>
                    <FaRegFilePdf />
                  </td>
                </div>
                <div className="col-1 pointer">
                  <SiFoundryvirtualtabletop />
                </div>
                <div className="col-1 pointer">
                  <td onClick={() => onDelete(name)}>
                    <FaRegTrashCan />
                  </td>
                </div>
                <div className="col-sm text-end">
                  {levelName}: {level}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default CharacterCard;
