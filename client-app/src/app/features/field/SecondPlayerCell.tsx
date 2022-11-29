import { observer } from "mobx-react";
import "./field.css";

export default observer(function SecondPlayerCell(props: any) {
    return (
        <>
        {props.cellState === 1 &&
            <div className="cell"></div>
        }
        {props.cellState === 2 && 
            <div className="cell"></div>
        }
        {props.cellState === 3 && 
            <div className="cell cellOrange"></div>
        }
        {props.cellState === 4 && 
            <div className="cell cellPurple"></div>
        }
        {props.cellState === 5 &&
            <div className="cell"></div>
        }
        {props.cellState === 6 && 
            <div className="cell cellRed"></div>
        }
        </>
    )
})