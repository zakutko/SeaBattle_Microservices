import { Card, CardActions, Typography } from "@mui/material";
import { observer } from "mobx-react";
import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { Button, CardContent, Container, Label } from "semantic-ui-react";
import agent from "../../api/agent";
import NavBar from "../../layout/NavBar";
import { GameList } from "../../models/gameList";
import "./game.css";

export default observer(function GameList() {
    const [gameList, setGameList] = useState<GameList[]>([]);
    const navigate = useNavigate();

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
            }, 3000)
            return () => clearInterval(interval);
        }
    }, [])

    const onClick = () => {
        navigate('/prepareGame');
    }

    const onClickJoin = (id: number) => {
        const token = localStorage.getItem('token');
        if(token){
            agent.Games.joinSecondPlayer(id, token);
        }
    }
    
    return (
        <>
        <Container id="gameList">
            <NavBar />
            <div className="gameList">

                <Button id="createGame-btn" className="createGameBtn" onClick={onClick} as={Link} to="/prepareGame" color="purple">Create Game</Button>

                <div id="cards" className="cards">
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
                                        <Button onClick={() => onClickJoin(game.id)}>Join the game</Button>
                                    </CardActions>
                                )}
                            </Card>
                        </>
                    ))}
                </div>
            </div>
        </Container>
        </>
    )
})