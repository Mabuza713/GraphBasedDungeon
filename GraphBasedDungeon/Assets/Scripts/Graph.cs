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
        public BoundsInt bounds;
        public GameObject GameObj;
        public List<Node> linkedNodes = new List<Node>();
        
        enum noderType
        {
            room,
            hallway,
            stairs,
            Offset
        }


    }

    public class Edge : IComparable<Edge>
    {
        public Node source; public Node target;
        public float weight;
        
        public void CalculateWeight()
        {
            weight = Vector3.Distance(source.bounds.position, target.bounds.position);
        }
        
        public void Draw()
        {
            Debug.DrawLine(source.bounds.position, target.bounds.position, Color.red,10000f);
        }

        public void DrawFinalLine(Vector3 point1, Vector3 point2)
        {
            Debug.DrawLine(point1, point2, Color.green, 10000f);
        }



        public int CompareTo(Edge comparedEdge)
        {
            return (int)(this.weight - comparedEdge.weight);
        }
    }

}
