import { createContext, useContext } from "react";
import ShipStore from "./shipStore";
import ShootStore from "./shootStore";
import UserStore from "./userStore";
import CommonStore from "./сommonStore";

interface Store {
    commonStore: CommonStore;
    userStore: UserStore;
    shipStore: ShipStore;
    shootStore: ShootStore;
}

export const store: Store = {
    commonStore: new CommonStore(),
    userStore: new UserStore(),
    shipStore: new ShipStore(),
    shootStore: new ShootStore()
}

export const StoreContext = createContext(store);

export function useStore(){
    return useContext(StoreContext);    
}