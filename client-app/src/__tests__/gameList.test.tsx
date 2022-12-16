import {cleanup, render, waitFor} from '@testing-library/react';
import { act } from 'react-dom/test-utils';
import { BrowserRouter } from 'react-router-dom';
import GameList from '../app/features/game/GameList';

beforeEach(() => {
    act(() => {
        render(<BrowserRouter><GameList/></BrowserRouter>);
    });
});

afterAll(() => {
    cleanup();
});

describe("GameList component tests", () => {

    describe("Renders correctly initial document", () => {

        test("The document should displays gamaList component", async () => {
            await waitFor(() => {
                const gameListContainer = document.querySelector('#gameList');
                expect(gameListContainer).toBeDefined();
            });
        });

        test("The container should have create game btn", async () => {
            await waitFor(() => {
                const createGameBtn = document.querySelector('#createGame-btn');
                expect(createGameBtn).toBeDefined();
                expect(createGameBtn?.className).toBe("ui purple button createGameBtn");
                expect(createGameBtn?.textContent).toBe("Create Game");
            });
        });

        test("The gameList container should have cards container", async () => {
            await waitFor(() => {
                const cardsContainer = document.querySelector('#cards');
                expect(cardsContainer).toBeDefined();
            });
        });
    });
});