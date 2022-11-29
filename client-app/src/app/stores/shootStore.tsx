import { makeAutoObservable } from "mobx";
import agent from "../api/agent";
import { ShootFormValues } from "../models/shoot";

export default class ShootStore {
    constructor(){
        makeAutoObservable(this)
    }

    fire = async (creds: ShootFormValues) => {
        try {
            await agent.Games.fire(creds);
        }
        catch(error){
            throw error;
        }
    }
}