import {cleanup, render, waitFor} from '@testing-library/react';
import { act } from 'react-dom/test-utils';
import { BrowserRouter } from 'react-router-dom';
import Game from '../app/features/game/Game';

beforeEach(() => {
    act(() => {
        render(<BrowserRouter><Game/></BrowserRouter>);
    });
});

afterAll(() => {
    cleanup();
});

describe("Game component tests", () => {

    describe("Renders correctly initial document", () => {

        test("The document should displays Game component", async () => {
            await waitFor(() => {
                const gameComponent = document.querySelector('#game');
                expect(gameComponent).toBeDefined();
            });
        });

        test("The Game container should 3 containers with block-color-* class", async () => {
            await waitFor(() => {
                const gameComponent = document.querySelector('#game');
                const containersWithBlockClass = gameComponent?.querySelectorAll('.block');
                expect(containersWithBlockClass).toHaveLength(3);
            });
        });

        test("The game container should have 2 fields", async () => {
            await waitFor(() => {
                const gameComponent = document.querySelector('#game');
                const fields = gameComponent?.querySelectorAll('.field');
                expect(fields).toHaveLength(2);
            });
        });

        test("The Game container should have gameFieldForm", async () => {
            await waitFor(() => {
                const gameComponent = document.querySelector('#game');
                const gameFieldForm = gameComponent?.querySelector('.gameFieldForm');
                expect(gameFieldForm).toBeDefined();
            });
        });
    });
});