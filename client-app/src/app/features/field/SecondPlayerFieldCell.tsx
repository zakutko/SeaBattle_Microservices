import { observer } from "mobx-react"
import RowCellNumber from "./RowCellNumber"
import SecondPlayerFieldCellRow from "./SecondPlayerFieldCellRow"

export default observer(function SecondPlayerFieldCell(props: any){
    const secondCellList = props.secondCellList;

    return (
        <>
        <RowCellNumber />
        <SecondPlayerFieldCellRow secondCellList={secondCellList} Y={1} number={1}/>
        <SecondPlayerFieldCellRow secondCellList={secondCellList} Y={2} number={2}/>
        <SecondPlayerFieldCellRow secondCellList={secondCellList} Y={3} number={3}/>
        <SecondPlayerFieldCellRow secondCellList={secondCellList} Y={4} number={4}/>
        <SecondPlayerFieldCellRow secondCellList={secondCellList} Y={5} number={5}/>
        <SecondPlayerFieldCellRow secondCellList={secondCellList} Y={6} number={6}/>
        <SecondPlayerFieldCellRow secondCellList={secondCellList} Y={7} number={7}/>
        <SecondPlayerFieldCellRow secondCellList={secondCellList} Y={8} number={8}/>
        <SecondPlayerFieldCellRow secondCellList={secondCellList} Y={9} number={9}/>
        <SecondPlayerFieldCellRow secondCellList={secondCellList} Y={10} number={10}/>
        </>
    )
})