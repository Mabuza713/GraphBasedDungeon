using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graphdunegon
{
    public class PathFinding : MonoBehaviour
    {
        public void FindPath(Node startNode, Node endNode)
        {
            List<Node> openSet = new List<Node>();
            HashSet<Node> closeSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++) // finding most optimal node
                {
                    if (openSet[i].fCost <= currentNode.fCost)
                    {
                        if (openSet[i].hCost >= currentNode.hCost)
                        {
                            currentNode = openSet[i];

                        }
                    }
                }
                openSet.Remove(currentNode); closeSet.Add(currentNode);

                if (currentNode == endNode)
                {
                    TracePath(endNode, startNode);
                    // Trace back the path 
                    break;
                }
                foreach (Node neighbour in transform.GetComponent<Grid>().GetNeighboringCells(currentNode))
                {
                    if (closeSet.Contains(neighbour)|| !neighbour.isWalkable) { continue; }

                    float costToNeighbour = currentNode.gCost + Vector3.Distance(currentNode.worldPosition, neighbour.worldPosition);
                    if (costToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = costToNeighbour;
                        neighbour.hCost = Vector3.Distance(neighbour.worldPosition, endNode.worldPosition);
                        neighbour.parentNode = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }

                    }
                }
            }
        }

        public void TracePath(Node endNode, Node startNode)
        {
            Node currentNode = endNode;

            while (currentNode != startNode) 
            {
                currentNode.isWalkable = false;
                currentNode = currentNode.parentNode;
            }
            currentNode.isWalkable = false;
        }
    }


}
