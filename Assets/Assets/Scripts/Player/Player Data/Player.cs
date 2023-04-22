using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private bool _isUsingPowerView;

    // --------------- Setters and Getters

    public bool IsUsingPowerView { get { return _isUsingPowerView; } set { _isUsingPowerView = value; } }

}
