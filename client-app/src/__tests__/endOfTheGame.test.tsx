import {cleanup, fireEvent, queryByAttribute, render} from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import EndOfTheGame from '../app/features/game/EndOfTheGame';
import 'jest-canvas-mock';
import { act } from 'react-dom/test-utils';

afterAll(() => {
    cleanup(); 
});

const getById = queryByAttribute.bind(null, 'id');

describe("EndOfThGame component tests", () => {

    describe("Renders correctly initial document", () => {

        test("Teh document should displays endOfTheGame container", () => {
            const dom = render(<BrowserRouter><EndOfTheGame winnerUserName="Test"/></BrowserRouter>);
            const endOfTheGame = getById(dom.container, 'endOfTheGame');
            expect(endOfTheGame).toBeDefined();
        });
        
        test("EndOfTheGame component should displays Confetti component", () => {
            const dom = render(<BrowserRouter><EndOfTheGame winnerUserName="Test"/></BrowserRouter>);
            const confetti = getById(dom.container, 'confetti');
            expect(confetti).toBeDefined();
        });

        test("EndOfTheGame component should have header with text content \'Congratulations!\'", () => {
            const dom = render(<BrowserRouter><EndOfTheGame winnerUserName="Test"/></BrowserRouter>);
            const endOfTheGame = getById(dom.container, 'endOfTheGame');
            const header = endOfTheGame?.querySelector('h1')
            expect(header?.textContent).toContain("Congratulations!");
        });

        test("EndOfTheGame component should have button with text content \'Back to home page\'", () => {
            const dom = render(<BrowserRouter><EndOfTheGame winnerUserName="Test"/></BrowserRouter>);
            const button = getById(dom.container, 'endOfTheGame-btn');
            expect(button).toBeDefined();
            expect(button?.textContent).toContain("Back to home page");
        });

        //TODO:
        test("After click on button navigate to \'main page\'", () => {
            const dom = render(<BrowserRouter><EndOfTheGame winnerUserName="Test"/></BrowserRouter>);
            const button = getById(dom.container, 'endOfTheGame-btn');
            if(button !== null){
                act(() => {
                    fireEvent.click(button);
                });
            }
        });
    });
});