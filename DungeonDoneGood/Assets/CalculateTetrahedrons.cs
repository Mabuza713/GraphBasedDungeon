using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;


namespace Graphdunegon
{
    public class CalculateTetrahedrons : MonoBehaviour
    {
        public void CalculateConnections()
        {
            List<Node> nodeList = transform.GetComponent<PlaceRooms>().nodeList;
            for (int i = 0; i < nodeList.Count; i++)
            {
                for (int j = i + 1;  j < nodeList.Count; j++)
                {
                    for (int k = j + 1; k < nodeList.Count; k++)
                    {
                        for(int l = k + 1; l < nodeList.Count; l++)
                        {
                            Vector4 result = CalculateCircumsphere(nodeList[i].worldPosition, nodeList[j].worldPosition, nodeList[k].worldPosition, nodeList[l].worldPosition);
                            Physics.SyncTransforms();
                            Collider[] colliders = Physics.OverlapSphere(new Vector3(result[1], result[2], result[3]), result[0]);
                            if (colliders.Length <= 4)
                            {
                                AddNodesToLinkedNodesList(nodeList[i], nodeList[j], nodeList[k], nodeList[l]);
                                AddNodesToLinkedNodesList(nodeList[l], nodeList[j], nodeList[k], nodeList[i]);
                                AddNodesToLinkedNodesList(nodeList[i], nodeList[l], nodeList[k], nodeList[j]);
                                AddNodesToLinkedNodesList(nodeList[i], nodeList[j], nodeList[l], nodeList[k]);


                            }


                        }
                    }
                }
            }
        }
        public void AddNodesToLinkedNodesList(Node node1, Node node2, Node node3, Node currentNode)
        {
            currentNode.linkedNodes.Add(node1);
            currentNode.linkedNodes.Add(node2);
            currentNode.linkedNodes.Add(node3);

        }


        // Given four points find right circumsphere that is intersected by all of given points
        // first returned value is radious of sphere

        public Vector4 CalculateCircumsphere(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4) 
        {
            Matrix4x4 xMatrix = new Matrix4x4(new Vector4(point1.sqrMagnitude, point2.sqrMagnitude, point3.sqrMagnitude, point4.sqrMagnitude),
                                              new Vector4(point1.x, point2.x, point3.x, point4.x),
                                              new Vector4(point1.z, point2.z, point3.z, point4.z),
                                              new Vector4(1, 1, 1, 1));

            Matrix4x4 yMatrix = new Matrix4x4(new Vector4(point1.sqrMagnitude, point2.sqrMagnitude, point3.sqrMagnitude, point4.sqrMagnitude),
                                              new Vector4(point1.x, point2.x, point3.x, point4.x),
                                              new Vector4(point1.z, point2.z, point3.z, point4.z),
                                              new Vector4(1, 1, 1, 1));

            Matrix4x4 zMatrix = new Matrix4x4(new Vector4(point1.sqrMagnitude, point2.sqrMagnitude, point3.sqrMagnitude, point4.sqrMagnitude),
                                              new Vector4(point1.x, point2.x, point3.x, point4.x),
                                              new Vector4(point1.y, point2.y, point3.y, point4.y),
                                              new Vector4(1, 1, 1, 1));


            Matrix4x4 aMatrix = new Matrix4x4(new Vector4(point1.x, point2.x, point3.x, point4.x),
                                              new Vector4(point1.y, point2.y, point3.y, point4.y),
                                              new Vector4(point1.z, point2.z, point3.z, point4.z),
                                              new Vector4(1, 1, 1, 1));

            Matrix4x4 cMatrix = new Matrix4x4(new Vector4(point1.sqrMagnitude, point2.sqrMagnitude, point3.sqrMagnitude, point4.sqrMagnitude),
                                              new Vector4(point1.x, point2.x, point3.x, point4.x),
                                              new Vector4(point1.y, point2.y, point3.y, point4.y),
                                              new Vector4(point1.z, point2.z, point3.z, point4.z));

            float radious = (Mathf.Sqrt(((xMatrix.determinant * xMatrix.determinant) + (yMatrix.determinant * yMatrix.determinant) + (zMatrix.determinant * zMatrix.determinant)) - (4 * cMatrix.determinant * aMatrix.determinant)) / (2 * Mathf.Abs(aMatrix.determinant)));
            Vector4 result = new Vector4(radious, xMatrix.determinant / (2 * aMatrix.determinant), -1 * yMatrix.determinant / (2 * aMatrix.determinant), zMatrix.determinant / (2 * aMatrix.determinant));
            return result;
        }


    }
}

