import {cleanup, render, waitFor} from '@testing-library/react';
import { act } from 'react-dom/test-utils';
import { BrowserRouter } from 'react-router-dom';
import HomePage from '../app/features/home/HomePage';

beforeEach(() => {
    act(() => {
        render(<BrowserRouter><HomePage/></BrowserRouter>);
    });
});

afterAll(() => {
   cleanup(); 
});

describe("HomePage component tests", () => {

    describe("Renders correctly initial document", () => {

        test("The document should display container", async () => {
            await waitFor(() => {
                const container = document.querySelector('#homePage');
                expect(container).toBeDefined();
            });
        });

        test("Container should have common Header", async () => {
            await waitFor(() => {
                const commonHeader = document.querySelector('#common-header');
                expect(commonHeader).toBeDefined();
                expect(commonHeader?.textContent).toBe("Welcome to my sea battle game!");
            })
        });

        test("Container should have notIsLoggedIn header if user don't login", async () => {
            await waitFor(() => {
                const notIsLoggedInHeader = document.querySelector('#notIsLoggedId-header');
                expect(notIsLoggedInHeader).toBeDefined();
                expect(notIsLoggedInHeader?.textContent).toContain("If you don't have an account, please");
            });
        });
    });
});