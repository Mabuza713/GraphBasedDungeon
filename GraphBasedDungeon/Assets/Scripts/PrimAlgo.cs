using GraphDungeon;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;


namespace GraphDungeon
{
    public class PrimAlgo : MonoBehaviour
    {
        public List<Edge> finalEdgeList = new List<Edge>();
        public List<Edge> currentPossibleEdges = new List<Edge>();
        public List<Node> activatedNodes = new List<Node>();
        public void PrimAlgorithm()
        {
            Node node = transform.GetComponent<PlaceRandomRooms>().listOfNodes[0];
            activatedNodes.Add(node);
            while (finalEdgeList.Count != transform.GetComponent<PlaceRandomRooms>().listOfNodes.Count - 1)
            {
                CurrentPossibleEdges();
                Edge edgeToAppend = FindSmallestWaightEdge();
                edgeToAppend.DrawFinalLine();
                finalEdgeList.Add(edgeToAppend);
                Debug.Log(finalEdgeList.Count);


            }

        }

        public void CurrentPossibleEdges()
        {
            currentPossibleEdges.Clear();
            foreach (Node mainNode in activatedNodes)
            {
                foreach (Node tempNode in mainNode.linkedNodes)
                {
                    Edge tempEdge = new Edge();
                    tempEdge.source = mainNode; tempEdge.target = tempNode;
                    tempEdge.CalculateWeight();
                    if (!activatedNodes.Contains(tempNode))
                    {
                        currentPossibleEdges.Add(tempEdge);
                    }
                }
            }
        }


        public Edge FindSmallestWaightEdge()
        {
            float minWeight = 10000;
            Edge rememberedEdge = null;
            foreach (Edge tempEdge in currentPossibleEdges)
            {
                if (tempEdge.weight < minWeight)
                {
                    minWeight = tempEdge.weight;
                    rememberedEdge = tempEdge;
                }
            }
            activatedNodes.Add(rememberedEdge.target);
            return rememberedEdge;
        }

    }


}