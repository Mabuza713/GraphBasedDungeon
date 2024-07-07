using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


namespace Graphdunegon
{
    public class PrimAlgo : MonoBehaviour
    {
        public List<Edge> finalEdgeList = new List<Edge>();
        public List<Node> activatedNodesList = new List<Node>(); // already traversed Nodes
        public List<Edge> PrimAlgorithm()
        {
            Node node = transform.GetComponent<PlaceRooms>().nodeList[0];
            activatedNodesList.Add(node);
            while (finalEdgeList.Count != transform.GetComponent<PlaceRooms>().nodeList.Count - 1)
            {
                Edge edgeToAppend = CurrentPossibleEdges();
                //edgeToAppend.DrawFinalLines();
                finalEdgeList.Add(edgeToAppend);

            }
            return finalEdgeList;
        }

        public Edge CurrentPossibleEdges()
        {
            float minWeight = 100000f;
            Edge result = null;
            Node rememberedTargetNode = null; Node rememberedTempNode = null; Node rememberedMainNode = null;
            Node startNode = null;
            Node endNode = null;
            foreach (Node mainNode in activatedNodesList)
            {
                foreach (Node tempNode in mainNode.linkedNodes)
                {
                    if (!activatedNodesList.Contains(tempNode) && Vector3.Distance(mainNode.worldPosition, tempNode.worldPosition) < minWeight)
                    {
                        float minDistance = 1000000f;
                        foreach (Node tempStartNode in mainNode.intersectingObject.GetComponent<Room>().destinationNodesList)
                        {
                            foreach (Node tempEndNode in tempNode.intersectingObject.GetComponent<Room>().destinationNodesList)
                            {
                                if (Vector3.Distance(tempStartNode.worldPosition, tempEndNode.worldPosition) < minDistance)
                                {
                                    startNode = tempStartNode;
                                    endNode = tempEndNode;
                                    minDistance = Vector3.Distance(tempStartNode.worldPosition, tempEndNode.worldPosition);
                                }
                            }
                        }


                        Edge tempEdge = new Edge(startNode, endNode);
                        rememberedTargetNode = tempNode;
                        rememberedMainNode = mainNode; rememberedTempNode = tempNode;
                        minWeight = Vector3.Distance(mainNode.worldPosition, tempNode.worldPosition);
                        result = tempEdge;

                    }
                }
            }

            rememberedMainNode.intersectingObject.GetComponent<Room>().destinationNodesList.Remove(result.sourceNode);
            rememberedTempNode.intersectingObject.GetComponent<Room>().destinationNodesList.Remove(result.targetNode);
            activatedNodesList.Add(rememberedTargetNode);
            return result;
        }



    }
}
