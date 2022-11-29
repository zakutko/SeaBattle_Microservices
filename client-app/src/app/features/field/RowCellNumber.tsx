import { observer } from "mobx-react"
import CellNumber from "./CellNumber"

export default observer(function RowCell(){
    return (
        <>
        <div className="rowCell">
            <CellNumber />
            <CellNumber number='1'/>
            <CellNumber number='2'/>
            <CellNumber number='3'/>
            <CellNumber number='4'/>
            <CellNumber number='5'/>
            <CellNumber number='6'/>
            <CellNumber number='7'/>
            <CellNumber number='8'/>
            <CellNumber number='9'/>
            <CellNumber number='10'/>
        </div>
        </>
    )
})