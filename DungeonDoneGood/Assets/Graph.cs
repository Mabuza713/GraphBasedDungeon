using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graphdunegon
{

    public class Node
    {
        // Variables used to generate Rooms and Edges
        public BoundsInt bounds;
        public GameObject intersectingObject;
        public List<Node> linkedNodes = new List<Node>();

        // Variables used for path finding
        public Vector3 worldPosition;
        public Vector3 positionOnGrid;
        public bool isWalkable;
        public Node parentNode;

        // Cost variables
        public float gCost;
        public float hCost;
        public float fCost;

        //type
        public cellType nodeType;
        public enum cellType
        {
            None,
            Room,
            Hallway
        }


        public Node(Vector3 _positionOnGrid, bool _isWalkable, Node _parentNode, Vector3Int size, Vector3Int position, cellType nodeType)
        {
            bounds = new BoundsInt(position, size);
            positionOnGrid = _positionOnGrid;
            isWalkable = _isWalkable;
            parentNode = _parentNode;
            nodeType = Node.cellType.None;
        }
    }

    public class Edge : MonoBehaviour
    {
        public Node sourceNode; public Node targetNode;
        public float weight;

        public Edge(Node _sourceNode, Node _targetNode)
        {
            
            sourceNode = _sourceNode;
            targetNode = _targetNode;
            weight = Vector3.Distance(_sourceNode.bounds.position, _targetNode.bounds.position);

        }

        public void DrawFinalLines()
        {
            Debug.DrawLine(sourceNode.bounds.position, targetNode.bounds.position, Color.red, 10000000f);
        }
    }
    
}