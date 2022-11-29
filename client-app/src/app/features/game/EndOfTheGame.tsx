import { observer } from "mobx-react";
import { Link } from "react-router-dom";
import { Button } from "semantic-ui-react";
import "./game.css";
import Confetti from "react-confetti";
import agent from "../../api/agent";

export default observer(function EndOfTheGame(props: any){
    
    const onClick = () => {
        const token = localStorage.getItem('token');
        if (token) {
            agent.Games.clearingDb(token);
        }
    } 

    return (
        <>
        <Confetti />
        <div className="endOfTheGame">
            <div>
                <h1 className="header">
                    Congratulations!
                </h1>
                <h2 className="header">Winner is<span className="winner">{props.winnerUserName}</span>!</h2>
                <Button onClick={onClick} as={Link} to={"/"} color="purple">Back to home page</Button>
            </div>
        </div>
        </>
    )
})