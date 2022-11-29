import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { User, UserFormValues } from "../models/user";
import { store } from "./store";

export default class UserStore
{
    user: User | null = null;

    constructor()
    {
        makeAutoObservable(this)
    }

    get isLoggedIn()
    {
        return !!this.user;
    }

    login = async (creds: UserFormValues) =>
    {
        try
        {
            const user = await agent.Account.login(creds);
            store.commonStore.setToken(user.token);
            runInAction(() => this.user = user);
            console.log(user);
        }
        catch (error)
        {
            throw error;
        }
    }

    logout = () =>
    {
        store.commonStore.setToken(null);
        window.localStorage.removeItem('token');
        this.user = null;
    }

    getUser = async () =>
    {
        try
        {
            const token = localStorage.getItem('token');
            if (token)
            {
                const user = await agent.Account.current(token);
                runInAction(() => this.user = user);
            }
        }
        catch (error)
        {
            console.log(error);
        }
    }

    register = async (creds: UserFormValues) =>
    {
        try
        {
            const user = await agent.Account.register(creds);
            store.commonStore.setToken(user.token);
            runInAction(() => this.user = user);
            console.log(user);
        }
        catch (error)
        {
            throw error;
        }
    }
}