using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Graphdunegon
{
    // First thing ill need to make and initialize Rooms in the 3D plain



    public class MainDunegonManager : MonoBehaviour
    {

        void Start()
        {
            //transform.GetComponent<Grid>().CreateGrid();
            //transform.GetComponent<PlaceRooms>().CreateRooms();
            foreach (var cell in transform.GetComponent<Grid>().grid)
            {
                Debug.Log(cell.nodeType);
            }
        }

    }


}
