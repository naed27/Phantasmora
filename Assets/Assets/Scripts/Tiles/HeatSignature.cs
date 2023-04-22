using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSignature : MonoBehaviour
{

    [SerializeField] private Material _wallHeatMaterial;
    [SerializeField] private Material _floorHeatMaterial;

    private int _layer;
    private SpriteRenderer _spriteRenderer;

    public void Init(Sprite sprite)
    {
        _layer = transform.parent.gameObject.layer;
        _spriteRenderer = transform.GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = sprite;

        if (_layer == LayerMask.NameToLayer("Wall"))
            _spriteRenderer.material = _wallHeatMaterial;

        if (_layer == LayerMask.NameToLayer("Floor"))
            _spriteRenderer.material = _floorHeatMaterial;
    }

}
