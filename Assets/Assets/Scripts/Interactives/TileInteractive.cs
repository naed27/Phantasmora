using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteractive : MonoBehaviour
{

    private Player _player;
    private bool _playerIsClose;
    private KeyCode _interactKey;
    private Tile _tile;

    public void Init(Tile tile, Player player, KeyCode key)
    {
        _tile = tile;
        _player = player;
        _interactKey = key;
    }

    private void Update()
    {
        if (Input.GetKey(_interactKey) && _playerIsClose && _tile.IsGoalPoint())
            _player.UnlockDoor();
    }

    private void OnTriggerEnter2D(Collider2D collisionTarget)
    {
        GameObject hitObject = collisionTarget.gameObject;
        if (hitObject.layer == LayerMask.NameToLayer("Player"))
            _playerIsClose = true;
    }

    private void OnTriggerExit2D(Collider2D collisionTarget)
    {
        GameObject hitObject = collisionTarget.gameObject;
        if (hitObject.layer == LayerMask.NameToLayer("Player"))
            _playerIsClose = false;

    }
}
