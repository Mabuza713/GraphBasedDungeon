using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Graphdunegon
{
    public class Grid : MonoBehaviour
    {
        public LayerMask unwalkableMask;
        public Vector3Int worldSize;
        public Node[,,] grid;
        public float nodeRadious;

        private float nodeDiameter;
        public int gridSizeX, gridSizeY, gridSizeZ;
        public void Awake()
        {
            Vector3Int worldSize = transform.GetComponent<PlaceRooms>().worldBoundry;

            nodeDiameter = nodeRadious * 2;
            gridSizeX = Mathf.RoundToInt(worldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(worldSize.y / nodeDiameter);
            gridSizeZ = Mathf.RoundToInt(worldSize.z / nodeDiameter);


            CreateGrid();
        }

        public void CreateGrid()
        {
            grid = new Node[gridSizeX, gridSizeY, gridSizeZ]; // Creating grid with Worldsize / node radious amount of cells
            Vector3 worldBottomLeft = transform.position - Vector3.right * worldSize.x - Vector3.forward * worldSize.z - Vector3.up * worldSize.y; // calculating place of first node on the grid

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    for (int z = 0; z < gridSizeZ; z++)
                    {
                        Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadious) + Vector3.forward * (z * nodeDiameter + nodeRadious) + Vector3.up * (y * nodeDiameter + nodeRadious);
                        grid[x, y, z] = new Node(new Vector3(x,y,z), true, null, new Vector3Int((int)nodeDiameter, (int)nodeDiameter, (int)nodeDiameter), new Vector3Int((int)worldPoint.x, (int)worldPoint.y, (int)worldPoint.z), Node.cellType.None);
                        grid[x, y, z].worldPosition = worldPoint;

                    }
                }
            }
        }

        public Node NodeFromWorldPosition(Vector3 position) 
        {
            int x = Mathf.RoundToInt(position.x - 2.5f) / 5;
            int y = Mathf.RoundToInt(position.y - 2.5f) / 5;
            int z = Mathf.RoundToInt(position.z - 2.5f) / 5;

            return grid[x, y, z];
        }

        public List<Node> GetNeighboringCells(Node node)
        {
            List<Node> result = new List<Node>();
            for (int i = -1; i <= 1; i++) 
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = -1; k <= 1; k++)
                    {
                        if (i == 0 && j == 0 && k == 0) // We dont need to append checked node
                        {
                            continue;
                        }
                        if (node.positionOnGrid.x + i > 0 && node.positionOnGrid.x + i < gridSizeX &&
                                 node.positionOnGrid.y + j > 0 && node.positionOnGrid.y + j < gridSizeY &&
                                 node.positionOnGrid.z + k > 0 && node.positionOnGrid.z + k < gridSizeZ &&
                                 Mathf.Abs(i) + Mathf.Abs(j) + Mathf.Abs(k) <= 1)
                        {

                            result.Add(grid[(int)node.positionOnGrid.x + i, (int)node.positionOnGrid.y + j, (int)node.positionOnGrid.z + k]);
                        }
                    }

                }

            }
            return result;
        }

        private void OnDrawGizmos()
        {
            if (grid != null)
            {
                foreach (Node node in grid)
                {
                    Gizmos.color = Color.red;
                    if (node.isWalkable == false)
                    {
                        //Gizmos.DrawCube(node.worldPosition, Vector3.one * nodeDiameter);

                    }
                }

            }
        }
    }
}


