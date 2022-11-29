import { observer } from "mobx-react";
import { useEffect, useState } from "react";
import agent from "../../api/agent";
import { TopPlayers } from "../../models/topPlayers";
import "./winners.css";

export default observer(function Winners()
{
    const [topPlayers, setTopPlayers] = useState<TopPlayers>();

    useEffect(() =>
    {
        agent.GameHistories.topPlayers().then(response =>
        {
            setTopPlayers(response);
        });
        const interval = setInterval(() =>
        {
            agent.GameHistories.topPlayers().then(response =>
            {
                setTopPlayers(response);
            });
        }, 3000)
        return () => clearInterval(interval);
    })

    return (
        <>
            <div className="winner">
                <h1 className="winner-title">Our record holders</h1>
                <div className="winner-categories">
                    <div className="third-place">
                        <h1 className="third-place-title">3 Place</h1>
                        <h2 className="third-place-player-name">{topPlayers?.thirdPlacePlayer}</h2>
                        <h2 className="third-place-number-of-wins">{topPlayers?.thirdPlaceNumberOfWins}</h2>
                    </div>
                    <div className="first-place">
                        <h1 className="first-place-title">1 Place</h1>
                        <h2 className="first-place-player-name">{topPlayers?.firstPlacePlayer}</h2>
                        <h2 className="first-place-number-of-wins">{topPlayers?.firstPlaceNumberOfWins}</h2>
                    </div>
                    <div className="second-place">
                        <h1 className="second-place-title">2 Place</h1>
                        <h2 className="second-place-player-name">{topPlayers?.secondPlacePlayer}</h2>
                        <h2 className="second-place-number-of-wins">{topPlayers?.secondPlaceNumberOfWins}</h2>
                    </div>
                </div>
            </div>
        </>
    )
})