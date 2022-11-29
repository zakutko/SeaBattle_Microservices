import { observer } from "mobx-react";
import "./field.css";

export default observer(function CellNumber(props: any) {
    return (
        <div className="cellNumber"><p>{props.number}</p></div>
    )
})