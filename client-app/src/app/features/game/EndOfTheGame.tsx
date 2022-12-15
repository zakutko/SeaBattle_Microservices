import { observer } from "mobx-react";
import { Link } from "react-router-dom";
import { Button, Container } from "semantic-ui-react";
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
        <Container id="endOfTheGame">
            <Confetti id="confetti"/>
            <div className="endOfTheGame">
                <div>
                    <h1 className="header">
                        Congratulations!
                    </h1>
                    <h2 className="header">Winner is<span className="winner">{props.winnerUserName}</span>!</h2>
                    <Button id="endOfTheGame-btn" onClick={onClick} as={Link} to={"/"} color="purple">Back to home page</Button>
                </div>
            </div>
        </Container>
    )
})