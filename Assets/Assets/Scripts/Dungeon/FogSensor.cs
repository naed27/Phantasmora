using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogSensor : MonoBehaviour
{
    private LayerMask mask;
    private SpriteRenderer fog;
    private string _mode = "null";

    private bool isWall = false;
    private bool isGround = false;

    readonly private float _lowestFogIntensity = 0f; //     <----- 0% foggy
    readonly private float _highestFogIntensity = 0.95f; //  <---- 80% foggy

    private void Start()
    {
        fog = transform.GetComponent<SpriteRenderer>();

        mask = transform.parent.gameObject.layer;
        isWall = mask == LayerMask.NameToLayer("Wall");
        isGround = mask == LayerMask.NameToLayer("Ground");

    }

    private void Update()
    {
        if (_mode == "in") Fade(_lowestFogIntensity);
        if (_mode == "out") Fade(_highestFogIntensity);
    }

    private void Fade(float goal)
    {
        float fadeSpeed = isWall ? 5f : 20f;
        Color currentColor = fog.color;
        if (currentColor.a == goal) { StopFading(); return; }
        float alphaValue = Mathf.Lerp(currentColor.a, goal, (fadeSpeed * Time.deltaTime));
        fog.color = new Color(currentColor.r, currentColor.g, currentColor.b, alphaValue);
    }

    private void OnTriggerEnter2D(Collider2D collisionTarget)
    {
        if (collisionTarget.gameObject.CompareTag("Player"))
            if (isWall)
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
