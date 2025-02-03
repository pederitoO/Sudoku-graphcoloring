using System.Collections.Generic;

namespace GraphColoring
{
    public class Node
    {
        public int Id { get; private set; }
        public int Data { get; private set; }
        private Dictionary<int, int> ConnectedTo;

        public Node(int idx, int data = 0)
        {
            Id = idx;
            Data = data;
            ConnectedTo = new Dictionary<int, int>();
        }

        public void AddNeighbour(Node neighbour, int weight = 0)
        {
            if (!ConnectedTo.ContainsKey(neighbour.Id))
            {
                ConnectedTo[neighbour.Id] = weight;
            }
        }

        public void SetData(int data)
        {
            Data = data;
        }

        public IEnumerable<int> GetConnections()
        {
            return ConnectedTo.Keys;
        }

        public int GetWeight(Node neighbour)
        {
            return ConnectedTo[neighbour.Id];
        }
    }

    public class Graph
    {
        public int TotalV { get; private set; }
        private Dictionary<int, Node> AllNodes;

        public Graph()
        {
            TotalV = 0;
            AllNodes = new Dictionary<int, Node>();
        }

        public Node? AddNode(int idx)
        {
            if (AllNodes.ContainsKey(idx))
                return null;

            TotalV++;
            var node = new Node(idx: idx);
            AllNodes[idx] = node;
            return node;
        }

        public void AddNodeData(int idx, int data)
        {
            if (AllNodes.ContainsKey(idx))
            {
                AllNodes[idx].SetData(data);
            }
        }

        public void AddEdge(int src, int dst, int wt = 0)
        {
            AllNodes[src].AddNeighbour(AllNodes[dst], wt);
            AllNodes[dst].AddNeighbour(AllNodes[src], wt);
        }

        public bool IsNeighbour(int u, int v)
        {
            if (u >= 1 && u <= 81 && v >= 1 && v <= 81 && u != v)
            {
                return AllNodes[u].GetConnections().Contains(v);
            }
            return false;
        }

        public Node? GetNode(int idx)
        {
            return AllNodes.ContainsKey(idx) ? AllNodes[idx] : null;
        }

        public IEnumerable<int> GetAllNodesIds()
        {
            return AllNodes.Keys;
        }
    }
}