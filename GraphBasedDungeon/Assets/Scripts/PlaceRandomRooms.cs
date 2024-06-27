using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


namespace GraphDungeon
{

    public class PlaceRandomRooms : MonoBehaviour
    {
        

        [SerializeField] int RoomAmount = 15;
        [SerializeField] Vector3 boundryVec;
        [SerializeField] Vector3 roomMaxSize;
        [SerializeField] int howFarApart;
        public List<Node> listOfNodes = new List<Node>();
        public GameObject prefab;

        public List<Edge> Edges = new List<Edge>();

        public Material materialRoom;

        private void Start()
        {
            List<Node> Nodes = new List<Node>();

            PlaceRooms();
            Triangulation();


        }

        private void PlaceRooms()
        {
            for (int i = 0; i < RoomAmount; i++)
            {
                // Values will change to false if there would be colliding room
                bool place = true;

                // We draw random position and size of a room and offset of it to make sure rooms are placed apart of each other
                Vector3Int position = new Vector3Int(
                    (int)Random.Range(-boundryVec.x, boundryVec.x),
                    (int)Random.Range(-boundryVec.y, boundryVec.y),
                    (int)Random.Range(-boundryVec.z, boundryVec.z));

                Vector3Int size = new Vector3Int(
                    (int)Random.Range(3, 3 + roomMaxSize.x),
                    (int)Random.Range(3, 3 + roomMaxSize.y),
                    (int)Random.Range(3, 3 + roomMaxSize.z));

                // Creating tempNode and its offset it will work only for rooms on cube plain
                Node tempNode = new Node();
                tempNode.bounds = new BoundsInt(position, size);

                Node offset = new Node();
                offset.bounds = new BoundsInt(position, size + new Vector3Int(howFarApart, 0, howFarApart)); // might need to tweek values there

                // Check if any already placed room is colliding with current room
                foreach (Node Node in listOfNodes)
                {
                    if (BoundryCheck(Node, offset)) // if offset is intersetcing with any other room we cannot place it
                    {
                        place = false;
                        break;
                    }
                }

                //Checking if room is not outside of our generation space
                if (tempNode.bounds.xMin < -boundryVec.x || tempNode.bounds.xMax >= boundryVec.x ||
                    tempNode.bounds.yMin < -boundryVec.y || tempNode.bounds.yMax >= boundryVec.y ||
                    tempNode.bounds.zMin < -boundryVec.z || tempNode.bounds.zMax >= boundryVec.z)
                {
                    place = false;
                }

                if (place == true)
                {

                    listOfNodes.Add(tempNode);
                    PlaceRoom(tempNode.bounds.position, tempNode.bounds.size, tempNode);
                }


            }
        }


        private bool BoundryCheck(Node node1, Node node2)
        {
            if ((Mathf.Abs(node1.bounds.position.x) >= (Mathf.Abs(node2.bounds.position.x) + node2.bounds.size.x)) ||
                (Mathf.Abs(node1.bounds.position.y) >= (Mathf.Abs(node2.bounds.position.y) + node2.bounds.size.y)) ||
                (Mathf.Abs(node1.bounds.position.z) >= (Mathf.Abs(node2.bounds.position.z) + node2.bounds.size.z)) ||
                ((Mathf.Abs(node1.bounds.position.x) + node1.bounds.size.x) <= Mathf.Abs(node2.bounds.position.x)) ||
                ((Mathf.Abs(node1.bounds.position.y) + node1.bounds.size.y) <= Mathf.Abs(node2.bounds.position.y)) ||
                ((Mathf.Abs(node1.bounds.position.z) + node1.bounds.size.z) <= Mathf.Abs(node2.bounds.position.z)))
            {
                return false;
            }
            return true;
        }

        void PlaceRoom(Vector3Int position, Vector3Int size, Node node)
        {
            GameObject Obj = Instantiate(prefab, position, Quaternion.identity);
            Obj.transform.localScale = size;
            Obj.GetComponent<MeshRenderer>().material = materialRoom;
            node.GameObj = Obj;
        }


        void Triangulation() // XD lowkey bad but no idea 
        {
            for (int i = 0; i < listOfNodes.Count; i++)
            {
                for (int j = i + 1; j < listOfNodes.Count; j++)
                {
                    for (int k = j + 1; k < listOfNodes.Count; k++)
                    {
                        for (int l = k + 1; l < listOfNodes.Count; l++)
                        {
                            Physics.SyncTransforms();
                            Vector4 CircumCircle = CalculateCircumsphere(listOfNodes[i].bounds.position, listOfNodes[j].bounds.position, listOfNodes[k].bounds.position, listOfNodes[l].bounds.position);
                            Collider[] collider = Physics.OverlapSphere(new Vector3(CircumCircle[1], CircumCircle[2], CircumCircle[3]), CircumCircle[0]);
                            Physics.SyncTransforms();
                            if (collider.Length <= 4)
                            {
                                AddToList(listOfNodes[i], listOfNodes[j], listOfNodes[k], listOfNodes[l]);
                                AddToList(listOfNodes[l], listOfNodes[j], listOfNodes[k], listOfNodes[i]);
                                AddToList(listOfNodes[i], listOfNodes[l], listOfNodes[k], listOfNodes[j]);
                                AddToList(listOfNodes[i], listOfNodes[j], listOfNodes[l], listOfNodes[k]);

                            }

                        }

                    }

                }

            }
        }
        private void AddToList(Node node1, Node node2, Node node3, Node nodeAppended)
        {
            nodeAppended.linkedNodes.Add(node1);
            nodeAppended.linkedNodes.Add(node2);
            nodeAppended.linkedNodes.Add(node3);

            Edge edge1 = new Edge(); Edge edge2 = new Edge(); Edge edge3 = new Edge();
            edge1.source = nodeAppended; edge1.target = node1;
            edge2.source = nodeAppended; edge2.target = node2;
            edge3.source = nodeAppended; edge3.target = node3;

            edge1.Draw() ; edge2.Draw() ; edge3.Draw();

            Edges.Add(edge1);
            Edges.Add(edge2);
            Edges.Add(edge3);
        }

        private Vector4 CalculateCircumsphere(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4)
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
