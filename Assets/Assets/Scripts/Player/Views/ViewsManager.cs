using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewsManager : MonoBehaviour
{
    // ----------------- managers

    [SerializeField] private SoundManager _soundManager;

    // ----------------- properties

    [SerializeField] private Player _player;
    [SerializeField] private MeshField _fieldView;
    [SerializeField] private MeshField _clairvoyanceView;

    [SerializeField] private LayerMask _fieldViewMaskToCollideWith;
    [SerializeField] private LayerMask _powerViewMaskToCollideWith;

    private string _currentlyUsedSkill = "";

    public void Init()
    {
        _fieldView.Init(_player, _fieldViewMaskToCollideWith, true);
        _clairvoyanceView.Init(_player, _powerViewMaskToCollideWith, false);
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        if (_currentlyUsedSkill == "clairvoyance")
        {
            _fieldView.DisallowBreathing();
            _player.TurnOnClairvoyance();

            if (_fieldView.RayLength > 0)
            {
                _player.StartCastAnimation();
                _fieldView.Shrink();
            }
            else
            {

                _player.ShowHeat();
                _player.SetClairvoyanceViewStatus(true);
                _player.StopCastAnimation();
                _player.SlowDown();
                _clairvoyanceView.transform.localScale = Vector3.one;
                _clairvoyanceView.Enlarge(_clairvoyanceView.MaximumRayLength);
                _player.ConsumeClairvoyanceDuration();
            }
        }
        else if (_currentlyUsedSkill == "meld")
        {
            _fieldView.DisallowBreathing();

            if (_clairvoyanceView.RayLength > 0)
            {
                _clairvoyanceView.Shrink();
            }
            else 
            {
                _fieldView.Resize(_player.MeldVisionRange);

                if (_fieldView.ReachedSpecificLength(_player.MeldVisionRange))
                    _player.TurnOnMeld();
            }
            
            if(_player.IsUsingMeld)
            {
                _player.HideHeat();
                _player.SlowDown();
                _player.Meld();
                _player.ConsumeClairvoyanceDuration();
            }
        }
        else
        {

            _player.TurnOffMeld();
            //_soundManager.StopSecondaryAudio();

            if (_clairvoyanceView.RayLength <= 0 )
            {

                _player.HideHeat();
                _player.NormalizeSpeed();
                _player.TurnOffClairevoyance();
                _player.ReplenishPowerDuration();
                _fieldView.Enlarge(_fieldView.OriginalRayLength);

                if (_clairvoyanceView.transform.localScale != Vector3.zero)
                    _clairvoyanceView.transform.localScale = Vector3.zero;

            }
            else
            {
                _clairvoyanceView.Shrink();
            }
        }
    }

    public void ActivateSkill(string skillName)
    {
        if (_player.IsStatusBarVisible()){
            if (_currentlyUsedSkill != skillName) _currentlyUsedSkill = skillName;
        }
    }

    public void DeactivateSkill()
    {
        if (_currentlyUsedSkill != "") _currentlyUsedSkill = "";
    }
}
