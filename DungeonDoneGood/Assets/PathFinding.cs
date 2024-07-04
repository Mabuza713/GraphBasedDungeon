using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
                    FindClosestRoom(endNode);
                    FindClosestRoom(startNode);
                    TracePath(endNode, startNode);
                    DeleteWalls(endNode, startNode);
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
        public LayerMask roomMask;
        public void FindClosestRoom(Node node)
        {
            Collider[] startColliders = Physics.OverlapSphere(node.worldPosition, 5, roomMask);
            float minDistance = 100000;
            GameObject remembered = null; 
            foreach (Collider colider in startColliders)
            {
                if (Vector3.Distance(colider.transform.gameObject.transform.position, node.worldPosition) < minDistance)
                {
                    minDistance = Vector3.Distance(colider.transform.gameObject.transform.position, node.worldPosition);
                    remembered = colider.transform.gameObject;

                }
            }
            node.intersectingObject = remembered.transform.parent.parent.gameObject;
        }


        public GameObject hallwayStraight;
        public GameObject hallwayUp;

        public void TracePath(Node endNode, Node startNode)
        {
            Node currentNode = endNode;

            while (currentNode != startNode) 
            {

                GameObject temp = Instantiate(hallwayStraight);
                temp.transform.position = currentNode.worldPosition - Vector3.up * 3f;
                currentNode.isWalkable = false;
                currentNode = currentNode.parentNode;
            }
            currentNode.isWalkable = false;
            GameObject newTemp = Instantiate(hallwayStraight);
            newTemp.transform.position = currentNode.worldPosition - Vector3.up * 3f;
        }
        public LayerMask Wall;
        public void DeleteWalls(Node endNode, Node startNode)
        {
            Physics.SyncTransforms();

            Node currentNode = endNode;
            RaycastHit hitEnd;
            if (Physics.Raycast(currentNode.worldPosition, -(currentNode.worldPosition - new Vector3(currentNode.intersectingObject.transform.position.x, currentNode.intersectingObject.transform.position.y, currentNode.intersectingObject.transform.position.z)), out hitEnd, 5f, Wall))
            {
                Destroy(hitEnd.transform.gameObject);
            }

            while (currentNode != startNode)
            {
                RaycastHit hit1;
                
                if( Physics.Raycast(currentNode.worldPosition, (currentNode.parentNode.worldPosition - currentNode.worldPosition).normalized,out hit1, 5f,Wall))
                {
                    Destroy(hit1.transform.gameObject);
                }

                RaycastHit hit2;

                if (Physics.Raycast(currentNode.parentNode.worldPosition, (currentNode.worldPosition - currentNode.parentNode.worldPosition).normalized, out hit2, 5f,Wall))
                {
                    Destroy(hit2.transform.gameObject);
                }

                currentNode = currentNode.parentNode;
            }
            RaycastHit hitStart;

            if (Physics.Raycast(currentNode.worldPosition, -(currentNode.worldPosition - new Vector3(currentNode.intersectingObject.transform.position.x, currentNode.intersectingObject.transform.position.y, currentNode.intersectingObject.transform.position.z)), out hitStart, 5f, Wall))
            {
                Destroy(hitStart.transform.gameObject);
            }

        }

        public Vector3 CalculateDirection(Vector3 position1, Vector3 position2)
        {
            Vector3 result = (position1 - position2).normalized;

            return result;

        }
    }


}
