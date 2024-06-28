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

        Node[,,] grid;
        // 
        public void CreateGrid()
        {
            gridWorldSize = transform.GetComponent<PlaceRandomRooms>().boundryVec * 2 + transform.GetComponent<PlaceRandomRooms>().roomMaxSize;
            grid = new Node[Mathf.RoundToInt(gridWorldSize.x), Mathf.RoundToInt(gridWorldSize.y), Mathf.RoundToInt(gridWorldSize.z)];
            
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

                        grid[x, y, z] = new Node();
                        
                        grid[x, y, z].worldPosition = worldPos;
                        grid[x, y, z].posAtGirdX = x; grid[x, y, z].posAtGirdY = y; grid[x, y, z].posAtGirdZ = z;
                        grid[x, y, z].walkable = isWalkable;
                    }
                }
            }

        }
        private void OnDrawGizmos()
        {
            if (grid != null)
            {
                foreach(Node node in grid)
                {
                    Gizmos.color = (node.walkable) ? Color.white : Color.red;
                    if (!node.walkable)
                    {
                        Gizmos.DrawCube(node.worldPosition, Vector3.one * (.9f));
                    }
                    
                }

            }

        }
    }
}
