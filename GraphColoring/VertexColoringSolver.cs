using System;
using System.Collections.Generic;
using System.Linq;
using Sudoku.Shared;

namespace GraphColoring
{
    // Implémentation du solveur de Sudoku utilisant l'algorithme DSatur pour la coloration de graphes.
    public class DSaturSolver : ISudokuSolver
    {
        // Gestion des connexions de Sudoku via un graphe.
        private SudokuConnections sudokuGraph;
        // Matrice pour mapper les positions de la grille de Sudoku aux noeuds du graphe.
        private int[][] mappedGrid;
        // Limite maximale d'essais pour éviter les boucles infinies.
        private const int MaxAttempts = 1000000;

        // Constructeur qui initialise le graphe et la matrice de mappage.
        public DSaturSolver()
        {
            sudokuGraph = new SudokuConnections();
            mappedGrid = GetMappedMatrix();
        }

        // Méthode principale pour résoudre le Sudoku.
        public SudokuGrid Solve(SudokuGrid s)
        {
            var board = new int[9, 9]; // Tableau temporaire pour stocker les valeurs de Sudoku.
            // Copie les valeurs actuelles du Sudoku dans le tableau board.
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    board[i, j] = s.Cells[i, j];

            // Tente de résoudre le Sudoku en utilisant DSatur avec backtracking.
            if (SolveDSaturWithBacktracking(board))
            {
                // Si résolu, recopie les valeurs résolues dans le Sudoku original.
                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                        s.Cells[i, j] = board[i, j];
            }

            return s; // Retourne la grille de Sudoku résolue ou non modifiée.
        }

        // Résout le Sudoku en utilisant un algorithme DSatur avec backtracking.
        private bool SolveDSaturWithBacktracking(int[,] board)
        {
            var (row, col) = FindEmptyCell(board); // Trouve la prochaine cellule vide avec la plus haute saturation.
            if (row == -1 && col == -1) return true; // Si aucune cellule vide, Sudoku est résolu.

            // Obtient les valeurs possibles pour la cellule vide.
            var possibleValues = GetPossibleValues(board, row, col);
            foreach (var value in possibleValues)
            {
                // Vérifie si la valeur peut être placée sans conflit.
                if (IsSafe(board, row, col, value))
                {
                    board[row, col] = value; // Place la valeur.

                    // Continue de résoudre récursivement.
                    if (SolveDSaturWithBacktracking(board))
                        return true;

                    board[row, col] = 0; // Efface la valeur si elle ne mène pas à une solution (backtracking).
                }
            }
            return false; // Retourne faux si aucune valeur n'est valide.
        }

        // Trouve une cellule vide dans la grille avec le plus haut degré de saturation.
        private (int row, int col) FindEmptyCell(int[,] board)
        {
            int maxSaturation = -1;
            int selectedRow = -1;
            int selectedCol = -1;

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col] == 0) // Seulement les cellules vides.
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
            return (selectedRow, selectedCol); // Retourne la position de la cellule avec la plus haute saturation.
        }

        // Calcule le degré de saturation d'une cellule, c'est-à-dire le nombre de valeurs différentes adjacents à cette cellule.
        private int CalculateSaturation(int[,] board, int row, int col)
        {
            var usedValues = new HashSet<int>();

            // Parcourt la ligne, la colonne, et le bloc pour compter les valeurs uniques.
            for (int c = 0; c < 9; c++)
                if (board[row, c] != 0)
                    usedValues.Add(board[row, c]);

            for (int r = 0; r < 9; r++)
                if (board[r, col] != 0)
                    usedValues.Add(board[r, col]);

            int boxRow = row - row % 3;
            int boxCol = col - col % 3;
            for (int r = boxRow; r < boxRow + 3; r++)
                for (int c = boxCol; c < boxCol + 3; c++)
                    if (board[r, c] != 0)
                        usedValues.Add(board[r, c]);

            return usedValues.Count; // Nombre de valeurs uniques.
        }

        // Retourne une liste de valeurs possibles qui peuvent être placées dans une cellule spécifique sans violer les règles du Sudoku.
        private List<int> GetPossibleValues(int[,] board, int row, int col)
        {
            var used = new bool[10]; // Marque les chiffres déjà utilisés dans la ligne, la colonne et le bloc.

            for (int c = 0; c < 9; c++)
                if (board[row, c] != 0)
                    used[board[row, c]] = true;

            for (int r = 0; r < 9; r++)
                if (board[r, col] != 0)
                    used[board[r, col]] = true;

            int boxRow = row - row % 3;
            int boxCol = col - col % 3;
            for (int r = boxRow; r < boxRow + 3; r++)
                for (int c = boxCol; c < boxCol + 3; c++)
                    if (board[r, c] != 0)
                        used[board[r, c]] = true;

            var possibleValues = new List<int>();
            for (int i = 1; i <= 9; i++)
                if (!used[i])
                    possibleValues.Add(i); // Ajoute le chiffre s'il n'est pas déjà utilisé.

            return possibleValues; // Liste des chiffres possibles pour la cellule.
        }

        // Vérifie si placer un chiffre dans une cellule est sûr, c'est-à-dire qu'il ne crée pas de conflits selon les règles du Sudoku.
        private bool IsSafe(int[,] board, int row, int col, int num)
        {
            for (int c = 0; c < 9; c++)
                if (board[row, c] == num)
                    return false;

            for (int r = 0; r < 9; r++)
                if (board[r, col] == num)
                    return false;

            int boxRow = row - row % 3;
            int boxCol = col - col % 3;
            for (int r = boxRow; r < boxRow + 3; r++)
                for (int c = boxCol; c < boxCol + 3; c++)
                    if (board[r, c] == num)
                        return false;

            return true; // Retourne vrai si placer le chiffre ne crée pas de conflit.
        }

        // Génère une matrice pour associer les cellules de la grille de Sudoku aux noeuds du graphe.
        private int[][] GetMappedMatrix()
        {
            var matrix = new int[9][];
            for (int i = 0; i < 9; i++)
            {
                matrix[i] = new int[9];
            }

            int count = 1;
            for (int rows = 0; rows < 9; rows++)
                for (int cols = 0; cols < 9; cols++)
                {
                    matrix[rows][cols] = count++; // Attribue un index unique à chaque cellule.
                }
            return matrix; // Retourne la matrice mappée.
        }
    }
}
