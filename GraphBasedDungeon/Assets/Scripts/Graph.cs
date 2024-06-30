using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GraphDungeon
{
    public class Node
    {
        // Variables used to generate Rooms and Edges
        public BoundsInt bounds;
        public GameObject GameObj;
        public List<Node> linkedNodes = new List<Node>();


        // Variables used for path finding algorithm
        public Vector3 worldPosition = Vector3.zero;
        public int posAtGirdX;
        public int posAtGirdY;
        public int posAtGirdZ;
        public bool walkable;
        public Node parent;
        public Node mainNode;

        // Variables used for A*
        public float gCost;
        public float hCost;
        public float finalCost;


    }

    public class Edge : IComparable<Edge>
    {
        public Node source; public Node target; // source Node is first node from which edge is starting
        public float weight;

        public void CalculateWeight()
        {
            weight = Vector3.Distance(source.bounds.position, target.bounds.position);
        }

        public void Draw()
        {
            Debug.DrawLine(source.bounds.position, target.bounds.position, Color.red, 10000f);
        }

        public void DrawFinalLine()
        {
            Debug.DrawLine(source.bounds.position, target.bounds.position, Color.green, 10000f);
        }



        public int CompareTo(Edge comparedEdge)
        {
            return (int)(this.weight - comparedEdge.weight);
        }
    }

}
