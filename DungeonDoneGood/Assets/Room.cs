using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

namespace Graphdunegon
{
    public class Room : MonoBehaviour
    {
        public Vector3Int roomSize;
        public List<GameObject> destinationList;
        public List<Node> destinationNodesList = new List<Node>();

        
    }
}
