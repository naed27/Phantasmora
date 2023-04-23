using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class MeshField : MonoBehaviour
{
    // ------------------ modifier properties

    [SerializeField] private float _originalRayLength = 0f;
    [SerializeField] private float _maximumRayLength = 0f;
    [SerializeField] private float _shrinkSpeed = 2f;
    [SerializeField] private float _enlargeSpeed = 2f;


    // ------------------ mesh properties

    private Mesh _mesh;
    private Player _playerObject;
    private LayerMask _masksToCollideWith;

    private float _currentRayLength;

    private int _raySpan;
    private int _rayCount;
    private float _rayAngle;

    private Vector3[] _vertices;
    private int[] _triangles;
    private Vector3 _meshDrawOrigin;

    // ---------------- setters and getters

    public float RayLength { get { return _currentRayLength; } }
    public float MaximumRayLength { get { return _maximumRayLength; } }
    public float OriginalRayLength { get { return _originalRayLength; } }

    public void Init(Player player, LayerMask maskToCollideWith)
    {
        // Setup Properties
        _playerObject = player;
        _masksToCollideWith = maskToCollideWith;

        // Create a new mesh
        _mesh = new Mesh();
        _meshDrawOrigin = Vector3.zero;
        GetComponent<MeshFilter>().mesh = _mesh;

        // Setup mesh
        _raySpan = 360;
        _rayCount = 360;
        _currentRayLength = _originalRayLength;
        _rayAngle = _raySpan / _rayCount;

        _vertices = new Vector3[_rayCount + 2];
        _triangles = new int[_rayCount * 3];
        _vertices[0] = _meshDrawOrigin;

    }

    void Update()
    {
        UpdateFieldOfView();
        UpdateMeshPosition();
    }


    private void UpdateMeshPosition()
    {
        Vector3 currentPosition = _playerObject.transform.position;
        currentPosition.z = -1;
        currentPosition.y -= 0.22f;
        transform.position = currentPosition;
    }

    private void UpdateFieldOfView()
    {

        Vector2 rayCastOrigin = new(transform.position.x, transform.position.y);

        for (int i = 0; i < _vertices.Length-1; i++)
        {
            RaycastHit2D rayCast = Physics2D.Raycast(rayCastOrigin, Helper.GetVectorFromAngle(-(_rayAngle * (i))), _currentRayLength, _masksToCollideWith);

            if (rayCast.collider == null)
                _vertices[i + 1] = _meshDrawOrigin + Helper.GetVectorFromAngle(-(_rayAngle * (i))) * _currentRayLength;
            else
                 _vertices[i + 1] = rayCast.point - rayCastOrigin;

            if (i < _rayCount)
            {
                _triangles[(i * 3) + 0] =     0;
                _triangles[(i * 3) + 1] = i + 1;
                _triangles[(i * 3) + 2] = i + 2;
            }

        }

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
    }

    public void Enlarge(float limit)
    {
        if (_currentRayLength < limit) 
            _currentRayLength += (_enlargeSpeed - _currentRayLength) * Time.fixedDeltaTime;
    }

    public void Shrink()
    {
        if (_currentRayLength > 0) 
            _currentRayLength -= (_shrinkSpeed) * Time.fixedDeltaTime;
    }

}