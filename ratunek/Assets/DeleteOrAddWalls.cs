using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeleteOrAddWalls : MonoBehaviour
{
    public GameObject wallPrefab;

    public void AddOrDestroyWall()
    {
        Physics.SyncTransforms();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.forward, out hit, 3))
        {
            Destroy(hit.transform.gameObject);
        }
        else
        {
            GameObject wall = Instantiate(wallPrefab);
            wall.transform.position = transform.position - transform.forward * 2.5f;
            GameObject tempEmpty = new GameObject();
            tempEmpty.transform.SetParent(transform);
            tempEmpty.transform.position = transform.position - transform.forward * 5;
            wall.transform.LookAt(tempEmpty.transform.position);


        }


    }
}
