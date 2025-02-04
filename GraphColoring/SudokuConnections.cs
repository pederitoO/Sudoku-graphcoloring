using System.Collections.Generic;

namespace GraphColoring
{
    // Gère les connexions entre les cellules du Sudoku en utilisant un graphe.
    public class SudokuConnections
    {
        // Le graphe qui représente les cellules du Sudoku et leurs connexions.
        public Graph Graph { get; private set; }
        // Nombre de lignes dans la grille de Sudoku, fixé à 9.
        private readonly int Rows = 9;
        // Nombre de colonnes dans la grille de Sudoku, également fixé à 9.
        private readonly int Cols = 9;
        // Nombre total de blocs ou de cellules dans le Sudoku.
        private readonly int TotalBlocks;
        // Collection contenant tous les identifiants des nœuds dans le graphe.
        public IEnumerable<int> AllIds { get; private set; }

        // Constructeur qui initialise le graphe et configure les connexions.
        public SudokuConnections()
        {
            Graph = new Graph();
            TotalBlocks = Rows * Cols;
            GenerateGraph();
            ConnectEdges();
            AllIds = Graph.GetAllNodesIds();
        }

        // Crée un nœud dans le graphe pour chaque cellule du Sudoku.
        private void GenerateGraph()
        {
            for (int idx = 1; idx <= TotalBlocks; idx++)
            {
                Graph.AddNode(idx);
            }
        }

        // Connecte les nœuds du graphe en fonction de leur proximité dans la grille de Sudoku.
        private void ConnectEdges()
        {
            var matrix = GetGridMatrix();
            var headConnections = new Dictionary<int, Dictionary<string, List<int>>>();

            // Détermine les connexions nécessaires pour chaque cellule.
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    int head = matrix[row][col];
                    var connections = WhatToConnect(matrix, row, col);
                    headConnections[head] = connections;
                }
            }

            // Établit les connexions dans le graphe basé sur les connexions déterminées.
            ConnectThose(headConnections);
        }

        // Connecte les nœuds du graphe en utilisant les connexions spécifiées.
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

        // Identifie les connexions nécessaires pour une cellule spécifique.
        private Dictionary<string, List<int>> WhatToConnect(int[][] matrix, int rows, int cols)
        {
            var connections = new Dictionary<string, List<int>>
            {
                ["rows"] = new List<int>(), // Connexions dans la même ligne.
                ["cols"] = new List<int>(), // Connexions dans la même colonne.
                ["blocks"] = new List<int>() // Connexions dans le même bloc 3x3.
            };

            // Connecte les cellules de la même ligne.
            for (int c = cols + 1; c < 9; c++)
            {
                connections["rows"].Add(matrix[rows][c]);
            }

            // Connecte les cellules de la même colonne.
            for (int r = rows + 1; r < 9; r++)
            {
                connections["cols"].Add(matrix[r][cols]);
            }

            // Connecte les cellules du même bloc 3x3.
            int startRow = rows / 3 * 3;
            int startCol = cols / 3 * 3;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int adjRow = startRow + i;
                    int adjCol = startCol + j;
                    if (adjRow != rows || adjCol != cols)  // Évite de se connecter à soi-même.
                    {
                        connections["blocks"].Add(matrix[adjRow][adjCol]);
                    }
                }
            }

            return connections;
        }

        // Génère une matrice représentant la disposition des cellules dans la grille du Sudoku.
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
                    matrix[rows][cols] = count++;  // Incrémente pour chaque cellule, de 1 à 81.
                }
            }
            return matrix;
        }
    }
}
