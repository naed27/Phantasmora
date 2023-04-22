using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewsManager : MonoBehaviour
{

    // ----------------- control keys

    [SerializeField] private KeyCode enlargeKey = KeyCode.E;

    // ----------------- properties

    [SerializeField] private GameObject _playerObject;
    [SerializeField] private FieldView _fieldViewPrefab;
    [SerializeField] private PowerView _powerViewPrefab;


    [SerializeField] private LayerMask _fieldViewMaskToCollideWith;
    [SerializeField] private LayerMask _powerViewMaskToCollideWith;

    private FieldView _fieldViewObject;
    private PowerView _powerViewObject;

    void Start()
    {
        _fieldViewObject = Instantiate(_fieldViewPrefab);
        _powerViewObject = Instantiate(_powerViewPrefab);

        _fieldViewObject.name = "Field View";
        _powerViewObject.name = "Power View";

        _fieldViewObject.transform.SetParent(transform);
        _powerViewObject.transform.SetParent(transform);

        _powerViewObject.Init(_playerObject, _powerViewMaskToCollideWith);
        _fieldViewObject.Init(_playerObject, _fieldViewMaskToCollideWith);
    }

    void Update()
    {
        if (Input.GetKey(enlargeKey))
        {
            if (_fieldViewObject.RayLength > 0)
            {
                _fieldViewObject.Shrink();
            }
            else
            {
                _powerViewObject.Enlarge();
            }
        }
        else
        {
            if (_powerViewObject.RayLength > 0)
            {
                _powerViewObject.Shrink();
            }
            else
            {
                _fieldViewObject.Enlarge();
            }
        }
    }
}
