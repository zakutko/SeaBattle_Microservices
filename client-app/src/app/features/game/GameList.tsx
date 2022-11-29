import { Card, CardActions, Typography } from "@mui/material";
import { observer } from "mobx-react";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { Button, CardContent, Label } from "semantic-ui-react";
import agent from "../../api/agent";
import NavBar from "../../layout/NavBar";
import { GameList } from "../../models/gameList";
import "./game.css";

export default observer(function GameList() {
    const [gameList, setGameList] = useState<GameList[]>([]);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token){
            agent.Games.games(token).then(response => {
                setGameList(response);
            });
            const interval = setInterval(() => {
                agent.Games.games(token).then(response => {
                    setGameList(response);
                });
            }, 1000)
            return () => clearInterval(interval);
        }
    }, [])

    const onClick = () => {
        const token = localStorage.getItem('token');
        if(token){
            agent.Games.createGame(token);
        }
    }

    const onClickJoin = (id: number) => {
        const token = localStorage.getItem('token');
        if(token){
            agent.Games.joinSecondPlayer(id, token);
        }
    }
    
    return (
        <>
        <NavBar />
        <div className="gameList">

            <Button className="createGameBtn" onClick={onClick} as={Link} to="/prepareGame" color="purple">Create Game</Button>

            <div className="cards">
                {gameList.map(game => (
                    <>
                        <Card id="card" key={game.id}>
                            <CardContent>
                                <Typography variant="h5" component="div">
                                    Game Id : <b>{game.id}</b>
                                </Typography>
                                <Typography variant="h5" component="div">
                                    Owner : <b>{game.firstPlayer}</b>
                                </Typography>
                                <Typography variant="h5" component="div">
                                    Game State : <b>{game.gameState}</b>
                                </Typography>
                                <Typography variant="h5" component="div">
                                    Number of Players : <b>{game.numberOfPlayers} of 2</b>
                                </Typography>
                            </CardContent>
                            {game.numberOfPlayers === 2 ? (
                                <Label className="card-label">The game already started!</Label>
                            ) : (
                                <CardActions>
                                    <Button as={Link} to="/prepareGame" onClick={() => onClickJoin(game.id)}>Join the game</Button>
                                </CardActions>
                            )}
                        </Card>
                    </>
                ))}
            </div>
        </div>
        </>
    )
})