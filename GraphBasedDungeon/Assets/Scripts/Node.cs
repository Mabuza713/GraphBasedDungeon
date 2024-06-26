using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public BoundsInt bounds;
    public GameObject GameObj;

    public void Awake()
    {
        GameObj = transform.gameObject;
    }
    enum chamberType
    { 
        room,
        hallway,
        stairs,
        Offset
    }




}
