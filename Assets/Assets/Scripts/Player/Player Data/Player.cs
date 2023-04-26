using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // --------------- UI 
    [SerializeField] private GameObject _gameResultMenu;


    // --------------- Main Props
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private ViewsManager _viewsManager;
    [SerializeField] private GameObject _heat;


    // --------------- Movement
    [SerializeField] private float _moveSpeed;
    private Rigidbody2D _rb;
    private float _scaleX = 1;
    private Vector2 _movement;
    private Animator _animator;
    private float _originalMoveSpeed;

    // --------------- Actions
    [SerializeField] private KeyCode _pauseKey = KeyCode.Escape;
    [SerializeField] private KeyCode _interactKey = KeyCode.F;
    [SerializeField] private KeyCode _powerViewKey = KeyCode.E;

    // --------------- Power Status

    [SerializeField] private StatusBar _powerDurationBar;
    [SerializeField] private float _powerDurationInSeconds = 10;
    private bool _isUsingPowerView;
    private bool _isPowerAvailable;
    private List<DungeonKey> _visibleKeys;

    // --------------- Status




    // --------------- Power Status

    [SerializeField] private KeyInventory _keyInventory;



    // --------------- Setters and Getters
    public bool IsUsingPowerView => _isUsingPowerView;
    public bool IsPowerAvailable => _isPowerAvailable;
    public KeyCode InteractKey => _interactKey;
    public KeyCode PowerViewKey => _powerViewKey;


    // --------------- Main

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _scaleX = transform.localScale.x;
        Init();
    }

    void Update()
    {
        ListenForMovement();
        ListenForPowerKey();
        ListenForPauseKey();
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveSpeed * Time.fixedDeltaTime * _movement.normalized);
    }

    private void OnTriggerEnter2D(Collider2D collisionTarget)
    {
        GameObject hitObject = collisionTarget.gameObject;
        if (hitObject.CompareTag("Danger Zone"))
            _gameManager.Gameover();
    }


    // --------------- Functions

    public void Init()
    {
        _isUsingPowerView = false;
        _isPowerAvailable = true;
        _originalMoveSpeed = _moveSpeed;
        _powerDurationBar.SetMaxValue(_powerDurationInSeconds);
        _visibleKeys = new List<DungeonKey>();
        _keyInventory.Init();
    }

    public void TurnOnPowerView() { if (!_isUsingPowerView) _isUsingPowerView = true; }
    public void TurnOffPowerView() { if (_isUsingPowerView) _isUsingPowerView = false; }
    public void CastPower() => _animator.SetBool("IsCasting", true);
    public void StopCasting() => _animator.SetBool("IsCasting", false);
    public void ShowHeat() => _heat.SetActive(true);
    public void HideHeat() => _heat.SetActive(false);
    public void SeesAKey(DungeonKey seenKey) {
        if (Helper.FilterList(_visibleKeys, key => key.Id == seenKey.Id).Count == 0)
            _visibleKeys.Add(seenKey);
    }

    public void StopsSeeingAKey(DungeonKey seenKey)
    {
        if (Helper.FilterList(_visibleKeys, key => key.Id == seenKey.Id).Count > 0)
            _visibleKeys.Remove(seenKey);
    }

    public bool IfThereAreKeysNearby() => _visibleKeys.Count > 0;

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
    private void ListenForPowerKey()
    {
        if (Input.GetKey(_powerViewKey) && _isPowerAvailable)
        {
            _viewsManager.ActivatePowerView();
        }
        else
        {
            StopCasting();
            _viewsManager.DeativatePowerView();
        }

        if (_powerDurationBar.IsFull()) TurnOnPower();
        if (_powerDurationBar.IsEmpty()) ShutdownPower();
    }

    private void ListenForPauseKey()
    {
        if (Input.GetKey(_pauseKey)) _gameManager.PauseGame();
    }



    private void TurnOnPower() {
        _isPowerAvailable = true;
        _powerDurationBar.SetColor(Helper.ConvertColor(40, 197, 185, 255));
        _powerDurationBar.HideLock();
    }
    private void ShutdownPower() {
        _isPowerAvailable = false;
        _powerDurationBar.SetColor(Helper.ConvertColor(67, 103, 118, 255));
        _powerDurationBar.DisplayLock();
    }

    public void SlowDown() { _moveSpeed = _originalMoveSpeed / 2; }

    public void NormalizeSpeed() { if (_moveSpeed != _originalMoveSpeed) _moveSpeed = _originalMoveSpeed; }

    public void SpeedUp() { _moveSpeed = _originalMoveSpeed * 2; }

    public void ConsumePowerDuration() { _powerDurationBar.DecreaseValueBy(1f * Time.deltaTime); }
    public void ReplenishPowerDuration() { _powerDurationBar.IncreaseValueBy(0.5f * Time.deltaTime); }

    public void AddKeyToInventory(DungeonKey key){
        _keyInventory.AddKey();
        StopsSeeingAKey(key);
        Destroy(key.transform.gameObject);
    }

    public void UnlockDoor()
    {
        if(_keyInventory.IsComplete())
        {
            _gameResultMenu.SetActive(true);
        }
    }

    public bool IsStatusBarVisible() => _powerDurationBar.transform.gameObject.activeInHierarchy;

    public void ShowStatusBar()
    {
        _keyInventory.transform.gameObject.SetActive(true);
        _powerDurationBar.transform.gameObject.SetActive(true);
    }

    public void HideStatusBar()
    {
        _keyInventory.transform.gameObject.SetActive(false);
        _powerDurationBar.transform.gameObject.SetActive(false);
    }

}
