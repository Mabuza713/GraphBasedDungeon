using GraphDungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GraphDungeon
{
    public class PrimAlgo : MonoBehaviour
    {
        public List<Edge> finalEdgeList = new List<Edge>();
        public bool StepInPrimAlgorithm(Node nodeMain)
        {
            float minWeight = 10000000f;
            Node rememberedNode = null;
            foreach (Node nodeTemp in nodeMain.linkedNodes)
            {
                if (transform.GetComponent<PlaceRandomRooms>().listOfNodes.Contains(nodeTemp))
                {
                    Debug.Log("dzilaa");
                    Edge tempEdge = new Edge();
                    tempEdge.source = nodeMain; tempEdge.target = nodeTemp;

                    tempEdge.CalculateWeight();
                    if (tempEdge.weight < minWeight)
                    {
                        minWeight = tempEdge.weight;
                        rememberedNode = nodeTemp;
                    }
                }

            }
            if (rememberedNode == null)
            {
                return false;
            }
            Edge newEdge = new Edge();
            newEdge.source = nodeMain; newEdge.target = rememberedNode;
            newEdge.weight = minWeight;

            finalEdgeList.Add(newEdge);
            return true;
        }

        public void PrimAlgorithm()
        {
            Node node = transform.GetComponent<PlaceRandomRooms>().listOfNodes[0];
            while (transform.GetComponent<PlaceRandomRooms>().listOfNodes.Count > 0)
            {
                Debug.Log("here");               


            }

        }
    }


}