import { observer } from "mobx-react"
import Cell from "./Cell"
import CellNumber from "./CellNumber"
import "./field.css"

export default observer(function FieldCellRow(props: any){
    const cellList = props.cellList;

    return (
        <>
        <div className="rowCell">
            <CellNumber number={props.number}/>
            {cellList.map(cell => (
                <>
                {cell.y === props.Y && 
                    <Cell key={cell.id} x={cell.x} y={cell.y} cellState={cell.cellStateId}/>
                }
                </>
            ))}
        </div>
        </>
    )
})