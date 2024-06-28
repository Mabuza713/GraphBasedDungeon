using GraphDungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphDungeon
{
    public class PathFinder : MonoBehaviour
    {
        public Grid grid;

        private void Awake()
        {
            grid = GetComponent<Grid>();
            

        }
        private void Start()
        {
            
        }

    }
}

