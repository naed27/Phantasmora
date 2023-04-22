using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ---------------- Global Prefabs

    [SerializeField] private Player _playerPrefab;
    [SerializeField] private ViewsManager _viewsManagerPrefab;
    [SerializeField] private DungeonManager _dungeonManagerPrefab;

    // ---------------- Global Objects

    private Player _player;
    private ViewsManager _viewsManager;
    private DungeonManager _dungeonManager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
