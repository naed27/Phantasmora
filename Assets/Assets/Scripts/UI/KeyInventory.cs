using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyInventory : MonoBehaviour
{
    [SerializeField] private GameObject _keyHolder1;
    [SerializeField] private GameObject _keyHolder2;
    [SerializeField] private GameObject _keyHolder3;
    [SerializeField] private GameObject _keyHolder4;
    [SerializeField] private GameObject _keysNotice;
    [SerializeField] private GameObject _noticeHighlight;

    private int _keyCount = 0;
    readonly private int _maxKeys = 4;

    // --------------- Getters

    public int KeyCount => _keyCount;

    public void Init()
    {
        _keyCount = 0;
        ResetColor(_keyHolder1);
        ResetColor(_keyHolder2);
        ResetColor(_keyHolder3);
        ResetColor(_keyHolder4);
        _keysNotice.SetActive(false);
        _noticeHighlight.SetActive(false);
    }

    public void AddKey()
    {
        if (_keyCount < _maxKeys)
        {
            if (_keyCount == 0) UpdateColor(_keyHolder1);
            if (_keyCount == 1) UpdateColor(_keyHolder2);
            if (_keyCount == 2) UpdateColor(_keyHolder3);
            if (_keyCount == 3) UpdateColor(_keyHolder4);
            _keyCount++;
        }

        if(_keyCount == _maxKeys)
        {
            _keysNotice.SetActive(true);
            _noticeHighlight.SetActive(true);
        }
        else
        {
            _keysNotice.SetActive(false);
            _noticeHighlight.SetActive(false);
        }
    }

   private void UpdateColor(GameObject holder)
   {
        Image image = holder.GetComponent<Image>();
        image.color = new Color(1, 1, 1, 1);
   }

    private void ResetColor(GameObject holder)
    {
        Image image = holder.GetComponent<Image>();
        image.color = new Color(0, 0, 0, 1);
    }

    public bool IsComplete() => _keyCount == _maxKeys;

  


}
