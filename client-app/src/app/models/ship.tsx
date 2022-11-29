export interface Ship {
    shipDirection: number;
    shipState: number;
    shipSize: number;
}

export interface ShipFormValues {
    shipSize: number;
    shipDirection: number;
    x: number;
    y: number;
}