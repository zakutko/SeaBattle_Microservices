import {cleanup, render, waitFor} from '@testing-library/react';
import { act } from 'react-dom/test-utils';
import { BrowserRouter } from 'react-router-dom';
import PrepareGame from '../app/features/game/PrepareGame';

beforeEach(() => {
    act(() => {
        render(<BrowserRouter><PrepareGame/></BrowserRouter>);
    });
});

afterAll(() => {
    cleanup();
});

describe("PrepareGame component tests", () => {

    describe("Renders correctly initial document", () => {
        
        test("The document should displays prepareGame container", async () => {
            await waitFor(() => {
                const container = document.querySelector('#prepareGame');
                expect(container).toBeDefined();
            });
        });

        test("PrepareGame container should have \'I\'m ready\' btn", async () => {
            await waitFor(() => {
                const button = document.querySelector('#prepareGame-btn');
                expect(button).toBeDefined();
                expect(button?.className).toBe('prepareGameButton');
                expect(button?.textContent).toBe('I\'m ready');
            });
        });

        test("PrepareGame container should have prepareGameCellList container", async () => {
            await waitFor(() => {
                const prepareGameCellList = document.querySelector('#prepareGameCellList');
                expect(prepareGameCellList).toBeDefined();
            });
        });

        test("PrepareGame container should have delete btn", async () => {
            await waitFor(() => {
                const deleteBtn = document.querySelector('#deleteGame-btn');
                expect(deleteBtn).toBeDefined();
                expect(deleteBtn?.textContent).toBe('Delete game');
            });
        });
    });
});