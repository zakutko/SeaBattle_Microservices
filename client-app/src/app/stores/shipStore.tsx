import { makeAutoObservable } from "mobx";
import agent from "../api/agent";
import { Ship, ShipFormValues } from "../models/ship";

export default class ShipStore {
    ship: Ship | null = null;

    constructor(){
        makeAutoObservable(this)
    }

    createShipOnField = async (creds: ShipFormValues) => {
        try {
            await agent.Games.createShipOnField(creds);
        }
        catch(error){
            throw error;
        }
    }
}