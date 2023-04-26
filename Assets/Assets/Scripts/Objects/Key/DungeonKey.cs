using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DungeonKey : MonoBehaviour
{
    private string _id;
    private bool _playerIsClose = false;
    private Player _player;
    private KeyCode _pickupObjectKey;

    // ----------- Setters and Getters

    public string Id => _id;

    private void Update()
    {
        if (Input.GetKey(_pickupObjectKey) && _playerIsClose)
            _player.AddKeyToInventory(this);
    }

    public void Init(int index, Tile tile, DungeonManager dungeonManager)
    {

        // Set Identity
        _id = "Key_" + index.ToString() + tile.Id;

        // Set Position
        transform.position = tile.Position;
        transform.parent = dungeonManager.transform;
        _player = dungeonManager.Player;
        _pickupObjectKey = _player.InteractKey;
    }

    private void OnTriggerEnter2D(Collider2D collisionTarget)
    {
        GameObject hitObject = collisionTarget.gameObject;
        if (hitObject.layer == LayerMask.NameToLayer("Player"))
            if (hitObject.TryGetComponent(out Player player))
            {
                player.SeesAKey(this);
                _playerIsClose = true;
            }
    }

    private void OnTriggerExit2D(Collider2D collisionTarget)
    {
        GameObject hitObject = collisionTarget.gameObject;
        if (hitObject.layer == LayerMask.NameToLayer("Player"))
            if (hitObject.TryGetComponent(out Player player))
            {
                player.StopsSeeingAKey(this);
                _playerIsClose = false;
            }
    }

}
