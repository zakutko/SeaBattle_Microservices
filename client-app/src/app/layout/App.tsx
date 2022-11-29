import { observer } from 'mobx-react'
import { useEffect } from 'react';
import { Route, Routes } from 'react-router-dom';
import Game from '../features/game/Game';
import GameHistoryList from '../features/game/GameHistoryList';
import GameList from '../features/game/GameList';
import PrepareGame from '../features/game/PrepareGame';
import HomePage from '../features/home/HomePage';
import LoginForm from '../features/users/LoginFrom';
import RegisterForm from '../features/users/RegisterForm';
import { useStore } from '../stores/store';
import "./style.css";

function App() {
  const {commonStore, userStore} = useStore();

  useEffect(() => {
    if (commonStore.token) {
      userStore.getUser().finally(() => commonStore.setAppLoaded());
    } else {
      commonStore.setAppLoaded();
    }
  }, [commonStore, userStore]);

  if(!commonStore.appLoaded) return (<div>App Loading...</div>)

  return (
    <div>
      <Routes>
        <Route index element = {<HomePage />} />
        <Route path='/login' element = {<LoginForm />} />
        <Route path='/register' element = {<RegisterForm />}/>
        <Route path='/gameList' element = {<GameList />}/>
        <Route path='/prepareGame' element = {<PrepareGame />}/>
        <Route path='/game' element = {<Game />}/>
        <Route path='/gameHistoryList' element ={<GameHistoryList />} />
      </Routes> 
    </div>  
  );
}
export default observer(App);