import {cleanup, fireEvent, queryByAttribute, render, waitFor, screen} from '@testing-library/react';
import { act } from 'react-dom/test-utils';
import { BrowserRouter } from 'react-router-dom';
import LoginForm from '../app/features/users/LoginForm';

afterAll(() => {
   cleanup(); 
});

const getById = queryByAttribute.bind(null, 'id');

describe("LoginForm component tests", () => {
    
    describe("Renders correctly initial document", () => {

        test("The document displays Formik", () => {
            const dom = render(<BrowserRouter><LoginForm/></BrowserRouter>);
            const form = getById(dom.container, 'login-form');
            expect(form).toBeDefined();
        });

        test("Form should displays 2 <input> tags", () => {
            const dom = render(<BrowserRouter><LoginForm/></BrowserRouter>);
            const form =  getById(dom.container, 'login-form');
            if(form !== null){
                const inputs = form.querySelectorAll("input");
                expect(inputs).toHaveLength(2);
            }
        });

        test("Form should have ErrorMessage component", () => {
            const dom = render(<BrowserRouter><LoginForm/></BrowserRouter>);
            const form = getById(dom.container, 'login-form');
            if(form !== null){
                const errorMessageComponents = form.querySelector("ErrorMessage");
                expect(errorMessageComponents).toBeDefined();
            }
        });

        test("Form inputs should have placeholder", () => {
            const dom = render(<BrowserRouter><LoginForm/></BrowserRouter>);
            const form = getById(dom.container, 'login-form');
            if(form !== null){
                const inputs = form.querySelectorAll("input");
                inputs.forEach(element => {
                    expect(element.placeholder).not.toBeNull();
                });
            }
        });

        test("Form button should be type=submit", () => {
            const dom = render(<BrowserRouter><LoginForm/></BrowserRouter>);
            const form = getById(dom.container, 'login-form');
            if(form !== null){
                const button = form.querySelector("button");
                expect(button?.type).toBe("submit");
            }
        });

        //TODO:
        test("After click on button navigate to \'main page\'", () => {
            const dom = render(<BrowserRouter><LoginForm/></BrowserRouter>);
            const button = getById(dom.container, 'button');
            if(button !== null){
                act(() => {
                    fireEvent.click(button);
                })
            }
        });
    });
});