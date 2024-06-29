using GraphDungeon;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace GraphDungeon
{
    public class Grid : MonoBehaviour
    {

        // might need to add func to change radious of node eetcc
        public LayerMask unwalkableMask;
        public Vector3 gridWorldSize;

        public Node[,,] grid;
        public void Awake()
        {
            gridWorldSize = transform.GetComponent<PlaceRandomRooms>().boundryVec * 2;
            grid = new Node[Mathf.RoundToInt(gridWorldSize.x), Mathf.RoundToInt(gridWorldSize.y), Mathf.RoundToInt(gridWorldSize.z)];
        }
        public void CreateGrid()
        {

            
            Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.z / 2 - Vector3.up * gridWorldSize.y / 2;
            for (int x = 0; x < gridWorldSize.x; x++)
            {
                for (int y = 0; y < gridWorldSize.y; y++)
                {
                    for (int z = 0; z < gridWorldSize.z; z++)
                    {
                        Vector3 worldPos = worldBottomLeft + Vector3.right * (x) + Vector3.forward * (z ) + Vector3.up * (y );
                        Physics.SyncTransforms();
                        bool isWalkable = !(Physics.CheckSphere(worldPos,0.45f, unwalkableMask));
                        if (grid[x, y, z] == null) { grid[x, y, z] = new Node(); }
                        
                        grid[x, y, z].worldPosition = worldPos;
                        grid[x, y, z].posAtGirdX = x; grid[x, y, z].posAtGirdY = y; grid[x, y, z].posAtGirdZ = z;
                        grid[x, y, z].walkable = isWalkable;
                    }
                }
            }

        }
        public Vector3Int NodeFromWorldPoint(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
            float percentY = (worldPosition.y + gridWorldSize.y/2) / gridWorldSize.y; // Assuming y-axis in worldPosition
            float percentZ = (worldPosition.z + gridWorldSize.z/2) / gridWorldSize.z; // Assuming z-axis in worldPosition

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);
            percentZ = Mathf.Clamp01(percentZ);

            int x = Mathf.RoundToInt((gridWorldSize.x - 1) * percentX);
            int y = Mathf.RoundToInt((gridWorldSize.y - 1) * percentY);
            int z = Mathf.RoundToInt((gridWorldSize.z - 1) * percentZ);

            return new Vector3Int(x,y,z);
        }
        public List<Node> GetNeighboringCells(Node node)
        {
            List<Node> result = new List<Node>();
            for (int i = -1; i <= 1; i++) // Allows us to get neighboring cells on x axis
            {
                for (int j = -1; j <= 1; j++) 
                {
                    for (int k = -1; k <= 1; k++) 
                    {
                        if (i == 0 && j == 0 && k == 0) // We dont need to append checked node
                        {
                            continue;
                        }
                        else if (node.posAtGirdX + i >= 0 && node.posAtGirdX + i < gridWorldSize.x &&
                                 node.posAtGirdY + j >= 0 && node.posAtGirdY + j < gridWorldSize.y &&
                                 node.posAtGirdZ + k >= 0 && node.posAtGirdZ + k < gridWorldSize.z )
                        {

                            result.Add(grid[node.posAtGirdX + i, node.posAtGirdY + j, node.posAtGirdZ + k]);

                            

                        }
                    }

                }

            }
            return result;
        }


    }
}
