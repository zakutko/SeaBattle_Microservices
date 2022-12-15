import { observer } from "mobx-react";
import { Link } from "react-router-dom";
import { Button, Container, Header } from "semantic-ui-react";
import NavBar from "../../layout/NavBar";
import { useStore } from "../../stores/store";
import "../home/homePage.css";

export default observer(function HomePage(){
    const store = useStore();
    
    return (
        <Container id="homePage" textAlign="center" className="home" data-testid="homePage">
            <Header id="common-header" as={'h1'} size='huge' inverted color='purple'>
                Welcome to my sea battle game!
            </Header>
            {store.userStore.isLoggedIn ? (
                <>
                    <NavBar />
                    <Header id="isLoggedIn-header" as={'h2'} inverted color="purple">
                        Serhii Zakutko
                    </Header>
                </>
            ) : (
                <>
                <Button as={Link} to='/login' size='medium' color="blue">
                    Login
                </Button>
                <Header id="notIsLoggedId-header" className="home-register-text" as={'h4'}>If you don't have an account, please <Link to={'/register'}>register*</Link></Header>
                </>
            )}
        </Container>
    )
})