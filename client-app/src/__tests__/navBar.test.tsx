import {cleanup, render, waitFor} from '@testing-library/react';
import { act } from 'react-dom/test-utils';
import { BrowserRouter } from 'react-router-dom';
import NavBar from '../app/layout/NavBar';

beforeEach(() => {
    act(() => {
        render(<BrowserRouter><NavBar/></BrowserRouter>);
    });
});

afterAll(() => {
    cleanup(); 
});

describe("NavBar component tests", () => {

    describe("Renders correctly initial document", () => {

        test("The document displays navbar menu", async () => {
            await waitFor(() => {
                const menuNavBar = document.querySelector('#navbar');
                expect(menuNavBar).toBeDefined();
            });
        });

        test("The navbar menu should have logo", async () => {
            await waitFor(() => {
                const logoImg = document.querySelector('#logo-img');
                if(logoImg !== null){
                    expect(logoImg).toBeDefined();
                }
            });
        });

        test("The navbar menu should have 2 buttons with \'navbar-btn\' className", async () => {
            await waitFor(() => {
                const buttons = document.getElementsByClassName('navbar-btn');
                expect(buttons).toHaveLength(2);
            });
        });

        test("The navbar menu should have button with Text Content \'Games\'", async () => {
            await waitFor(() => {
                const gamesBtn = document.querySelector('#games-btn');
                expect(gamesBtn?.textContent).toBe("Games");
            });
        });

        test("The navbar menu should have button with Text Content \'History\'", async () => {
            await waitFor(() => {
                const historyBtn = document.querySelector('#history-btn');
                expect(historyBtn?.textContent).toBe("History");
            });
        });

        test("The navbar should have Logut button", async () => {
            await waitFor(() => {
                const logoutBtn = document.querySelector('#logout-btn');
                expect(logoutBtn).toBeDefined();
                expect(logoutBtn?.textContent).toBe("Logout");
            });
        });
    });
});