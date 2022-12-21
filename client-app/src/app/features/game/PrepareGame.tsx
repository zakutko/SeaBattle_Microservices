import { AxiosError } from "axios";
import { observer } from "mobx-react";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import { Button, Container } from "semantic-ui-react";
import agent from "../../api/agent";
import { CellList } from "../../models/cellsList";
import FieldCell from "../field/FieldCell";
import FieldForm from "../field/FieldForm";
import "./game.css";

export default observer(function PrepareGame()
{
    const navigate = useNavigate();
    const [message, setMessage] = useState("");
    const [cellList, setCellList] = useState<CellList[]>([]);
    const [isGameOwner, setIsGameOwner] = useState(true);
    const [isSecondPlayerConnected, setIsSecondPlayerConnected] = useState(false);

    useEffect(() =>
    {
        const token = localStorage.getItem('token');
        if (token)
        {
            agent.Games.isGameOwner(token).then(response =>
            {
                setIsGameOwner(response.isGameOwner);
                setIsSecondPlayerConnected(response.isSecondPlayerConnected);
            });
            agent.Games.cells(token).then(response => {
                setCellList(response);
            })
            const interval = setInterval(() =>
            {
                agent.Games.isGameOwner(token).then(response =>
                {
                    setIsGameOwner(response.isGameOwner);
                    setIsSecondPlayerConnected(response.isSecondPlayerConnected);
                });
            }, 3000);
            return () => clearInterval(interval);
        }
    }, [])

    const onClickBuildAShip = () =>
    {
        const token = localStorage.getItem('token');
        if(token)
        {
            agent.Games.cells(token).then(response => {
                setCellList(response);
            });
        };
    }

    const onClick = () =>
    {
        const token = localStorage.getItem('token');
        if (token)
        {
            agent.Games.cells(token).then(response =>
            {
                setCellList(response);
            });
            agent.Games.isPlayerReady(token)
                .then(response => { setMessage(response.message) })
                .then(() => navigate('/game'))
                .catch(error => alert((error as AxiosError).response?.data))
        }
        console.log(message);
    }

    const onClickDelete = () =>
    {
        const token = localStorage.getItem('token');
        if (token)
        {
            agent.Games.deleteGame(token)
                .then(response => {
                    console.log(response);
                })
                .then(() => {
                    navigate("/gameList");
                });
        }
    }

    return (
        <>
        <Container id="prepareGame">
            <div className="plug">
                {isGameOwner && !isSecondPlayerConnected ? (
                    <Button id="deleteGame-btn" className="deleteGameBtn" onClick={onClickDelete} size="large">Delete game</Button>
                ) : (<div></div>)}
            </div>
            <div id="prepareGameCellList" className="prepareGame">
                <div className="field">
                    <FieldCell cellList={cellList} />
                </div>
                <div className="fieldForm">
                    <FieldForm onClickBuildAShip={onClickBuildAShip}/>
                </div>
            </div>
            <div id="prepareGame-btn" className="prepareGameButton">
                <Button className="prepareGameBtn" onClick={onClick} size="large">I'm ready</Button>
            </div>
        </Container>
        </>
    )
})