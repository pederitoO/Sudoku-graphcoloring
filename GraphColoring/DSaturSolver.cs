using System.Collections.Generic;

namespace GraphColoring
{
    // Représente un nœud dans un graphe, qui peut être connecté à d'autres nœuds.
    public class Node
    {
        // Identifiant unique du nœud.
        public int Id { get; private set; }
        // Donnée stockée dans le nœud, peut être utilisée pour diverses applications.
        public int Data { get; private set; }
        // Stocke les connexions aux autres nœuds et les poids de ces connexions.
        private Dictionary<int, int> ConnectedTo;

        // Initialise un nouveau nœud avec un identifiant et une donnée facultative.
        public Node(int idx, int data = 0)
        {
            Id = idx;
            Data = data;
            ConnectedTo = new Dictionary<int, int>();
        }

        // Établit une connexion entre ce nœud et un autre nœud avec un poids de connexion spécifique.
        public void AddNeighbour(Node neighbour, int weight = 0)
        {
            if (!ConnectedTo.ContainsKey(neighbour.Id))
            {
                ConnectedTo[neighbour.Id] = weight;
            }
        }

        // Modifie la donnée stockée dans ce nœud.
        public void SetData(int data)
        {
            Data = data;
        }

        // Renvoie les identifiants de tous les nœuds connectés à ce nœud.
        public IEnumerable<int> GetConnections()
        {
            return ConnectedTo.Keys;
        }

        // Retourne le poids de la connexion avec un nœud spécifique.
        public int GetWeight(Node neighbour)
        {
            return ConnectedTo[neighbour.Id];
        }
    }

    // Représente un graphe, une collection de nœuds qui peuvent être interconnectés.
    public class Graph
    {
        // Nombre total de nœuds dans le graphe.
        public int TotalV { get; private set; }
        // Stocke tous les nœuds du graphe, accessible par leur identifiant.
        private Dictionary<int, Node> AllNodes;

        // Crée un nouveau graphe vide.
        public Graph()
        {
            TotalV = 0;
            AllNodes = new Dictionary<int, Node>();
        }

        // Ajoute un nœud au graphe et renvoie le nœud ajouté ou null si un nœud avec cet identifiant existe déjà.
        public Node? AddNode(int idx)
        {
            if (AllNodes.ContainsKey(idx))
                return null;

            TotalV++;
            var node = new Node(idx);
            AllNodes[idx] = node;
            return node;
        }

        // Ajoute ou met à jour la donnée associée à un nœud spécifique.
        public void AddNodeData(int idx, int data)
        {
            if (AllNodes.ContainsKey(idx))
            {
                AllNodes[idx].SetData(data);
            }
        }

        // Crée une connexion bidirectionnelle entre deux nœuds spécifiés avec un poids facultatif.
        public void AddEdge(int src, int dst, int wt = 0)
        {
            AllNodes[src].AddNeighbour(AllNodes[dst], wt);
            AllNodes[dst].AddNeighbour(AllNodes[src], wt);
        }

        // Vérifie si deux nœuds sont directement connectés.
        public bool IsNeighbour(int u, int v)
        {
            if (u >= 1 && u <= 81 && v >= 1 && v <= 81 && u != v)
            {
                return AllNodes[u].GetConnections().Contains(v);
            }
            return false;
        }

        // Récupère un nœud du graphe par son identifiant, renvoie null si aucun nœud avec cet identifiant n'existe.
        public Node? GetNode(int idx)
        {
            return AllNodes.ContainsKey(idx) ? AllNodes[idx] : null;
        }

        // Renvoie les identifiants de tous les nœuds présents dans le graphe.
        public IEnumerable<int> GetAllNodesIds()
        {
            return AllNodes.Keys;
        }
    }
}
