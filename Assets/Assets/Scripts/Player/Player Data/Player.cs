using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    // --------------- UI 
    [SerializeField] private GameObject _gameResultMenu;


    // --------------- Manager Props
    [SerializeField] private MenuManager _menuManager;
    [SerializeField] private ViewsManager _viewsManager;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private ControlsManager _controlsManager;


    // --------------- Movement
    [SerializeField] private float _moveSpeed;
    private Rigidbody2D _rb;
    private float _scaleX = 1;
    private Vector2 _movement;
    private Animator _animator;
    private float _originalMoveSpeed;

    // --------------- Keybinds
    private KeyCode _pauseKey;
    private KeyCode _interactKey;
    private KeyCode _usePowerKey;
    private KeyCode _toggleNextPowerKey;

    // --------------- Power Status

    [SerializeField] private StatusBar _powerDurationBar;
    [SerializeField] private float _powerDurationInSeconds = 10;
    private bool _isUsingMeld;
    private bool _isUsingClairvoyance;
    private bool _isOnClairvoyanceView;
    private bool _isPowerAvailable;
    private List<DungeonKey> _visibleKeys;

    // --------------- Status
    [SerializeField] private Alert _keyAlert;
    [SerializeField] private GameObject _heat;
    [SerializeField] private KeyInventory _keyInventory;
    [SerializeField] private GameObject _statusBarPowerName;


    private TextMeshProUGUI _powerName;

    // --------------- Properties
    private string[] _powers = { "clairvoyance", "meld" };
    private int _powerIndex = 0;
    private SpriteRenderer _spriteRenderer;
    private bool _firstStart = true;


    // --------------- Presets
    [SerializeField] private float _meldVisionRange = 2.5f;

    // --------------- Setters and Getters
    public float MoveSpeed => _moveSpeed;
    public float MeldVisionRange => _meldVisionRange;
    public bool IsUsingMeld => _isUsingMeld;
    public bool IsUsingClairvoyance => _isUsingClairvoyance;
    public bool IsPowerAvailable => _isPowerAvailable;
    public KeyCode InteractKey => _interactKey;
    public KeyCode UsePowerKey => _usePowerKey;


    // --------------- Main

    void Start()
    {
        // ------- Setup 
        _scaleX = transform.localScale.x;
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = transform.GetComponent<SpriteRenderer>();
        _powerName = _statusBarPowerName.GetComponent<TextMeshProUGUI>();
        _originalMoveSpeed = _moveSpeed;

        SetupKeybinds();

        // ------- Start
        Init();
    }

    void Update()
    {
        ListenForSkill();
        ListenForPauseKey();
        ListenForMovement();
        ListenForToggleNextPower();
        Unmeld();
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _moveSpeed * Time.fixedDeltaTime * _movement.normalized);
    }

    private void OnTriggerEnter2D(Collider2D collisionTarget)
    {
        GameObject hitObject = collisionTarget.gameObject;
        if (hitObject.CompareTag("Danger Zone"))
        {
            _menuManager.Gameover();
            _soundManager.StopAllAudio();
            _soundManager.PlayPrimarySound("death");
        }
    }


    // --------------- Functions


    public void Init()
    {
        _powerIndex = 0;
        _isPowerAvailable = true;
        _isUsingClairvoyance = false;
        _moveSpeed = _originalMoveSpeed;
        _powerDurationBar.SetMaxValue(_powerDurationInSeconds);
        _visibleKeys = new List<DungeonKey>();
        _keyInventory.Init();
        _viewsManager.Init();
        _powerDurationBar.Init();
        _keyAlert.Init(IfThereAreKeysNearby); 
        UpdatePowerNameOnStatus();

        if (_firstStart)
        {
            _firstStart = false;
        }
        else
        {
            _soundManager.StopAllAudio();
        }
    }


    public void SetupKeybinds()
    {
        _pauseKey = _controlsManager.Pause;
        _interactKey = _controlsManager.Interact;
        _usePowerKey = _controlsManager.UsePower;
        _toggleNextPowerKey = _controlsManager.ToggleNextPower;
    }

    public void TurnOnMeld() { if (!_isUsingMeld) { TurnOffClairevoyance(); _isUsingMeld = true; _soundManager.PlaySecondarySound("meldEffect"); } }
    public void TurnOffMeld() { if (_isUsingMeld) _isUsingMeld = false; }
    public void SetClairvoyanceViewStatus(bool state) { 
        if(_isOnClairvoyanceView != state) {
            _isOnClairvoyanceView = state;
            _soundManager.PlaySecondarySound("clairvoyanceEffect");
        }
    }
    public void TurnOnClairvoyance() { if (!_isUsingClairvoyance) TurnOffMeld(); _isUsingClairvoyance = true; }
    public void TurnOffClairevoyance() { if (_isUsingClairvoyance) _isUsingClairvoyance = false; _isOnClairvoyanceView = false; }
    public void StopCastAnimation() { if (_animator.GetBool("IsCasting")) _animator.SetBool("IsCasting", false); }
    public void StartCastAnimation() { if (!_animator.GetBool("IsCasting")) _animator.SetBool("IsCasting", true); }
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
        if (Time.timeScale == 0) return;

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

    private void ListenForSkill()
    {
        if (Input.GetKey(_usePowerKey) && _isPowerAvailable)
        {
            _viewsManager.ActivateSkill(GetCurrentPower());
        }
        else
        {
            StopCastAnimation();
            _soundManager.StopSecondaryAudio();
            _viewsManager.DeactivateSkill();
        }

        if (_powerDurationBar.IsFull()) TurnOnPower();
        if (_powerDurationBar.IsEmpty()) ShutdownPower();
    }
  
    public void Meld()
    {
        const float goal = 0.4f;
        const float fadeSpeed = 5f;

        Color currentColor = _spriteRenderer.material.color;
        if (currentColor.a != goal) {
            float alphaValue = Mathf.Lerp(currentColor.a, goal, (fadeSpeed * Time.deltaTime));
            _spriteRenderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, alphaValue);
        }
    }
    public void Unmeld()
    {
        if (_isUsingMeld) return;

        const float goal = 1f;
        const float fadeSpeed = 5f;

        Color currentColor = _spriteRenderer.material.color;
        if (currentColor.a != goal)
        {
            float alphaValue = Mathf.Lerp(currentColor.a, goal, (fadeSpeed * Time.deltaTime));
            _spriteRenderer.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, alphaValue);
        }
    }

    private void ListenForPauseKey()
    {
        if (Input.GetKeyDown(_pauseKey)) _menuManager.PauseGame();
    }

    private void ListenForToggleNextPower()
    {
        if (Input.GetKeyDown(_toggleNextPowerKey))
        {
            ToggleNextPower();
            _soundManager.PlayTertiarySound("skillSwitch");
        }
    }

    private void TurnOnPower() {
        if (!_isPowerAvailable)
        {
            _isPowerAvailable = true;
            _powerDurationBar.SetColor(Helper.ConvertColor(40, 197, 185, 255));
            _powerDurationBar.HideLock();
            _soundManager.PlayTertiarySound("replenished");
        }
    }
    private void ShutdownPower() {
        if (_isPowerAvailable)
        {
            _isPowerAvailable = false;
            _powerDurationBar.SetColor(Helper.ConvertColor(67, 103, 118, 255));
            _powerDurationBar.DisplayLock();
            _soundManager.PlayTertiarySound("locked");
        }
    }

    public void SlowDown() { _moveSpeed = _originalMoveSpeed / 2; }

    public void NormalizeSpeed() { if (_moveSpeed != _originalMoveSpeed) _moveSpeed = _originalMoveSpeed; }

    public void SpeedUp() { _moveSpeed = _originalMoveSpeed * 2; }

    public void ConsumeClairvoyanceDuration() { _powerDurationBar.DecreaseValueBy(1f * Time.deltaTime); }
    public void ConsumeMeldDuration() { _powerDurationBar.DecreaseValueBy(1.5f * Time.deltaTime); }
    public void ReplenishPowerDuration() { _powerDurationBar.IncreaseValueBy(0.5f * Time.deltaTime); }

    public void AddKeyToInventory(DungeonKey key){
        _keyInventory.AddKey();
        StopsSeeingAKey(key);
        Destroy(key.transform.gameObject);
        _soundManager.PlayPrimarySound("item_pickup");
    }

    public void UnlockDoor()
    {
        if(_keyInventory.IsComplete())
        {
            _menuManager.Victory();
            _soundManager.PlayPrimarySound("unlock_door");
        }
    }

    public bool IsStatusBarVisible() => _powerDurationBar.transform.gameObject.activeInHierarchy;

    public void ShowStatusBar()
    {
        _keyInventory.transform.gameObject.SetActive(true);
        _powerDurationBar.transform.gameObject.SetActive(true);
    }

    public void HideStatusUI()
    {
        _keyInventory.transform.gameObject.SetActive(false);
        _powerDurationBar.transform.gameObject.SetActive(false);
    }

    private string GetCurrentPower() => _powers[_powerIndex];

    public void ToggleNextPower()
    {
        if (_powerIndex < _powers.Length - 1)
        {
            _powerIndex++;
        }
        else
        {
            _powerIndex = 0;
        }
        UpdatePowerNameOnStatus();

    }

    private void UpdatePowerNameOnStatus() => _powerName.text = $"◄ {GetCurrentPower()} ►";


    // ------------ Sound Functions


}
