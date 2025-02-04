using System.Collections.Generic;

namespace GraphColoring
{
    // Classe gérant les connexions entre les cellules d'une grille de Sudoku à l'aide d'un graphe.
    public class SudokuConnections
    {
        // Graphe représentant les connexions entre les cellules du Sudoku.
        public Graph Graph { get; private set; }
        // Nombre de lignes dans la grille de Sudoku.
        private readonly int Rows = 9;
        // Nombre de colonnes dans la grille de Sudoku.
        private readonly int Cols = 9;
        // Nombre total de blocs dans la grille.
        private readonly int TotalBlocks;
        // Ensemble des identifiants de tous les noeuds dans le graphe.
        public IEnumerable<int> AllIds { get; private set; }

        // Constructeur qui initialise le graphe et crée les connexions entre les cellules.
        public SudokuConnections()
        {
            Graph = new Graph();
            TotalBlocks = Rows * Cols;
            GenerateGraph();
            ConnectEdges();
            AllIds = Graph.GetAllNodesIds();
        }

        // Génère les noeuds du graphe pour chaque cellule du Sudoku.
        private void GenerateGraph()
        {
            for (int idx = 1; idx <= TotalBlocks; idx++)
            {
                Graph.AddNode(idx);
            }
        }

        // Connecte les noeuds du graphe selon les règles du Sudoku.
        private void ConnectEdges()
        {
            var matrix = GetGridMatrix();
            var headConnections = new Dictionary<int, Dictionary<string, List<int>>>();

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    int head = matrix[row][col];
                    var connections = WhatToConnect(matrix, row, col);
                    headConnections[head] = connections;
                }
            }

            ConnectThose(headConnections);
        }

        // Effectue les connexions entre les noeuds en fonction des relations définies.
        private void ConnectThose(Dictionary<int, Dictionary<string, List<int>>> headConnections)
        {
            foreach (var head in headConnections.Keys)
            {
                var connections = headConnections[head];
                foreach (var key in connections.Keys)
                {
                    foreach (var v in connections[key])
                    {
                        Graph.AddEdge(head, v);
                    }
                }
            }
        }

        // Détermine quels noeuds doivent être connectés à partir d'une position donnée dans la grille.
        private Dictionary<string, List<int>> WhatToConnect(int[][] matrix, int rows, int cols)
        {
            var connections = new Dictionary<string, List<int>>
            {
                ["rows"] = new List<int>(), // Connexions dans la même ligne.
                ["cols"] = new List<int>(), // Connexions dans la même colonne.
                ["blocks"] = new List<int>() // Connexions dans le même bloc de 3x3.
            };

            // Ajoute les connexions pour les lignes, colonnes et blocs en évitant les doublons.
            // ROWS
            for (int c = cols + 1; c < 9; c++)
            {
                connections["rows"].Add(matrix[rows][c]);
            }

            // COLS
            for (int r = rows + 1; r < 9; r++)
            {
                connections["cols"].Add(matrix[r][cols]);
            }

            // BLOCKS: déterminer les cellules à connecter dans le bloc 3x3 en fonction de la position actuelle.
            // Calcul basé sur les indices modulaires pour identifier les autres cellules du même bloc.
            int startRow = rows - rows % 3;
            int startCol = cols - cols % 3;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int adjRow = startRow + i;
                    int adjCol = startCol + j;
                    if (adjRow != rows || adjCol != cols)  // Éviter de se connecter à soi-même.
                    {
                        connections["blocks"].Add(matrix[adjRow][adjCol]);
                    }
                }
            }

            return connections;
        }

        // Crée une matrice représentant les indices des cellules de la grille de Sudoku.
        private int[][] GetGridMatrix()
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
                    matrix[rows][cols] = count++; // Attribue un numéro unique à chaque cellule, de 1 à 81.
                }
            }
            return matrix;
        }
    }
}
