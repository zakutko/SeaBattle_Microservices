import { observer } from "mobx-react";
import { Link, useNavigate } from "react-router-dom";
import { Button, Container, Menu } from "semantic-ui-react";
import { useStore } from "../stores/store";
import "./style.css";

export default observer(function NavBar() {
    const {userStore: {logout}} = useStore();
    const navigate = useNavigate();

    return (
        <Menu inverted fixed='top' tabular id="navbar">
            <Container>
                <Menu.Item header as={Link} to='/'>
                    <img id="logo-img" src="/assets/logo.png" alt='logo' style={{height: 40, width: 120, paddingRight: 20}}/>
                    MySeaBattle
                </Menu.Item>
                <Menu.Item>
                    <Button id="games-btn" className="navbar-btn" as={Link} to='/gameList'>Games</Button>
                </Menu.Item>
                <Menu.Item>
                    <Button id="history-btn" className="navbar-btn" as={Link} to='/gameHistoryList'>History</Button>
                </Menu.Item>
                <Menu.Item position="right">
                    <Button id="logout-btn" onClick={() => {logout(); navigate("/")}} color='grey'>Logout</Button>
                </Menu.Item>
            </Container>
        </Menu>
    )
})