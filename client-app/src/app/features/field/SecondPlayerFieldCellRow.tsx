import { observer } from "mobx-react";
import CellNumber from "./CellNumber";
import SecondPlayerCell from "./SecondPlayerCell";

export default observer(function SecondPlayerFieldCellRow(props: any){
    const cellList = props.secondCellList;

    return (
        <>
        <div className="rowCell">
            <CellNumber number={props.number}/>
            {cellList.map(cell => (
                <>
                {cell.y === props.Y && 
                    <SecondPlayerCell cellState={cell.cellStateId}/>
                }
                </>
            ))}
        </div>
        </>
    )
})