using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyAlert : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int _fadeSpeed = 1000;
    private Color _originalColor;
    private string _direction = "up";

    private void Start()
    {
        _originalColor = spriteRenderer.material.color;
    }

    private void Update()
    {
        if(player.IfThereAreKeysNearby())
        {
            ListenForDirection();
            UpdateAlpha();
        }
        else
        {
            if (spriteRenderer.material.color.a != 0)
            {
                SetDirection("down");
                UpdateAlpha();
            }
            else
            {
                SetDirection("up");
            }
                
        }
    }

    private void ListenForDirection()
    {
        float alpha = spriteRenderer.material.color.a;
        if (alpha >= 1)
            _direction = "down";
        if (alpha <= 0)
            _direction = "up";
    }

    private void UpdateAlpha()
    {
        float alpha = spriteRenderer.material.color.a;
        float fadeSpeed = _direction == "up" ? _fadeSpeed : _fadeSpeed * -1;

        if((_direction == "up" && alpha < 1) || (_direction == "down" && alpha > 0))
        {
            alpha += (fadeSpeed * Time.deltaTime) / 255f;
            spriteRenderer.material.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, alpha);
        }
    }

    private void SetDirection(string direction) => _direction = direction; 
}
