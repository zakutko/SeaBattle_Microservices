import {cleanup, render, waitFor} from '@testing-library/react';
import { act } from 'react-dom/test-utils';
import { BrowserRouter } from 'react-router-dom';
import RegisterForm from '../app/features/users/RegisterForm';

beforeEach(() => {
    act(() => {
        render(<BrowserRouter><RegisterForm/></BrowserRouter>)
    });
});

afterAll(() => {
   cleanup(); 
});

describe("RegisterForm component tests", () => {

    describe("Renders correctly initial document", () => {

        test("The document displays Form", async () => {
            await waitFor(() => {
                const form = document.querySelector('#register-form');
                expect(form).toBeDefined();
            });
        });

        test("Form should displays 4 <input> tags", async () => {
            await waitFor(() => {
                const form = document.querySelector('#register-form');
                if(form !== null){
                    const inputs = form.querySelectorAll('input');
                    expect(inputs).toHaveLength(4);
                }
            });
        });

        test("All inputs should have placeholders", async () => {
            await waitFor(() => {
                const form = document.querySelector('#register-form');
                if(form !== null){
                    const inputs = form.querySelectorAll('input');
                    inputs.forEach(element => expect(element.placeholder).not.toBeNull());
                }
            });
        });

        test("Form should have ErrorMessage aria", async () => {
            await waitFor(() => {
                const form = document.querySelector('#register-form');
                if(form !== null){
                    const errorMessage = form.querySelector("ErrorMessage");
                    expect(errorMessage).toBeDefined();
                }
            });
        });

        test("Form should have button \'Register\'", async () => {
            await waitFor(() => {
                const button = document.querySelector('#register-button');
                expect(button?.textContent).toBe("Register");
            });
        });
    }); 
});