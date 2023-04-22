using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewsManager : MonoBehaviour
{

    // ----------------- control keys

    [SerializeField] private KeyCode enlargeKey = KeyCode.E;

    // ----------------- properties

    [SerializeField] private Player _playerObject;
    [SerializeField] private MeshField _fieldViewPrefab;
    [SerializeField] private MeshField _powerViewPrefab;

    [SerializeField] private LayerMask _fieldViewMaskToCollideWith;
    [SerializeField] private LayerMask _powerViewMaskToCollideWith;

    private MeshField _fieldView;
    private MeshField _powerView;

    void Start()
    {
        _fieldView = Instantiate(_fieldViewPrefab);
        _powerView = Instantiate(_powerViewPrefab);

        _fieldView.name = "Field View";
        _powerView.name = "Power View";

        _fieldView.transform.SetParent(transform);
        _powerView.transform.SetParent(transform);

        _powerView.Init(_playerObject, _powerViewMaskToCollideWith);
        _fieldView.Init(_playerObject, _fieldViewMaskToCollideWith);
    }

    void Update()
    {
        if (Input.GetKey(enlargeKey))
        {
            _playerObject.TurnOnPowerView();

            if (_fieldView.RayLength > 0)
            {
                _fieldView.Shrink();
            }
            else
            {
                _powerView.Enlarge(10);
            }
        }
        else
        {
            if (_powerView.RayLength > 0)
            {
                _powerView.Shrink();
            }
            else
            {
                _playerObject.TurnOffPowerView();
                _fieldView.Enlarge(_fieldView.OriginalRayLength);
            }
        }
    }
}
