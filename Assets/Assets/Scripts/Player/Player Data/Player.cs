using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Main Props
    [SerializeField] private ViewsManager _viewsManager;


    // --------------- Movement
    [SerializeField] private float _moveSpeed;
    private Rigidbody2D _rb;
    private float _scaleX = 1;
    private Vector2 _movement;
    private Animator _animator;
    private float _originalMoveSpeed;

    // --------------- Actions
    [SerializeField] private KeyCode PowerViewKey = KeyCode.E;

    // --------------- Status

    //private int health;
    [SerializeField] private StatusBar _powerDurationBar;
    [SerializeField] private float _powerDurationInSeconds = 10;
    private bool _isUsingPowerView = false;
    private bool _isPowerAvailable = true;


    // --------------- Setters and Getters
    public bool IsUsingPowerView => _isUsingPowerView;
    public bool IsPowerAvailable => _isPowerAvailable;


    // --------------- Main

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _scaleX = transform.localScale.x;
        _originalMoveSpeed = _moveSpeed;
        _powerDurationBar.SetMaxValue(_powerDurationInSeconds);
    }

    void Update()
    {
        ListenForMovement();
        ListenForPowerAction();
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveSpeed * Time.fixedDeltaTime * _movement.normalized);
    }


    // --------------- Functions

    public void TurnOnPowerView() { if (!_isUsingPowerView) _isUsingPowerView = true; }
    public void TurnOffPowerView() { if (_isUsingPowerView) _isUsingPowerView = false; }

    private void ListenForMovement()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
        _animator.SetFloat("Speed", _movement.sqrMagnitude);

        if (_movement.x != 0)
        {
            // Flip Animation
            Vector3 scale = transform.localScale;
            scale.x = _scaleX * (_movement.x >= 0 ? 1 : -1);
            transform.localScale = scale;
        }
    }
    private void ListenForPowerAction()
    {
        if (Input.GetKey(PowerViewKey) && _isPowerAvailable)
        {
            _viewsManager.ActivatePowerView();
        }
        else
        {
            _viewsManager.DeativatePowerView();
        }

        if (_powerDurationBar.IsFull()) TurnOnPower();
        if (_powerDurationBar.IsEmpty()) ShutdownPower();

    }

    private void TurnOnPower() {
        _isPowerAvailable = true;
        _powerDurationBar.SetColor(Helper.ConvertColor(40, 197, 185, 255));
    }
    private void ShutdownPower() {
        _isPowerAvailable = false;
        _powerDurationBar.SetColor(Helper.ConvertColor(67, 103, 118, 255));
    }

    public void SlowDown() { _moveSpeed = _originalMoveSpeed / 2; }

    public void NormalizeSpeed() { if(_moveSpeed!=_originalMoveSpeed) _moveSpeed = _originalMoveSpeed; }

    public void SpeedUp() { _moveSpeed = _originalMoveSpeed * 2; }

    public void ConsumePowerDuration() { _powerDurationBar.DecreaseValueBy(1f* Time.deltaTime); }
    public void ReplenishPowerDuration() { _powerDurationBar.IncreaseValueBy(0.5f * Time.deltaTime); }

}
