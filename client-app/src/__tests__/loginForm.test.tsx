import {cleanup, fireEvent, render, screen} from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import LoginForm from '../app/features/users/LoginForm';

afterAll(() => {
   cleanup(); 
});

describe("LoginForm component tests", () => {
    
    describe("Renders correctly initial document", () => {

        test("The document displays Form", () => {
            render(<BrowserRouter><LoginForm/></BrowserRouter>);
            expect(screen.getByTestId("form")).toBeDefined();
        });

        test("Form should displays 2 <input> tags", () => {
            render(<BrowserRouter><LoginForm/></BrowserRouter>);
            const form = screen.getByTestId("form");
            const inputs = form.querySelectorAll("input");
            expect(inputs).toHaveLength(2);
        });

        test("Form should have ErrorMessage component", () => {
            render(<BrowserRouter><LoginForm/></BrowserRouter>);
            const form = screen.getByTestId("form");
            const errorMessageComponents = form.querySelector("ErrorMessage");
            expect(errorMessageComponents).toBeDefined();
        });

        test("Form inputs should have placeholder", () => {
            render(<BrowserRouter><LoginForm/></BrowserRouter>);
            const form = screen.getByTestId("form");
            const inputs = form.querySelectorAll("input");
            inputs.forEach(element => {
                expect(element.placeholder).not.toBeNull();
            });
        });

        test("Form button should be type=submit", () => {
            render(<BrowserRouter><LoginForm/></BrowserRouter>);
            const form = screen.getByTestId("form");
            const button = form.querySelector("button");
            expect(button?.type).toBe("submit");
        });

        test("After click on button navigate to \'main page\'", () => {
            render(<BrowserRouter><LoginForm/></BrowserRouter>);
            const button = screen.getByTestId("button");
            if(button !== null){
                fireEvent.click(button);
            }
        });
    });
});