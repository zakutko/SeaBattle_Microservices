import {cleanup, render, waitFor} from '@testing-library/react';
import { act } from 'react-dom/test-utils';
import { BrowserRouter } from 'react-router-dom';
import GameFieldForm from '../app/features/game/GameFieldForm';

beforeEach(() => {
    act(() => {
        render(<BrowserRouter><GameFieldForm/></BrowserRouter>);
    });
});

afterAll(() => {
    cleanup();
});

describe("GameFieldForm component tests", () => {

    describe("Renders correctly initial document", () => {

        test("The document should displays gameFieldForm container", async () => {
            await waitFor(() => {
                const gameFieldForm = document.querySelector('#gameFieldForm');
                expect(gameFieldForm).toBeDefined();
            });
        });

        test("The gameFieldForm container should have ui form", async () => {
            await waitFor(() => {
                const gameFieldForm = document.querySelector('#gameFieldForm');
                const form = gameFieldForm?.querySelector('form');
                expect(form).toBeDefined();
                expect(form?.className).toContain('form');
            });
        });

        test("The ui form should have 2 error-containers", async () => {
            await waitFor(() => {
                const gameFieldForm = document.querySelector('#gameFieldForm');
                const form = gameFieldForm?.querySelector('form');
                const firstContainerWithClassFields = form?.querySelector('.fields');
                const errorContainers = firstContainerWithClassFields?.querySelectorAll('.error-container');
                expect(errorContainers).toHaveLength(2);
            });
        });

        test("The ui form should have 2 inputs", async () => {
            await waitFor(() => {
                const gameFieldForm = document.querySelector('#gameFieldForm');
                const form = gameFieldForm?.querySelector('form');
                const inputs = form?.querySelectorAll('input');
                expect(inputs).toHaveLength(2);
                inputs?.forEach(element => {
                    expect(element.placeholder).not.toBeNull();
                    expect(element.className).toBe('form-control');
                });
            });
        });

        test("The ui form should have form-btn", async () => {
            await waitFor(() => {
                const gameFieldForm = document.querySelector('#gameFieldForm');
                const form = gameFieldForm?.querySelector('form');
                const button = form?.querySelector('#form-btn');
                expect(button).toBeDefined();
                expect(button?.className).toContain('ui positive button');
                expect(button?.textContent).toBe('Shoot');
            });
        });
    });
});