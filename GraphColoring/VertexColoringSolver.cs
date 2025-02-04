using System;
using System.Collections.Generic;
using System.Linq;
using Sudoku.Shared;

namespace GraphColoring
{
    // Implémentation d'un solveur de Sudoku utilisant l'algorithme de saturation de degré (DSatur) pour la coloration de graphes.
    public class DSaturSolver : ISudokuSolver
    {
        // Connexions de Sudoku utilisées pour gérer les relations entre les cellules.
        private SudokuConnections sudokuGraph;
        // Matrice pour associer les positions de la grille de Sudoku aux nœuds du graphe.
        private int[][] mappedGrid;
        // Limite maximale de tentatives pour prévenir les boucles infinies lors de la résolution.
        private const int MaxAttempts = 1000000;

        // Constructeur qui initialise les connexions de Sudoku et prépare la matrice de mappage.
        public DSaturSolver()
        {
            sudokuGraph = new SudokuConnections();
            mappedGrid = GetMappedMatrix();
        }

        // Méthode principale pour résoudre le Sudoku. Utilise l'approche de coloration de graphe DSatur.
        public SudokuGrid Solve(SudokuGrid s)
        {
            var board = new int[9, 9];  // Tableau pour stocker les valeurs actuelles du Sudoku.
            // Copie les valeurs du Sudoku dans un tableau local pour le traitement.
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    board[i, j] = s.Cells[i, j];

            // Appel de la méthode de résolution utilisant DSatur combiné avec backtracking.
            if (SolveDSaturWithBacktracking(board))
            {
                // Si le Sudoku est résolu, recopier les valeurs dans la grille initiale.
                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                        s.Cells[i, j] = board[i, j];
            }

            return s;  // Retourne la grille de Sudoku, modifiée ou non selon que la solution a été trouvée.
        }

        // Méthode récursive pour résoudre le Sudoku en utilisant DSatur et backtracking.
        private bool SolveDSaturWithBacktracking(int[,] board)
        {
            var (row, col) = FindEmptyCell(board);  // Trouve une cellule vide avec le plus haut degré de saturation.
            if (row == -1 && col == -1) return true;  // Si aucune cellule vide n'est trouvée, le Sudoku est résolu.

            // Obtient et teste toutes les valeurs possibles pour la cellule trouvée.
            var possibleValues = GetPossibleValues(board, row, col);
            foreach (var value in possibleValues)
            {
                // Vérifie si la valeur peut être placée sans conflit.
                if (IsSafe(board, row, col, value))
                {
                    board[row, col] = value;  // Place la valeur.

                    // Continue de résoudre le reste de la grille récursivement.
                    if (SolveDSaturWithBacktracking(board))
                        return true;
                    
                    board[row, col] = 0;  // Efface la valeur si elle ne mène pas à une solution (backtracking).
                }
            }
            return false;  // Retourne faux si aucune solution n'est trouvée pour cette branche.
        }

        // Trouve une cellule vide avec le plus haut degré de saturation (nombre de valeurs différentes déjà fixées à proximité).
        private (int row, int col) FindEmptyCell(int[,] board)
        {
            int maxSaturation = -1;
            int selectedRow = -1;
            int selectedCol = -1;

            // Parcourt toutes les cellules de la grille pour trouver la cellule vide la plus contrainte.
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col] == 0) // Uniquement les cellules vides.
                    {
                        int saturation = CalculateSaturation(board, row, col);  // Calcule la saturation de la cellule.
                        if (saturation > maxSaturation)
                        {
                            maxSaturation = saturation;
                            selectedRow = row;
                            selectedCol = col;
                        }
                    }
                }
            }
            return (selectedRow, selectedCol);  // Retourne la position de la cellule la plus contrainte.
        }

        // Calcule le degré de saturation d'une cellule, c'est-à-dire le nombre de valeurs différentes dans son voisinage immédiat.
        private int CalculateSaturation(int[,] board, int row, int col)
        {
            var usedValues = new HashSet<int>();  // Ensemble pour stocker les valeurs déjà utilisées autour de la cellule.

            // Examine la ligne, la colonne, et le bloc 3x3 pour compter les valeurs uniques.
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

            return usedValues.Count;  // Retourne le nombre de valeurs différentes autour de la cellule.
        }

        // Renvoie une liste des valeurs qui peuvent légalement être placées dans la cellule spécifiée.
        private List<int> GetPossibleValues(int[,] board, int row, int col)
        {
            var used = new bool[10];  // Tableau pour marquer les valeurs déjà présentes et donc interdites.

            // Marque les valeurs présentes dans la ligne, la colonne et le bloc.
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

            var possibleValues = new List<int>();  // Liste des valeurs possibles pour cette cellule.
            for (int i = 1; i <= 9; i++)
                if (!used[i])
                    possibleValues.Add(i);  // Ajoute la valeur si elle n'est pas déjà utilisée.

            return possibleValues;  // Retourne la liste complète des valeurs possibles.
        }

        // Vérifie si placer un certain numéro dans une cellule donnée ne cause pas de conflit selon les règles du Sudoku.
        private bool IsSafe(int[,] board, int row, int col, int num)
        {
            // Vérifie la présence du numéro dans la ligne, la colonne et le bloc.
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

            return true;  // Retourne vrai si aucun conflit n'est détecté.
        }

        // Crée une matrice pour mapper chaque cellule de la grille de Sudoku à un index unique utilisé dans le graph.
        private int[][] GetMappedMatrix()
        {
            var matrix = new int[9][];  // Initialisation de la matrice.
            for (int i = 0; i < 9; i++)
            {
                matrix[i] = new int[9];
            }

            int count = 1;  // Compteur pour attribuer un index unique à chaque cellule.
            for (int rows = 0; rows < 9; rows++)
                for (int cols = 0; cols < 9; cols++)
                {
                    matrix[rows][cols] = count++;  // Assignation de l'index.
                }
            return matrix;  // Retourne la matrice mappée.
        }
    }
}
