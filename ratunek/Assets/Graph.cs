using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graphdunegon
{

    public class Node
    {
        // Variables used to generate Rooms
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

        // Type
        public cellType nodeType;
        public enum cellType
        {
            None,
            Room,
            Hallway
        }


        public Node(Vector3 _positionOnGrid, bool _isWalkable, Node _parentNode)
        {
            positionOnGrid = _positionOnGrid;
            isWalkable = _isWalkable;
            parentNode = _parentNode;
            nodeType = Node.cellType.None;
        }
    }

    public class Edge
    {
        public Node sourceNode; public Node targetNode;

        public Edge(Node _sourceNode, Node _targetNode)
        {

            sourceNode = _sourceNode;
            targetNode = _targetNode;

        }

        public void DrawFinalLines()
        {
            Debug.DrawLine(sourceNode.worldPosition, targetNode.worldPosition, Color.green, 10000000f);
        }
    }

}