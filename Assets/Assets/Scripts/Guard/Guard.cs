using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    // Properties
    [SerializeField] private float _moveSpeed;
    [SerializeField] private PlayerSensor _playerSensor;

    private Player _player;

    private Vector2 _movement;
    private Animator _animator;
    private Rigidbody2D _rigidBody2D;

    // Position
    private float _scaleX = 1;
    private Tile _previousTile;
    private Tile _goalTile;

    public string targetTile;

    private float xTimer = 0;
    private float yTimer = 0;

    // Game Variables
    private DungeonManager _dungeonManager;


    public void Init(Tile tile, DungeonManager dungeonManager)
    {
        // Initialize properties
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _scaleX = transform.localScale.x;

        // Setup identifications
        _dungeonManager = dungeonManager;
        _player = dungeonManager.Player;

        // Set Position
        transform.position = tile.Position;
        _previousTile = tile;
        _goalTile = tile;
        SetNewGoalFrom(tile);
    }


    private void Update()
    {
        if (_playerSensor.PlayerIsClose)
        {
            if (xTimer <= 0)
                _movement.x = ComparePosition(transform.position.x, _player.transform.position.x, "x");
            if (yTimer <= 0)
                _movement.y = ComparePosition(transform.position.y, _player.transform.position.y, "y");
        }
        else
        {
            if (xTimer <= 0)
                _movement.x = ComparePosition(transform.position.x, _goalTile.Position.x, "x");
            if (yTimer <= 0)
                _movement.y = ComparePosition(transform.position.y, _goalTile.Position.y, "y");
        }

        
        _animator.SetFloat("Speed", _movement.sqrMagnitude);

        if (_movement.x != 0)
        {
            // Flip Animation
            Vector3 scale = transform.localScale;
            scale.x = _scaleX * (_movement.x >= 0 ? 1 : -1);
            transform.localScale = scale;
        }

        DecreaseXYTimers();
    }

    void FixedUpdate()
    {
        float moveSpeed = (_playerSensor.PlayerIsClose) ? _moveSpeed * 4 : _moveSpeed;
        _rigidBody2D.MovePosition(_rigidBody2D.position + moveSpeed * Time.fixedDeltaTime * _movement.normalized);
    }

    private void OnTriggerStay2D(Collider2D collisionTarget)
    {
        GameObject hitObject = collisionTarget.gameObject;
        if (hitObject.CompareTag("Guard"))
                SetNewGoalFrom(_previousTile);
    }

    private void DecreaseXYTimers()
    {
        if(xTimer > 0) xTimer -= 0.1f;
        if(yTimer > 0) yTimer -= 0.1f;
    }

    public void SetNewGoalFrom(Tile tile)
    {
        if(tile.Id == _goalTile.Id)
        {
            // if the tile is the goal, set a new goal
            List<Tile> choices = Helper.FilterList(tile.Neighbors, tile => tile.IsFloor());
            _previousTile = tile;
            _goalTile = choices[Helper.GenerateRandomNumber(0, choices.Count - 1)];
            targetTile = _goalTile.Id;// for debug
        }
    }

    private int ComparePosition(float current, float destination, string type)
    {
        if (type == "x")
            xTimer = 3;
        if(type == "y")
            yTimer = 3;

        if (current > destination) 
            return -1;
        if (current < destination)
            return 1;
        return 0;
    }
}
