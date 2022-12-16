import {cleanup, render, waitFor} from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import EndOfTheGame from '../app/features/game/EndOfTheGame';
import 'jest-canvas-mock';
import { act } from 'react-dom/test-utils';

beforeEach(() => {
    act(() => {
        render(<BrowserRouter><EndOfTheGame winnerUserName="Test"/></BrowserRouter>);
    })
});

afterAll(() => {
    cleanup(); 
});

describe("EndOfThGame component tests", () => {

    describe("Renders correctly initial document", () => {

        test("Teh document should displays endOfTheGame container", async () => {
            await waitFor(() => {
                const endOfTheGame = document.querySelector('#endOfTheGame');
                expect(endOfTheGame).toBeDefined();
            });
        });
        
        test("EndOfTheGame component should displays Confetti component", async () => {
            await waitFor(() => {
                const confetti = document.querySelector('#confetti');
                expect(confetti).toBeDefined();
            });
        });

        test("EndOfTheGame component should have header with text content \'Congratulations!\'", async () => {
            await waitFor(() => {
                const endOfTheGame = document.querySelector('#endOfTheGame');
                const header = endOfTheGame?.querySelector('h1')
                expect(header?.textContent).toContain("Congratulations!");
            });
        });

        test("EndOfTheGame component should have button with text content \'Back to home page\'", async () => {
            await waitFor(() => {
                const button = document.querySelector('#endOfTheGame-btn');
                expect(button).toBeDefined();
                expect(button?.textContent).toContain("Back to home page");
            });
        });
    });
});