using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private GameObject Player;

    private Mesh mesh;

    [SerializeField] private int raySpan; // degrees
    [SerializeField] private int rayCount;
    [SerializeField] private int rayLength;
    private float rayAngle;

    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;
    private Vector3 meshDrawOrigin = Vector3.zero;


    void Start()
    {
        // Create a new mesh

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

    }

    void Update()
    {
        UpdateFieldOfView();
        UpdateMeshPosition();
    }


    private void UpdateMeshPosition()
    {
        Vector3 currentPosition = Player.transform.position;
        currentPosition.z = -1;
        currentPosition.y -= 0.22f;
        transform.position = currentPosition;
    }

    private void UpdateFieldOfView()
    {
        rayAngle = raySpan / rayCount;

        vertices = new Vector3[rayCount + 2];
        uv = new Vector2[vertices.Length];
        triangles = new int[rayCount * 3];

        vertices[0] = meshDrawOrigin;
        Vector2 rayCastOrigin = new(transform.position.x, transform.position.y);

        for (int i = 0; i < vertices.Length-1; i++)
        {
            RaycastHit2D rayCast = Physics2D.Raycast(rayCastOrigin, Helper.GetVectorFromAngle(-(rayAngle * (i))), rayLength, wallLayerMask);

            if (rayCast.collider == null)
                vertices[i + 1] = meshDrawOrigin + Helper.GetVectorFromAngle(-(rayAngle * (i))) * rayLength;
            else
                 vertices[i + 1] = rayCast.point - rayCastOrigin;

            if (i < rayCount)
            {
                triangles[(i * 3) + 0] =     0;
                triangles[(i * 3) + 1] = i + 1;
                triangles[(i * 3) + 2] = i + 2;
            }

        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    private void ChangeColor()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Material material = new Material(Shader.Find("Unlit/Color"));
        Color color = new(1, 1, 1);
        color.a = 0;
        material.color = color;
        meshRenderer.material = material;
    }

}