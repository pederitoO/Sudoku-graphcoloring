using System;
using System.Collections.Generic;
using System.Linq;
using Sudoku.Shared;

namespace GraphColoring
{
    // Implémentation d'un solveur de Sudoku utilisant un algorithme de coloration de sommets.
    public class VertexColoringSolver : ISudokuSolver
    {
        // Graphe des connexions du Sudoku, utilisé pour déterminer les voisins de chaque cellule.
        private SudokuConnections sudokuGraph;
        // Matrice pour mapper les positions de la grille de Sudoku aux indices des sommets du graphe.
        private int[][] mappedGrid;
        // Dictionnaire pour stocker la couleur assignée à chaque sommet.
        private Dictionary<int, int?> vertexColors;
        // Limite maximale d'essais pour prévenir les boucles infinies.
        private const int MaxAttempts = 1000000;

        // Constructeur qui initialise les structures nécessaires au solveur.
        public VertexColoringSolver()
        {
            sudokuGraph = new SudokuConnections();
            mappedGrid = GetMappedMatrix();
            vertexColors = new Dictionary<int, int?>();
        }

        // Résout la grille de Sudoku donnée.
        public SudokuGrid Solve(SudokuGrid s)
        {
            InitializeColors(s);
            if (SolveWithBacktracking())
            {
                TransferColorsToGrid(s);
            }
            return s;
        }

        // Tente de résoudre le Sudoku par backtracking.
        private bool SolveWithBacktracking()
        {
            int uncoloredVertex = FindMostConstrainedVertex();
            if (uncoloredVertex == -1)
                return true; // Si tous les sommets sont colorés, la solution est trouvée.

            var availableColors = GetSortedAvailableColors(uncoloredVertex);
            foreach (var color in availableColors)
            {
                if (IsSafeToColor(uncoloredVertex, color))
                {
                    vertexColors[uncoloredVertex] = color;
                    if (SolveWithBacktracking())
                        return true;
                    vertexColors[uncoloredVertex] = null; // Retire la couleur si elle mène à une impasse.
                }
            }
            return false;
        }

        // Trouve le sommet le plus contraint (celui avec le moins d'options de couleur).
        private int FindMostConstrainedVertex()
        {
            int maxConstraints = -1;
            int selectedVertex = -1;

            for (int vertex = 1; vertex <= 81; vertex++)
            {
                if (!vertexColors[vertex].HasValue)
                {
                    int constraints = CountConstraints(vertex);
                    if (constraints > maxConstraints)
                    {
                        maxConstraints = constraints;
                        selectedVertex = vertex;
                    }
                }
            }
            return selectedVertex;
        }

        // Compte le nombre de contraintes pour un sommet donné.
        private int CountConstraints(int vertex)
        {
            int count = 0;
            var usedColors = new HashSet<int>();

            for (int adj = 1; adj <= 81; adj++)
            {
                if (sudokuGraph.Graph.IsNeighbour(vertex, adj) && vertexColors[adj].HasValue)
                {
                    count++;
                    usedColors.Add(vertexColors[adj].Value);
                }
            }
            return count * 10 + usedColors.Count; // Poids les contraintes pour prioriser les sommets les plus contraints.
        }

        // Obtient les couleurs disponibles pour un sommet, triées par fréquence d'utilisation pour minimiser les conflits.
        private List<int> GetSortedAvailableColors(int vertex)
        {
            var colorFrequency = new Dictionary<int, int>();
            for (int color = 1; color <= 9; color++)
            {
                colorFrequency[color] = 0;
            }

            for (int adj = 1; adj <= 81; adj++)
            {
                if (sudokuGraph.Graph.IsNeighbour(vertex, adj) && vertexColors[adj].HasValue)
                {
                    var adjColor = vertexColors[adj].Value;
                    colorFrequency[adjColor]++;
                }
            }

            return Enumerable.Range(1, 9)
                .OrderBy(c => colorFrequency[c])
                .ToList(); // Trie les couleurs par ordre croissant de conflit potentiel.
        }

        // Vérifie si colorer un sommet avec une couleur donnée ne crée pas de conflit.
        private bool IsSafeToColor(int vertex, int color)
        {
            for (int adj = 1; adj <= 81; adj++)
            {
                if (sudokuGraph.Graph.IsNeighbour(vertex, adj) && 
                    vertexColors[adj].HasValue && 
                    vertexColors[adj].Value == color)
                {
                    return false; // Retourne faux si un voisin a déjà cette couleur.
                }
            }
            return true;
        }

        // Initialise les couleurs des sommets basées sur les valeurs initiales de la grille de Sudoku.
        private void InitializeColors(SudokuGrid s)
        {
            vertexColors.Clear();
            
            // Initialise tous les sommets comme non colorés.
            for (int i = 1; i <= 81; i++)
            {
                vertexColors[i] = null;
            }

            // Définit les couleurs initiales à partir de la grille de Sudoku.
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (s.Cells[row, col] != 0)
                    {
                        int vertexId = mappedGrid[row][col];
                        vertexColors[vertexId] = s.Cells[row, col];
                    }
                }
            }
        }

        // Transfère les couleurs des sommets résolus vers la grille de Sudoku.
        private void TransferColorsToGrid(SudokuGrid s)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    int vertexId = mappedGrid[row][col];
                    if (vertexColors[vertexId].HasValue)
                    {
                        s.Cells[row, col] = vertexColors[vertexId].Value;
                    }
                }
            }
        }

        // Retourne true si le coloriage actuel est valide, c'est-à-dire si toutes les couleurs sont assignées correctement sans conflit.
        private bool IsValidColoring()
        {
            // Vérifie si tous les sommets sont colorés correctement.
            if (vertexColors.Values.Any(c => !c.HasValue))
                return false;

            // Vérifie si des sommets voisins partagent la même couleur, ce qui serait incorrect.
            for (int v = 1; v <= 81; v++)
            {
                for (int u = v + 1; u <= 81; u++)
                {
                    if (sudokuGraph.Graph.IsNeighbour(v, u) && 
                        vertexColors[v].Value == vertexColors[u].Value)
                    {
                        return false;
                    }
                }
            }

            return true; // Retourne vrai si toutes les conditions sont respectées.
        }

        // Génère une matrice pour mapper chaque cellule de la grille de Sudoku à un indice unique utilisé dans le graphe.
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
                    matrix[rows][cols] = count++; // Attribue un index unique à chaque cellule.
                }
            }
            return matrix; // Retourne la matrice mappée.
        }
    }
}
