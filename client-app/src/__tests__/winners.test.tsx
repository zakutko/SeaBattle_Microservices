import Winners from '../app/features/winners/Winners';
import {cleanup, render, queryByAttribute } from '@testing-library/react';

afterAll(() => {
    cleanup();
});

const getById = queryByAttribute.bind(null, 'id');

describe("Winners component tests", () => {

    describe("Renders correctly initial document", () => {

        test('The document displays 4 <h1> tags', () => {
            const dom = render(<Winners/>);
            const winners = getById(dom.container, 'winners');
            if (winners !== null){
                const inputs = winners.querySelectorAll("h1");
                expect(inputs).toHaveLength(4);
            }
        });

        test("The document displays 6 <h2> tags", () => {
            const dom = render(<Winners/>);
            const winners = getById(dom.container, 'winners');
            if (winners !== null){
                const inputs = winners.querySelectorAll("h2");
                expect(inputs).toHaveLength(6);
            }
        });

        test("The document displays <h1> tag with text content \'Our record holders\'", () => {
            const dom = render(<Winners/>);
            const winners = getById(dom.container, 'winners');
            if (winners !== null){
                const isHeader = winners.querySelector("h1")?.textContent?.includes("Our record holders");
                expect(isHeader).toEqual(true);
            }
        })

        test("The document has <div> tag with className \'winner\'", () => {
            const dom = render(<Winners/>);
            const winners = getById(dom.container, 'winners');
            if (winners !== null){
                const isClassName = winners.querySelector("div")?.className?.includes("winner");
                expect(isClassName).toEqual(true);
            }
        })
    })
});