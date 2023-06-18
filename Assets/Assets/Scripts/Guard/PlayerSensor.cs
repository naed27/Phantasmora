using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : MonoBehaviour
{

    private bool _playerIsClose = false;
    public bool PlayerIsClose => _playerIsClose;

    private void OnTriggerStay2D(Collider2D collisionTarget)
    {
        GameObject hitObject = collisionTarget.gameObject;
        if (hitObject.TryGetComponent(out Player player))
        {
            if (!player.IsUsingMeld && _playerIsClose == false)
                _playerIsClose = true;
            else if (player.IsUsingMeld && _playerIsClose == true)
                _playerIsClose = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collisionTarget)
    {
        GameObject hitObject = collisionTarget.gameObject;
        if (hitObject.layer == LayerMask.NameToLayer("Player"))
            _playerIsClose = false;

    }


}
