using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    private int _layer;
    private Player _player;
    private SpriteRenderer _spriteRenderer;

    private string _mode = "null";
    private bool _isWall = false;
    private bool _isFloor = false;

    readonly private float _lowestFogIntensity = 0f; //     <----- 0% foggy
    readonly private float _highestFogIntensity = 0.80f; //  <---- 80% foggy

    public void Init(Player player)
    {
        _player = player;
        _layer = transform.parent.gameObject.layer;
        _isWall = _layer == LayerMask.NameToLayer("Wall");
        _isFloor = _layer == LayerMask.NameToLayer("Floor");
        _spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_player.IsUsingPowerView && _mode == "in") { DoFadeOut(); }
        if (_mode == "in") Fade(_lowestFogIntensity);
        if (_mode == "out") Fade(_highestFogIntensity);
    }

    private void Fade(float goal)
    {
        float fadeSpeed = _isWall ? 10f : 20f;
        Color currentColor = _spriteRenderer.color;
        if (currentColor.a == goal) { StopFading(); return; }
        float alphaValue = Mathf.Lerp(currentColor.a, goal, (fadeSpeed * Time.deltaTime));
        _spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, alphaValue);
    }

    private void OnTriggerStay2D(Collider2D collisionTarget)
    {
        GameObject hitObject = collisionTarget.gameObject;
        if (hitObject.CompareTag("Player"))
            if (collisionTarget.gameObject.TryGetComponent(out Player player))
                if (_isWall && !player.IsUsingPowerView)
                    DoFadeIn();
    }



    private void OnTriggerExit2D(Collider2D collisionTarget)
    {
        if (collisionTarget.gameObject.CompareTag("Player"))
            DoFadeOut();
    }




    // -------------------- Setter Functions

    public void DoFadeIn() { if(_mode != "in") _mode = "in" ; }

    public void DoFadeOut() { if(_mode != "out") _mode = "out"; }

    public void StopFading() { if(_mode != "null") _mode = "null"; }

}
