using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DSaturProject
{
    class DSatur
    {
        String fileName = "";

        public DSatur(String fileName)
        {
            this.fileName = fileName;
        }

        #region METODE

        public int maxDegree(int[,] arr)
        {
            int max = arr[0, 0];
            int indexMax = 0;
            for (int i = 1; i < arr.GetLength(1); i++)
            {
                if (arr[0, i] > max)
                {
                    max = arr[0, i];
                    indexMax = i;
                }
            }
            return indexMax;
        }

        public bool uncolored(int[,] arr)
        {
            for (int i = 0; i < arr.GetLength(1); i++)
            {
                if (arr[1, i] == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public HashSet<int> setUncolAdj(int[,] arr)
        {
            HashSet<int> set = new HashSet<int>();
            int max = arr[2, 0];
            for (int i = 0; i < arr.GetLength(1); i++)
            {
                if (arr[2, i] > max)
                {
                    max = arr[2, i];
                }
            }
            for (int i = 0; i < arr.GetLength(1); i++)
            {
                if (arr[2, i] == max)
                {
                    set.Add(i); //u skup dodajemo index
                }
            }
            return set;
        }

        public KeyValuePair<int,KeyValuePair<int[,],LinkedList<int>[]>> main()
        {
            int[,] dataMatrix = null;
            LinkedList<int>[] adjacentVertices = null;
            FileStream fs = new FileStream("../../graph/" + this.fileName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            String line;

            HashSet<int> colorSet = new HashSet<int>();
            colorSet.Add(1); //dodajemo prvu boju
            colorSet.Add(2); //dodajemo drugu boju
            int color = 2;

            #region UCITAVANjE GRAFA
            int v = -1; // broj cvorova
            while ((line = sr.ReadLine()) != null)
            {
                String[] lineStrings = line.Split(' ');
                if (lineStrings[0] == "p")
                {
                    v = Int32.Parse(lineStrings[2]);
                    dataMatrix = new int[3, v];
                    adjacentVertices = new LinkedList<int>[v];
                    for (int i = 0; i < v; i++)
                    {
                        adjacentVertices[i] = new LinkedList<int>();
                    }
                }
                else
                {
                    if (lineStrings[0] == "e")
                    {
                        int vertex1 = Int32.Parse(lineStrings[1]);
                        int vertex2 = Int32.Parse(lineStrings[2]);
                        adjacentVertices[vertex1].AddLast(vertex2);
                        adjacentVertices[vertex2].AddLast(vertex1);
                    }
                }
            }
            sr.Close();
            fs.Close();
            #endregion

            for (int i = 0; i < v; i++)
            {
                dataMatrix[0, i] = adjacentVertices[i].Count;
            }

            int maxDeg = maxDegree(dataMatrix); //index cvora sa maksimalnim stepenom
            dataMatrix[1, maxDeg] = 1;

            bool uncol = uncolored(dataMatrix); //ako je true znaci da ima neobojanih..u suprotnom je false
            while (uncol)
            {
                for (int i = 0; i < dataMatrix.GetLength(1); i++)
                {
                    if (dataMatrix[1, i] == 0)
                    {
                        HashSet<int> temp = new HashSet<int>();
                        foreach (int w in adjacentVertices[i])
                        {
                            temp.Add(dataMatrix[1, w]);
                        }
                        dataMatrix[2, i] = temp.Count;
                    }
                }
                HashSet<int> setuncoladj = setUncolAdj(dataMatrix);
                int index = -1;
                if (setuncoladj.Count == 1)
                {
                    index = setuncoladj.First();
                }
                else
                {
                    index = setuncoladj.First();
                    int currMax = dataMatrix[0, index];
                    foreach (int id in setuncoladj)
                    {
                        int tempMax = dataMatrix[0, id];
                        if (tempMax > currMax)
                        {
                            index = id;
                        }
                    }
                }
                int control = -1;
                foreach (int col in colorSet)
                {
                    int count = 0;
                    foreach (int adj in adjacentVertices[index])
                    {
                        if (dataMatrix[1, adj] == col)
                        {
                            break;
                        }
                        else
                        {
                            count++;
                        }
                    }
                    if (count == adjacentVertices[index].Count)
                    {
                        dataMatrix[1, index] = col;
                        control = 1;
                        break;
                    }
                }
                if (control == -1)
                {
                    color += 1;
                    colorSet.Add(color);
                    dataMatrix[1, index] = color;
                }
                for (int i = 0; i < dataMatrix.GetLength(1); i++)
                {
                    dataMatrix[2, i] = 0;
                }
                uncol = uncolored(dataMatrix);
            }
            KeyValuePair<int[,], LinkedList<int>[]> tempData = new KeyValuePair<int[,], LinkedList<int>[]>(dataMatrix, adjacentVertices);
            KeyValuePair<int, KeyValuePair<int[,], LinkedList<int>[]>> data = new KeyValuePair<int, KeyValuePair<int[,], LinkedList<int>[]>>(colorSet.Count, tempData);
            return data;
        }

        #endregion

    }
}