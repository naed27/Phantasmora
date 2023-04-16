using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class FieldOfView : MonoBehaviour
{


    void Start()
    {
        // Create a new mesh
        Mesh mesh = new();
        GetComponent<MeshFilter>().mesh = mesh;

        float fov = 90f;
        Vector3 origin = Vector3.zero;
        int rayCount = 360;
        float angle = 0f;
        float angleIncrease = fov / rayCount;
        float viewDistance = 5f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount*3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int i = 0; i < rayCount; i++)
        {
            Vector3 vertex;
            //RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, Helper.GetVectorFromAngle(angle), viewDistance);

            //if (raycastHit2D.collider == null)
                vertex = origin + Helper.GetVectorFromAngle(angle) * viewDistance; // No Hit
            //else
                //vertex = raycastHit2D.point; // Hit Object

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

    }

    void Update()
    {
        // Update the position of the mesh to match the player's position
        transform.SetPositionAndRotation(transform.parent.position, transform.parent.rotation);

        Vector3 currentPosition = transform.position;

        // Set the X and Y position of the mesh to match the player's position
        currentPosition.x = transform.parent.position.x;
        currentPosition.y = transform.parent.position.y;

        // Set the Z position of the mesh to -1
        currentPosition.z = -1;

        // Update the position of the field of view mesh
        transform.position = currentPosition;
    }
}