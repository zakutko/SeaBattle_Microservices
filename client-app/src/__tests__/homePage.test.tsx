import {cleanup, fireEvent, queryByAttribute, render} from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import HomePage from '../app/features/home/HomePage';

afterAll(() => {
   cleanup(); 
});

const getById = queryByAttribute.bind(null, 'id');

describe("HomePage component tests", () => {

    describe("Renders correctly initial document", () => {

        test("The document should display container", () => {
            const dom = render(<BrowserRouter><HomePage/></BrowserRouter>);
            const container = getById(dom.container, 'homePage');
            expect(container).toBeDefined();
        });

        test("Container should have common Header", () => {
            const dom = render(<BrowserRouter><HomePage/></BrowserRouter>);
            const commonHeader = getById(dom.container, 'common-header');
            expect(commonHeader).toBeDefined();
            expect(commonHeader?.textContent).toBe("Welcome to my sea battle game!");
        });

        //TODO:
        /*
        test("Container should display isLoggedIn header if user login", () => {
            const dom = render(<BrowserRouter><HomePage/></BrowserRouter>);
            const isLoggedInHeader = getById(dom.container, 'isLoggedIn-header');
            expect(isLoggedInHeader).toBeDefined();
            expect(isLoggedInHeader?.textContent).toBe("Serhii Zakutko");
        });
        */

        test("Container should have notIsLoggedIn header if user don't login", () => {
            const dom = render(<BrowserRouter><HomePage/></BrowserRouter>);
            const notIsLoggedInHeader = getById(dom.container, 'notIsLoggedId-header');
            expect(notIsLoggedInHeader).toBeDefined();
            expect(notIsLoggedInHeader?.textContent).toContain("If you don't have an account, please");
        });
    });
})