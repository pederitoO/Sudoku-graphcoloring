using System;
using System.Collections.Generic;
using System.Linq;
using Sudoku.Shared;

namespace GraphColoring
{
    // La classe DSaturSolver implémente l'interface ISudokuSolver qui définit une méthode Solve, 
    // permettant d'intégrer cette classe dans un cadre plus large de résolution de Sudoku.
    
    public class DSaturSolver : ISudokuSolver
    {
        private SudokuConnections sudokuGraph;
        private int[][] mappedGrid;
        private const int MaxAttempts = 1000000; // Limite pour éviter les boucles infinies

        //Le constructeur initialise sudokuGraph et mappedGrid
        
        public DSaturSolver()
        {
            sudokuGraph = new SudokuConnections();
            mappedGrid = GetMappedMatrix();
        }

        // Cette méthode copie la grille Sudoku donnée dans un tableau local board pour le traitement. 
        // Elle utilise ensuite une méthode de résolution récursive (SolveDSaturWithBacktracking) pour essayer de résoudre la grille
        
        public SudokuGrid Solve(SudokuGrid s)
        {
            var board = new int[9, 9];
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    board[i, j] = s.Cells[i, j];

            if (SolveDSaturWithBacktracking(board))
            {
                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                        s.Cells[i, j] = board[i, j];
            }

            return s;
        }

        // Cette fonction est le cœur du solveur DSatur.
        // Elle trouve une cellule vide avec la plus grande saturation, puis essaie récursivement de placer une valeur 
        // valide dans cette cellule jusqu'à ce que la grille soit complète ou qu'aucune solution ne soit trouvée.

csharp
Copy

        private bool SolveDSaturWithBacktracking(int[,] board)
        {
            var (row, col) = FindEmptyCell(board);
            if (row == -1 && col == -1) return true;

            var possibleValues = GetPossibleValues(board, row, col);
            foreach (var value in possibleValues)
            {
                if (IsSafe(board, row, col, value))
                {
                    board[row, col] = value;
                    
                    if (SolveDSaturWithBacktracking(board))
                        return true;
                    
                    board[row, col] = 0;
                }
            }
            return false;
        }


        // Les méthodes FindEmptyCell, CalculateSaturation, GetPossibleValues, et IsSafe fournissent la logique nécessaire
        // pour identifier la cellule à essayer ensuite, calculer la saturation (nombre de valeurs différentes
        // dans les cellules adjacentes), déterminer les valeurs possibles pour une cellule, 
        // et vérifier si placer une valeur dans une cellule est sûr (ne viole aucune contrainte du Sudoku).
        
        private (int row, int col) FindEmptyCell(int[,] board)
        {
            int maxSaturation = -1;
            int selectedRow = -1;
            int selectedCol = -1;

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col] == 0)
                    {
                        int saturation = CalculateSaturation(board, row, col);
                        if (saturation > maxSaturation)
                        {
                            maxSaturation = saturation;
                            selectedRow = row;
                            selectedCol = col;
                        }
                    }
                }
            }
            return (selectedRow, selectedCol);
        }

        private int CalculateSaturation(int[,] board, int row, int col)
        {
            var usedValues = new HashSet<int>();
            
            // Check row
            for (int c = 0; c < 9; c++)
                if (board[row, c] != 0)
                    usedValues.Add(board[row, c]);

            // Check column
            for (int r = 0; r < 9; r++)
                if (board[r, col] != 0)
                    usedValues.Add(board[r, col]);

            // Check 3x3 box
            int boxRow = row - row % 3;
            int boxCol = col - col % 3;
            for (int r = boxRow; r < boxRow + 3; r++)
                for (int c = boxCol; c < boxCol + 3; c++)
                    if (board[r, c] != 0)
                        usedValues.Add(board[r, c]);

            return usedValues.Count;
        }

        private List<int> GetPossibleValues(int[,] board, int row, int col)
        {
            var used = new bool[10];
            
            // Check row
            for (int c = 0; c < 9; c++)
                if (board[row, c] != 0)
                    used[board[row, c]] = true;

            // Check column
            for (int r = 0; r < 9; r++)
                if (board[r, col] != 0)
                    used[board[r, col]] = true;

            // Check 3x3 box
            int boxRow = row - row % 3;
            int boxCol = col - col % 3;
            for (int r = boxRow; r < boxRow + 3; r++)
                for (int c = boxCol; c < boxCol + 3; c++)
                    if (board[r, c] != 0)
                        used[board[r, c]] = true;

            var possibleValues = new List<int>();
            for (int i = 1; i <= 9; i++)
                if (!used[i])
                    possibleValues.Add(i);

            return possibleValues;
        }

        private bool IsSafe(int[,] board, int row, int col, int num)
        {
            // Check row
            for (int c = 0; c < 9; c++)
                if (board[row, c] == num)
                    return false;

            // Check column
            for (int r = 0; r < 9; r++)
                if (board[r, col] == num)
                    return false;

            // Check 3x3 box
            int boxRow = row - row % 3;
            int boxCol = col - col % 3;
            for (int r = boxRow; r < boxRow + 3; r++)
                for (int c = boxCol; c < boxCol + 3; c++)
                    if (board[r, c] == num)
                        return false;

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
