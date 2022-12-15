import {cleanup, fireEvent, queryByAttribute, render} from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import NavBar from '../app/layout/NavBar';

afterAll(() => {
    cleanup(); 
});
 
const getById = queryByAttribute.bind(null, 'id');

describe("NavBar component tests", () => {

    describe("Renders correctly initial document", () => {

        test("The document displays navbar menu", () => {
            const dom = render(<BrowserRouter><NavBar/></BrowserRouter>);
            const menuNavBar = getById(dom.container, 'navbar');
            expect(menuNavBar).toBeDefined();
        });

        test("The navbar menu should have logo", () => {
            const dom = render(<BrowserRouter><NavBar/></BrowserRouter>);
            const logoImg = getById(dom.container, 'logo-img');
            if(logoImg !== null){
                expect(logoImg).toBeDefined();
            }
        });

        test("The navbar menu should have 2 buttons with \'navbar-btn\' className", () => {
            const dom = render(<BrowserRouter><NavBar/></BrowserRouter>);
            const buttons = dom.container.getElementsByClassName('navbar-btn');
            expect(buttons).toHaveLength(2);
        });

        test("The navbar menu should have button with Text Content \'Games\'", () => {
            const dom = render(<BrowserRouter><NavBar/></BrowserRouter>);
            const gamesBtn = getById(dom.container, 'games-btn');
            expect(gamesBtn?.textContent).toBe("Games");
        });

        test("The navbar menu should have button with Text Content \'History\'", () => {
            const dom = render(<BrowserRouter><NavBar/></BrowserRouter>);
            const historyBtn = getById(dom.container, 'history-btn');
            expect(historyBtn?.textContent).toBe("History");
        });

        test("The navbar should have Logut button", () => {
            const dom = render(<BrowserRouter><NavBar/></BrowserRouter>);
            const logoutBtn = getById(dom.container, 'logout-btn');
            expect(logoutBtn).toBeDefined();
            expect(logoutBtn?.textContent).toBe("Logout");
        });

        //TODO:
        test("After click on button Games should navigate to \'/gameList\'", () => {
            const dom = render(<BrowserRouter><NavBar/></BrowserRouter>);
            const gamesBtn = getById(dom.container, 'games-btn');
            if(gamesBtn !== null){
                fireEvent.click(gamesBtn);
            }
        });

        test("After click on button History should navigate to \'/gameHistoryList\'", () => {
            const dom = render(<BrowserRouter><NavBar/></BrowserRouter>);
            const historyBtn = getById(dom.container, 'history-btn');
            if(historyBtn !== null){
                fireEvent.click(historyBtn);
            }
        });

        test("After click on button Logout should navigate to HomePage", () => {
            const dom = render(<BrowserRouter><NavBar/></BrowserRouter>);
            const logoutBtn = getById(dom.container, 'logout-btn');
            if(logoutBtn !== null){
                fireEvent.click(logoutBtn);
            }
        });
    });
});