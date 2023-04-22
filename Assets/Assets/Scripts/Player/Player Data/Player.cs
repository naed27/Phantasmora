using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private bool _isUsingPowerView = false;
    public bool IsUsingPowerView { get { return _isUsingPowerView; } }

    // --------------- Setters and Getters


    public void TurnOnPowerView() { if (!_isUsingPowerView) _isUsingPowerView = true; }
    public void TurnOffPowerView() { if (_isUsingPowerView) _isUsingPowerView = false; }

}
