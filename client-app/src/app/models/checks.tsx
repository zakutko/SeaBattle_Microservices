export interface IsHit {
    isHit: boolean;
}

export interface IsPlayerReady {
    message: string;
}

export interface IsTwoPlayersReady {
    numberOfReadyPlayers: number;
}

export interface IsEndOfTheGame {
    isEndOfTheGame: boolean;
    winnerUserName: string;
}

export interface IsGameOwner {
    isGameOwner: boolean;
    isSecondPlayerConnected: boolean;
}