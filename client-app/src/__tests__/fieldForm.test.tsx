import {cleanup, render, waitFor} from '@testing-library/react';
import { act } from 'react-dom/test-utils';
import { BrowserRouter } from 'react-router-dom';
import FieldForm from '../app/features/field/FieldForm';

beforeEach(() => {
    act(() => {
        render(<BrowserRouter><FieldForm/></BrowserRouter>);
    });
});

afterAll(() => {
    cleanup();
});

describe("FieldForm component tests", () => {

    describe("Renders correctly initial document", () => {

        test("The document should displays fieldForm container with form", async () => {
            await waitFor(() => {
                const fieldFormContainer = document.querySelector('#fieldForm');
                expect(fieldFormContainer).toBeDefined();
                expect(fieldFormContainer?.querySelector('.form')).toBeDefined();
            });
        });

        test("The form should 3 cases to choise input data", async () => {
            await waitFor(() => {
                const fieldFormContainer = document.querySelector('#fieldForm');
                const choiseInputs = fieldFormContainer?.querySelectorAll('h2');
                expect(choiseInputs).toHaveLength(3);
                choiseInputs?.forEach(element => {
                    expect(element.textContent).toMatch(/(Ship size:|Ship direction:|Ship Position:)/i);
                });
            });
        });

        test("The shipSizeRadioGroup should have 4 options to choise", async () => {
            await waitFor(() => {
                const fieldFormContainer = document.querySelector('#fieldForm');
                const shipSizeRadioGroup = fieldFormContainer?.querySelector('#shipSizeRadioGroup');
                const shipSizeRadioGroupInputs = shipSizeRadioGroup?.querySelectorAll('.PrivateSwitchBase-input');
                expect(shipSizeRadioGroupInputs).toBeDefined();
                expect(shipSizeRadioGroupInputs).toHaveLength(4);
                shipSizeRadioGroupInputs?.forEach(element => {
                    expect(element.getAttribute('value')).toMatch(/(1|2|3|4)/i);
                    expect(element.getAttribute('type')).toBe('radio');
                });
            });
        });

        test("The shipDirectionRadioGroup should have 2 options to choise", async () => {
            await waitFor(() => {
                const fieldFormContainer = document.querySelector('#fieldForm');
                const shipDirectionRadioGroup = fieldFormContainer?.querySelector('#shipDirectionRadioGroup');
                const shipDirectionRadioGroupInputs = shipDirectionRadioGroup?.querySelectorAll('.PrivateSwitchBase-input');
                expect(shipDirectionRadioGroupInputs).toBeDefined();
                expect(shipDirectionRadioGroupInputs).toHaveLength(2);
                shipDirectionRadioGroupInputs?.forEach(element => {
                    expect(element.getAttribute('value')).toMatch(/(1|2)/i);
                    expect(element.getAttribute('type')).toBe('radio');
                });
            });
        });

        test("The shipPositionGroup should have 2 inputs [x, y]", async () => {
            await waitFor(() => {
                const fieldFormContainer = document.querySelector('#fieldForm');
                const shipPositionGroup = fieldFormContainer?.querySelector('#shipPositionGroup');
                const inputs = shipPositionGroup?.querySelectorAll('input');
                expect(inputs).toHaveLength(2);
                inputs?.forEach(element => {
                    expect(element.placeholder).not.toBeNull();
                    expect(element.className).toBe('form-control');
                });
            });
        });

        test("The form should have errorMessagesGroup with two errorContainers", async () => {
            await waitFor(() => {
                const fieldFormContainer = document.querySelector('#fieldForm');
                const errorMessagesGroup = fieldFormContainer?.querySelector('#errorMessagesGroup');
                const errorContainers = errorMessagesGroup?.querySelectorAll('.error-container');
                expect(errorMessagesGroup).toBeDefined();
                expect(errorContainers).toHaveLength(2);
            });
        });
    });
});