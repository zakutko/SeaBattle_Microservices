import {cleanup, render, waitFor} from '@testing-library/react';
import { act } from 'react-dom/test-utils';
import { BrowserRouter } from 'react-router-dom';
import LoginForm from '../app/features/users/LoginForm';

beforeEach(() => {
    act(() => {
        render(<BrowserRouter><LoginForm/></BrowserRouter>);
    });
})

afterAll(() => {
   cleanup(); 
});

describe("LoginForm component tests", () => {
    
    describe("Renders correctly initial document", () => {

        test("The document displays Formik", async () => {
            await waitFor(() => {
                const form = document.querySelector('#login-form');
                expect(form).toBeDefined();
            });
        });

        test("Form should displays 2 <input> tags", async () => {
            await waitFor(() => {
                const form = document.querySelector('#login-form');
                if(form !== null){
                    const inputs = form.querySelectorAll("input");
                    expect(inputs).toHaveLength(2);
                }
            });
        });

        test("Form should have ErrorMessage component", async () => {
            await waitFor(() => {
                const form = document.querySelector('#login-form');
                if(form !== null){
                    const errorMessageComponents = form.querySelector("ErrorMessage");
                    expect(errorMessageComponents).toBeDefined();
                }
            });
        });

        test("Form inputs should have placeholder", async () => {
            await waitFor(() => {
                const form = document.querySelector('#login-form');
                if(form !== null){
                    const inputs = form.querySelectorAll("input");
                    inputs.forEach(element => {
                        expect(element.placeholder).not.toBeNull();
                    });
                }
            });
        });

        test("Form button should be type=submit", async () => {
            await waitFor(() => {
                const form = document.querySelector('#login-form');
                if(form !== null){
                    const button = form.querySelector("button");
                    expect(button?.type).toBe("submit");
                }
            });
        });
    });
});