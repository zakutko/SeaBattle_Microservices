import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Ship, ShipFormValues } from "../models/ship";

export default class ShipStore {
    ship: Ship | null = null;

    constructor(){
        makeAutoObservable(this)
    }

    createShipOnField = async (creds: ShipFormValues) => {
        try {
            const ship = await agent.Games.createShipOnField(creds);
            runInAction(() => this.ship = ship);
            console.log(ship);
        }
        catch(error){
            throw error;
        }
    }
}