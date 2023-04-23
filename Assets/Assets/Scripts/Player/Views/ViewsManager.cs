using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewsManager : MonoBehaviour
{
  
    // ----------------- properties

    [SerializeField] private Player _player;
    [SerializeField] private MeshField _fieldViewPrefab;
    [SerializeField] private MeshField _powerViewPrefab;

    [SerializeField] private LayerMask _fieldViewMaskToCollideWith;
    [SerializeField] private LayerMask _powerViewMaskToCollideWith;

    private MeshField _fieldView;
    private MeshField _powerView;

    private bool _isUsingPowerView = false;

    void Start()
    {
        _fieldView = Instantiate(_fieldViewPrefab);
        _powerView = Instantiate(_powerViewPrefab);

        _fieldView.name = "Field View";
        _powerView.name = "Power View";

        _fieldView.transform.SetParent(transform);
        _powerView.transform.SetParent(transform);

        _powerView.Init(_player, _powerViewMaskToCollideWith);
        _fieldView.Init(_player, _fieldViewMaskToCollideWith);
    }

    void Update()
    {
        if (_isUsingPowerView)
        {
            _player.TurnOnPowerView();

            if (_fieldView.RayLength > 0)
            {
                _fieldView.Shrink();
            }
            else
            {
                _player.SlowDown();
                _powerView.Enlarge(_powerView.MaximumRayLength);
                _player.ConsumePowerDuration();
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
                _player.NormalizeSpeed();
                _player.TurnOffPowerView();
                _fieldView.Enlarge(_fieldView.OriginalRayLength);
                _player.ReplenishPowerDuration();
            }
        }
    }

    public void ActivatePowerView() { _isUsingPowerView = true; }
    public void DeativatePowerView() { _isUsingPowerView = false; }
}
