import { observer } from "mobx-react";
import "./field.css";
import RowCellNumber from "./RowCellNumber";
import FieldCellRow from "./FieldCellRow";

export default observer(function FieldCell(props: any){
    const cellList = props.cellList;
    return (
        <>
        <RowCellNumber />
        <FieldCellRow cellList={cellList} Y={1} number={1}/>
        <FieldCellRow cellList={cellList} Y={2} number={2}/>
        <FieldCellRow cellList={cellList} Y={3} number={3}/>
        <FieldCellRow cellList={cellList} Y={4} number={4}/>
        <FieldCellRow cellList={cellList} Y={5} number={5}/>
        <FieldCellRow cellList={cellList} Y={6} number={6}/>
        <FieldCellRow cellList={cellList} Y={7} number={7}/>
        <FieldCellRow cellList={cellList} Y={8} number={8}/>
        <FieldCellRow cellList={cellList} Y={9} number={9}/>
        <FieldCellRow cellList={cellList} Y={10} number={10}/>
        </>
    )
})