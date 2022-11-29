import { Box, IconButton, Paper, Table, TableBody, TableCell, TableContainer, TableFooter, TableHead, TablePagination, TableRow, useTheme } from "@mui/material"
import { observer } from "mobx-react"
import { useEffect, useState } from "react"
import agent from "../../api/agent"
import NavBar from "../../layout/NavBar"
import { GameHistoryList } from "../../models/gameHistoryList"
import Winners from "../winners/Winners"
import "./game.css";
import FirstPageIcon from '@mui/icons-material/FirstPage';
import KeyboardArrowLeft from '@mui/icons-material/KeyboardArrowLeft';
import KeyboardArrowRight from '@mui/icons-material/KeyboardArrowRight';
import LastPageIcon from '@mui/icons-material/LastPage';

interface TablePaginationActionsProps
{
    count: number;
    page: number;
    rowsPerPage: number;
    onPageChange: (
        event: React.MouseEvent<HTMLButtonElement>,
        newPage: number,
    ) => void;
}

function TablePaginationActions(props: TablePaginationActionsProps)
{
    const theme = useTheme();
    const { count, page, rowsPerPage, onPageChange } = props;

    const handleFirstPageButtonClick = (
        event: React.MouseEvent<HTMLButtonElement>,
    ) =>
    {
        onPageChange(event, 0);
    };

    const handleBackButtonClick = (event: React.MouseEvent<HTMLButtonElement>) =>
    {
        onPageChange(event, page - 1);
    };

    const handleNextButtonClick = (event: React.MouseEvent<HTMLButtonElement>) =>
    {
        onPageChange(event, page + 1);
    };

    const handleLastPageButtonClick = (event: React.MouseEvent<HTMLButtonElement>) =>
    {
        onPageChange(event, Math.max(0, Math.ceil(count / rowsPerPage) - 1));
    };

    return (
        <Box sx={{ flexShrink: 0, ml: 2.5 }}>
            <IconButton
                onClick={handleFirstPageButtonClick}
                disabled={page === 0}
                aria-label="first page"
            >
                {theme.direction === 'rtl' ? <LastPageIcon /> : <FirstPageIcon />}
            </IconButton>
            <IconButton
                onClick={handleBackButtonClick}
                disabled={page === 0}
                aria-label="previous page"
            >
                {theme.direction === 'rtl' ? <KeyboardArrowRight /> : <KeyboardArrowLeft />}
            </IconButton>
            <IconButton
                onClick={handleNextButtonClick}
                disabled={page >= Math.ceil(count / rowsPerPage) - 1}
                aria-label="next page"
            >
                {theme.direction === 'rtl' ? <KeyboardArrowLeft /> : <KeyboardArrowRight />}
            </IconButton>
            <IconButton
                onClick={handleLastPageButtonClick}
                disabled={page >= Math.ceil(count / rowsPerPage) - 1}
                aria-label="last page"
            >
                {theme.direction === 'rtl' ? <FirstPageIcon /> : <LastPageIcon />}
            </IconButton>
        </Box>
    );
}

export default observer(function GameHistoryList()
{
    const [gameHistoryList, setGameHistoryList] = useState<GameHistoryList[]>([])

    useEffect(() =>
    {
        agent.GameHistories.gameHistories().then(response =>
        {
            setGameHistoryList(response);
        });
        const interval = setInterval(() =>
        {
            agent.GameHistories.gameHistories().then(response =>
            {
                setGameHistoryList(response);
            });
        }, 3000)
        return () => clearInterval(interval);
    }, [])

    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(5);

    const handleChangePage = (
        event: React.MouseEvent<HTMLButtonElement> | null,
        newPage: number,
    ) =>
    {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (
        event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
    ) =>
    {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
    };

    return (
        <>
            <NavBar />
            <Winners />

            <div className="gameHistoryTable">
                <TableContainer component={Paper}>
                    <Table sx={{ minWidth: 650 }} aria-label="simple table">
                        <TableHead>
                            <TableRow>
                                <TableCell><b>Game Id:</b></TableCell>
                                <TableCell align="right"><b>First player:</b></TableCell>
                                <TableCell align="right"><b>Second player:</b></TableCell>
                                <TableCell align="right"><b>Game state:</b></TableCell>
                                <TableCell align="right"><b>Winner:</b></TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {(rowsPerPage > 0
                                ? gameHistoryList.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                                : gameHistoryList
                            ).map((gameHistory) => (
                                <TableRow
                                    key={gameHistory.id}
                                    sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                >
                                    <TableCell component="th" scope="row">
                                        {gameHistory.gameId}
                                    </TableCell>
                                    <TableCell align="right" component="th" scope="row">
                                        {gameHistory.firstPlayerName}
                                    </TableCell>
                                    <TableCell align="right" component="th" scope="row">
                                        {gameHistory.secondPlayerName}
                                    </TableCell>
                                    <TableCell align="right" component="th" scope="row">
                                        {gameHistory.gameStateName}
                                    </TableCell>
                                    <TableCell align="right" component="th" scope="row">
                                        {gameHistory.winnerName}
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                        <TableFooter>
                            <TableRow>
                                <TablePagination
                                    rowsPerPageOptions={[5, 10, 15, { label: 'All', value: -1 }]}
                                    colSpan={3}
                                    count={gameHistoryList.length}
                                    rowsPerPage={rowsPerPage}
                                    page={page}
                                    SelectProps={{
                                        inputProps: {
                                            'aria-label': 'rows per page',
                                        },
                                        native: true,
                                    }}
                                    onPageChange={handleChangePage}
                                    onRowsPerPageChange={handleChangeRowsPerPage}
                                    ActionsComponent={TablePaginationActions}
                                />
                            </TableRow>
                        </TableFooter>
                    </Table>
                </TableContainer>
            </div>
        </>
    )
})