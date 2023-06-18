using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlsManager : MonoBehaviour
{

    // ------------- Game Keybinds
    [SerializeField] private KeyCode _pause = KeyCode.Escape;

    // ------------- Player Keybinds
    [SerializeField] private KeyCode _interact = KeyCode.F;
    [SerializeField] private KeyCode _usePower = KeyCode.Q;
    [SerializeField] private KeyCode _toggleNextPower = KeyCode.T;

    // ------------- Guide Menus
    [SerializeField] private GameObject _pauseGuideText;
    [SerializeField] private GameObject _mainMenuGuideText;

    private TextMeshProUGUI _pauseTMP;
    private TextMeshProUGUI _mainMenuTMP;

    // ------------- Setters and Getters
    public KeyCode Pause { get { return _pause; } set { _pause = value; } }
    public KeyCode Interact { get { return _interact; } set { _interact = value; } }
    public KeyCode UsePower { get { return _usePower; } set { _usePower = value; } }
    public KeyCode ToggleNextPower { get { return _toggleNextPower; } set { _toggleNextPower = value; } }

    private void Start()
    {
        string guideText = $"Controls:\n\n{_interact} - Interact\n{_usePower} - Use Skill\n{_toggleNextPower} - Switch Skill\nWASD - Move";

        _pauseTMP = _pauseGuideText.GetComponent<TextMeshProUGUI>();
        _mainMenuTMP = _mainMenuGuideText.GetComponent<TextMeshProUGUI>();

        _pauseTMP.text = guideText;
        _mainMenuTMP.text = guideText;
    }

}
