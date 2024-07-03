using System.Collections;
using System.Collections.Generic;
using UnityEditor.AI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Graphdunegon
{
    public class PlaceRooms : MonoBehaviour
    {
        [SerializeField] int roomAmount;
        public Vector3Int worldBoundry;

        public List<GameObject> roomPrefabs = new List<GameObject>();
        [SerializeField] int roomSpacing; // stops rooms from intersecting
        
        public List<Node> nodeList = new List<Node>(); // list containing all placed room Nodes
        public List<Edge> edgeList = new List<Edge>(); // list containing all possible connections

        // improted from Grid
        public Node[,,] grid;
        int girdSizeX, girdSizeZ, girdSizeY;

        public void Start()
        {
            grid = transform.GetComponent<Grid>().grid;

            girdSizeX = transform.GetComponent<Grid>().gridSizeX;
            girdSizeY = transform.GetComponent<Grid>().gridSizeY;
            girdSizeZ = transform.GetComponent<Grid>().gridSizeZ;
            CreateRooms();
            transform.GetComponent<CalculateTetrahedrons>().CalculateConnections();
            List<Edge> finalEdgeList = transform.GetComponent<PrimAlgo>().PrimAlgorithm();
            foreach (Edge edge in finalEdgeList)
            {
                transform.GetComponent<PathFinding>().FindPath(edge.sourceNode, edge.targetNode);
            }
        }
        public void CreateRooms()
        {
            for (int i = 0; i < roomAmount; i++)
            {
                GameObject roomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Count)]; // Picks random premade asset

                bool placeNewRoom = true;

                Vector3Int gridPosition = new Vector3Int(Random.Range(0, girdSizeX), // position of room
                                                         Random.Range(0, girdSizeY),
                                                         Random.Range(0, girdSizeZ));



                // Creating two nodes, one bigger than other, it will allow us to check if there is enough space so two rooms wont intersect
                Node tempNode = grid[gridPosition.x, gridPosition.y, gridPosition.z];

                // Checking already created and placed nodes if they intersect with our new room
                foreach (Node node in nodeList) 
                {
                    if (CheckBoundries(tempNode))
                    {
                        placeNewRoom = false;
                        break;
                    }

                }
                //Checking if room is not outside of our generation space
                if (tempNode.worldPosition.x - roomPrefab.GetComponent<Room>().roomSize.x /2 < 0 || tempNode.worldPosition.x + roomPrefab.GetComponent<Room>().roomSize.x /2 >= worldBoundry.x ||
                    tempNode.worldPosition.y - roomPrefab.GetComponent<Room>().roomSize.y / 2 < 0 || tempNode.worldPosition.y + roomPrefab.GetComponent<Room>().roomSize.y / 2 >= worldBoundry.y ||
                    tempNode.worldPosition.z - roomPrefab.GetComponent<Room>().roomSize.z / 2 < 0 || tempNode.worldPosition.z + roomPrefab.GetComponent<Room>().roomSize.z / 2 >= worldBoundry.z )
                {
                    placeNewRoom = false;
                }

                // Checking if rooms are not generating outside of grid/space


                if (placeNewRoom)
                {
                    
                    GameObject tempObj = Instantiate(roomPrefab);



                    grid[gridPosition.x, gridPosition.y, gridPosition.z].isWalkable = false; 
                    tempObj.transform.position = tempNode.worldPosition;
                    nodeList.Add(grid[gridPosition.x, gridPosition.y, gridPosition.z]); // appending node to list of all valid rooms
                    foreach (GameObject nodePositionObject in tempObj.GetComponent<Room>().destinationList) // We are appending nodes to list
                    {
                        if (nodePositionObject.transform.position.x > 0 && nodePositionObject.transform.position.x < worldBoundry.x &&
                            nodePositionObject.transform.position.y > 0 && nodePositionObject.transform.position.y < worldBoundry.y &&
                            nodePositionObject.transform.position.z > 0 && nodePositionObject.transform.position.z < worldBoundry.z)
                        {
                            tempObj.GetComponent<Room>().destinationNodesList.Add(transform.GetComponent<Grid>().NodeFromWorldPosition(nodePositionObject.transform.position));

                        }
                    }
                    for (int j = -Mathf.RoundToInt(roomPrefab.GetComponent<Room>().roomSize.x/5 / 2); j <= Mathf.RoundToInt(roomPrefab.GetComponent<Room>().roomSize.x / 5 / 2); ++j)
                    {
                        for (int k = -Mathf.RoundToInt(roomPrefab.GetComponent<Room>().roomSize.y / 5 / 2); k <= Mathf.RoundToInt(roomPrefab.GetComponent<Room>().roomSize.y / 5 / 2); k++)
                        {
                            for (int l = -Mathf.RoundToInt(roomPrefab.GetComponent<Room>().roomSize.z / 5 / 2); l <= Mathf.RoundToInt(roomPrefab.GetComponent<Room>().roomSize.z / 5 / 2); l++)
                            {
                              
                                // might need to add a check if they are still in boundry of grid
                                // positive vals 
                                if (gridPosition.x + j >= girdSizeX || gridPosition.y + k >= girdSizeY || gridPosition.z + l >= girdSizeZ)
                                {
                                    continue;
                                }
                                // negative vals
                                else if (gridPosition.x + j < 0 || gridPosition.y + k < 0 || gridPosition.z + l < 0)
                                {
                                    continue;

                                }
                                else
                                {
                                    grid[gridPosition.x + j, gridPosition.y + k, gridPosition.z + l].isWalkable = false;
                                    grid[gridPosition.x + j, gridPosition.y + k, gridPosition.z + l].intersectingObject = tempObj;
                                    grid[gridPosition.x + j, gridPosition.y + k, gridPosition.z + l].nodeType = Node.cellType.Room;
                                }

                            }
                        }

                    }
                    tempNode.bounds.size = new Vector3Int(5, 5, 5);
                }



            }
        }

        // function that checks if two rooms are intersecting
        public bool CheckBoundries(Node nodeA)
        {
            Physics.SyncTransforms();
            if (Physics.CheckSphere(nodeA.worldPosition, roomSpacing))
            {
                return true;
            }
            return false;

        }
    }
}
