using GraphDungeon;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ProjectWindowCallback;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace GraphDungeon
{
    public class PathFinder : MonoBehaviour
    {
        public GameObject pref;
        public Grid grid;
        public LayerMask hallwaysMask;
        public void Awake()
        {
            grid = GetComponent<Grid>();
        }

        public void FindPath(Node startingNode, Node endNode)
        {
            List<Node> openSet = new List<Node>();
            HashSet<Node> closeSet = new HashSet<Node>(); // using hashset for better performance
            openSet.Add(startingNode);
            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                // This part 
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].finalCost <= currentNode.finalCost) // might need to change
                    {
                        if (openSet[i].hCost <= currentNode.hCost)
                        {
                            currentNode = openSet[i];


                        }
                    } 
                }
                openSet.Remove(currentNode); closeSet.Add(currentNode);
                Debug.Log(Physics.CheckSphere(currentNode.worldPosition, 1f, transform.GetComponent<Grid>().unwalkableMask) && closeSet.Count > 1);

                if (currentNode == endNode)
                {
                    Debug.Log("sds");
                    TraceBackPath(startingNode, endNode);
                    return;
                }


                foreach (Node neighbour in grid.GetNeighboringCells(currentNode))
                {
                    if (closeSet.Contains(neighbour) ) { continue; }

                    float costToNeighbour = currentNode.gCost + Vector3.Distance(currentNode.worldPosition, neighbour.worldPosition);
                    if (costToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = costToNeighbour;
                        neighbour.hCost = Vector3.Distance(endNode.worldPosition, neighbour.worldPosition);
                        neighbour.finalCost = neighbour.gCost + neighbour.hCost;
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
        }

        public void TraceBackPath(Node startNode, Node endNode)
        {
            
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                if (Physics.CheckSphere(currentNode.worldPosition, 0.4f, hallwaysMask))
                {
                    Debug.Log("hereeee");
                    currentNode.walkable = false;
                    currentNode = currentNode.parent;
                }
                else
                {
                    GameObject inst = Instantiate(pref);
                    inst.transform.position = currentNode.worldPosition;
                    currentNode.walkable = false;
                    currentNode = currentNode.parent;
                }
                
                
            }
            
            
        }
    }


}

