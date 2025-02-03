using Sudoku.Shared;

namespace GraphColoring
{
    public class graphcoloringsolver : ISudokuSolver
    {
        private SudokuConnections sudokuGraph;
        private int[][] mappedGrid;

        public graphcoloringsolver()
        {
            sudokuGraph = new SudokuConnections();
            mappedGrid = GetMappedMatrix();
        }

        public SudokuGrid Solve(SudokuGrid s)
        {
            var board = new int[9, 9];
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    board[i, j] = s.Cells[i, j];

            var (color, given) = GraphColoringInitializeColor(board);
            if (GraphColorUtility(9, color, 1, given))
            {
                int count = 1;
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        s.Cells[row, col] = color[count];
                        count++;
                    }
                }
            }

            return s;
        }

        private (int[] color, HashSet<int> given) GraphColoringInitializeColor(int[,] board)
        {
            var color = new int[sudokuGraph.Graph.TotalV + 1];
            var given = new HashSet<int>();

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col] != 0)
                    {
                        int idx = mappedGrid[row][col];
                        color[idx] = board[row, col];
                        given.Add(idx);
                    }
                }
            }

            return (color, given);
        }

        private bool GraphColorUtility(int m, int[] color, int v, HashSet<int> given)
        {
            if (v == sudokuGraph.Graph.TotalV + 1)
                return true;

            for (int c = 1; c <= m; c++)
            {
                if (IsSafe2Color(v, color, c, given))
                {
                    color[v] = c;
                    if (GraphColorUtility(m, color, v + 1, given))
                        return true;
                    if (!given.Contains(v))
                        color[v] = 0;
                }
            }

            return false;
        }

        private bool IsSafe2Color(int v, int[] color, int c, HashSet<int> given)
        {
            if (given.Contains(v) && color[v] == c)
                return true;
            else if (given.Contains(v))
                return false;

            for (int i = 1; i <= sudokuGraph.Graph.TotalV; i++)
            {
                if (color[i] == c && sudokuGraph.Graph.IsNeighbour(v, i))
                    return false;
            }
            return true;
        }

        private int[][] GetMappedMatrix()
        {
            var matrix = new int[9][];
            for (int i = 0; i < 9; i++)
            {
                matrix[i] = new int[9];
            }

            int count = 1;
            for (int rows = 0; rows < 9; rows++)
            {
                for (int cols = 0; cols < 9; cols++)
                {
                    matrix[rows][cols] = count++;
                }
            }
            return matrix;
        }
    }
}
