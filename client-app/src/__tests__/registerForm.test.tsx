import {cleanup, fireEvent, queryByAttribute, render} from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import RegisterForm from '../app/features/users/RegisterForm';

afterAll(() => {
   cleanup(); 
});

const getById = queryByAttribute.bind(null, 'id');

describe("RegisterForm component tests", () => {

    describe("Renders correctly initial document", () => {

        test("The document displays Form", () => {
            const dom = render(<BrowserRouter><RegisterForm/></BrowserRouter>);
            const form = getById(dom.container, 'register-form');
            expect(form).toBeDefined();
        });

        test("Form should displays 4 <input> tags", () => {
            const dom = render(<BrowserRouter><RegisterForm/></BrowserRouter>);
            const form = getById(dom.container, 'register-form');
            if(form !== null){
                const inputs = form.querySelectorAll('input');
                expect(inputs).toHaveLength(4);
            };
        });

        test("All inputs should have placeholders", () => {
            const dom = render(<BrowserRouter><RegisterForm/></BrowserRouter>);
            const form = getById(dom.container, 'register-form');
            if(form !== null){
                const inputs = form.querySelectorAll('input');
                inputs.forEach(element => expect(element.placeholder).not.toBeNull());
            }
        });

        test("Form should have ErrorMessage aria", () => {
            const dom = render(<BrowserRouter><RegisterForm/></BrowserRouter>);
            const form = getById(dom.container, 'register-form');
            if(form !== null){
                const errorMessage = form.querySelector("ErrorMessage");
                expect(errorMessage).toBeDefined();
            }
        });

        test("Form should have button \'Register\'", () => {
            const dom = render(<BrowserRouter><RegisterForm/></BrowserRouter>);
            const button = getById(dom.container, 'register-button');
            expect(button?.textContent).toBe("Register");
        });

        test("After click on button navigate to \'main page\'", () => {
            const dom = render(<BrowserRouter><RegisterForm/></BrowserRouter>);
            const button = getById(dom.container, 'register-button');
            if(button !== null){
                fireEvent.click(button);
            }
        });
    }); 
});