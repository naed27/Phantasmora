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

    // ------------------ mesh properties

    private Mesh _mesh;
    private Player _player;
    private LayerMask _masksToCollideWith;

    public float _currentRayLength;

    private int _raySpan;
    private int _rayCount;
    private float _rayAngle;
    private bool _breatheEffect = false;
    private float _breatheCap = 2;
    private float _breatheScore = 0;
    [SerializeField] private float _breathePace = 0.05f;
    private bool _allowBreathing = false;


    private Vector3[] _vertices;
    private int[] _triangles;
    private Vector3 _meshDrawOrigin;

    // ---------------- setters and getters

    public float RayLength { get { return _currentRayLength; } }
    public float MaximumRayLength { get { return _maximumRayLength; } }
    public float OriginalRayLength { get { return _originalRayLength; } }

    public void Start()
    {
        // Create a new mesh
        _mesh = new Mesh();
        _meshDrawOrigin = Vector3.zero;
        GetComponent<MeshFilter>().mesh = _mesh;

        // Setup mesh
        _raySpan = 360;
        _rayCount = 360;
        _rayAngle = _raySpan / _rayCount;
        _currentRayLength = _originalRayLength;

        _vertices = new Vector3[_rayCount + 2];
        _triangles = new int[_rayCount * 3];
        _vertices[0] = _meshDrawOrigin;
    }

    public void Init(Player player, LayerMask maskToCollideWith, bool breatheEffect = false)
    {
        // Setup Properties
        _player = player;
        _masksToCollideWith = maskToCollideWith;

        _breatheEffect = breatheEffect;
        _breatheCap = _breathePace * 3.5f;
        _currentRayLength = _originalRayLength;

    }

    void Update()
    {
        UpdateFieldOfView();
        UpdateMeshPosition();
        Breathe();

    }

    private void Breathe()
    {
        if (!_player) return;

        if (ReachedOriginalLength() && !_allowBreathing)
            AllowBreathing();

        if (_breatheEffect && _allowBreathing)
        {
            _currentRayLength -= _breathePace * Time.fixedDeltaTime;
            _breatheScore += _breathePace * Time.fixedDeltaTime;
            if (_breatheScore >= _breatheCap || _breatheScore <= 0)
                _breathePace *= -1;
        }
    }


    private void UpdateMeshPosition()
    {
        if (!_player) return;
        Vector3 currentPosition = _player.transform.position;
        currentPosition.z = -1;
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
            _currentRayLength += (_maximumRayLength - _currentRayLength) * Time.fixedDeltaTime;
    }

    public void Shrink(float limit = 0)
    {
        if (_currentRayLength > limit) 
            _currentRayLength -= (_shrinkSpeed) * Time.fixedDeltaTime;
    }

    public void Resize(float goal = 0)
    {
        if(_currentRayLength!=goal)
            _currentRayLength = Mathf.Lerp(_currentRayLength, goal, (_shrinkSpeed) * Time.fixedDeltaTime);
    }

    public bool ReachedOriginalLength() => _currentRayLength >= _originalRayLength - 0.001f || _currentRayLength <= _originalRayLength + 0.001f;
    public bool ReachedSpecificLength(float length) => _currentRayLength >= length - 0.001f || _currentRayLength <= length + 0.001f;

    public void AllowBreathing() => _allowBreathing = true;
    public void DisallowBreathing() => _allowBreathing = false;


}