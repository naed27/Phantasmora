using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideInfo : MonoBehaviour
{
    [SerializeField] private TextMesh textMesh;
    [SerializeField] private ControlsManager controlsManager;

    private void Start()
    {
        textMesh = GetComponent<TextMesh>();
    }
}
