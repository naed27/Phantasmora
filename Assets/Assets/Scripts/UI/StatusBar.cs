using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{

    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _fill;

    public float Value { get { return _slider.value; } }

    public void SetMaxValue(float value)
    {
        _slider.maxValue = value;
        _slider.value = value;
    }


    public void SetValue(float value) { _slider.value = value; }
    public void DecreaseValueBy(float value) { if(_slider.value > 0) _slider.value -= value; }
    public void IncreaseValueBy(float value) { if (_slider.value < _slider.maxValue) _slider.value += value; }
    public bool IsEmpty() => _slider.value == 0;
    public bool IsFull() => _slider.value == _slider.maxValue;
    public void SetColor(Color color) => _fill.GetComponent<Image>().color = color;



}
