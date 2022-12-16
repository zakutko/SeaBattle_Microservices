import Winners from '../app/features/winners/Winners';
import {cleanup, render, waitFor } from '@testing-library/react';
import { act } from 'react-dom/test-utils';

beforeEach(() => {
    act(() => {
        render(<Winners/>)
    });
});

afterAll(() => {
    cleanup();
});

describe("Winners component tests", () => {

    describe("Renders correctly initial document", () => {

        test('The document displays 4 <h1> tags', async () => {
            await waitFor(() => {
                const winners = document.querySelector('#winners');
                if (winners !== null){
                    const inputs = winners.querySelectorAll("h1");
                    expect(inputs).toHaveLength(4);
                }
            });
        });

        test("The document displays 6 <h2> tags", async () => {
            await waitFor(() => {
                const winners = document.querySelector('#winners');
                if (winners !== null){
                    const inputs = winners.querySelectorAll("h2");
                    expect(inputs).toHaveLength(6);
                }
            });
        });

        test("The document displays <h1> tag with text content \'Our record holders\'", async () => {
            await waitFor(() => {
                const winners = document.querySelector('#winners');
                if (winners !== null){
                    const isHeader = winners.querySelector("h1")?.textContent?.includes("Our record holders");
                    expect(isHeader).toEqual(true);
                }
            });
        });

        test("The document has <div> tag with className \'winner\'", async () => {
            await waitFor(() => {
                const winners = document.querySelector('#winners');
                if (winners !== null){
                    const isClassName = winners.querySelector("div")?.className?.includes("winner");
                    expect(isClassName).toEqual(true);
                }
            });
        });
    });
});