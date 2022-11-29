import { observer } from "mobx-react";
import FieldCell from "../field/FieldCell";
import SecondPlayerFieldCell from "../field/SecondPlayerFieldCell";
import "./game.css";
import "../field/field.css";
import { useEffect, useState } from "react";
import agent from "../../api/agent";
import GameFieldForm from "./GameFieldForm";
import EndOfTheGame from "./EndOfTheGame";
import { CellList } from "../../models/cellsList";

export default observer(function Game()
{
    const [numberOfReadyPlayers, setNumberOfReadyPlayers] = useState(0);
    const [isHit, setIsHit] = useState(true);
    const [isEndOfTheGame, setIsEndOfTheGame] = useState(false);
    const [winnerUserName, setWinnerUserName] = useState("");
    const [cellList, setCellList] = useState<CellList[]>([]);
    const [secondCellList, setSecondCellList] = useState<CellList[]>([]);

    useEffect(() =>
    {
        const token = localStorage.getItem('token');
        if (token)
        {
            agent.Games.cells(token).then(response =>
            {
                setCellList(response);
            });
            agent.Games.secondPlayerCells(token).then(response =>
            {
                setSecondCellList(response);
            });
            const interval = setInterval(() =>
            {
                agent.Games.numberOfReadyPlayers(token).then(response =>
                {
                    setNumberOfReadyPlayers(response.numberOfReadyPlayers);
                });
                agent.Games.priopity(token).then(response =>
                {
                    setIsHit(response.isHit);
                });
                agent.Games.endOfTheGame(token).then(response =>
                {
                    setWinnerUserName(response.winnerUserName);
                    setIsEndOfTheGame(response.isEndOfTheGame);
                });
                agent.Games.cells(token).then(response =>
                {
                    setCellList(response);
                });
                agent.Games.secondPlayerCells(token).then(response =>
                {
                    setSecondCellList(response);
                });
            }, 1500);
            return () => clearInterval(interval);
        }
    }, [])

    return isEndOfTheGame ? (<EndOfTheGame winnerUserName={winnerUserName} />) : (
        <>
            <div>
                <div className="help-colors">
                    <div className="block color-purple">
                        <p>Miss</p>
                    </div>
                    <div className="block color-orange">
                        <p>Hit</p>
                    </div>
                    <div className="block color-red">
                        <p>Destroyed</p>
                    </div>
                </div>
                <div className="game">
                    <div className="field">
                        <FieldCell cellList={cellList} />
                    </div>
                    <div className="field">
                        <SecondPlayerFieldCell secondCellList={secondCellList} />
                    </div>
                </div>
                <div className="gameFieldForm">
                    {numberOfReadyPlayers === 1 &&
                        <p className="waiting">Waiting an opponent!</p>
                    }
                    {numberOfReadyPlayers === 2 &&
                        <>
                            {isHit ? (<GameFieldForm />) : (<p className="waiting">Enemy fire!</p>)}
                        </>
                    }
                </div>
            </div>
        </>
    )
})