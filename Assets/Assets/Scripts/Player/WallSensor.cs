
using System.Collections;
using UnityEngine;

public class WallSensor : MonoBehaviour
{


    private const float FADE_DURATION = 0.5f;
    private Coroutine fadeCoroutine;
    private SpriteRenderer lastHitSpriteRenderer;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall Unseen"))
        {

            other.gameObject.layer = LayerMask.NameToLayer("Wall Seen");

            lastHitSpriteRenderer = other.GetComponent<SpriteRenderer>();
            if (lastHitSpriteRenderer != null && fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine); // Stop any existing fade coroutine on the previous hit object.
                fadeCoroutine = StartCoroutine(FadeIn(lastHitSpriteRenderer));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall Seen"))
        {
            other.gameObject.layer = LayerMask.NameToLayer("Wall Unseen");

            SpriteRenderer spriteRenderer = other.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                fadeCoroutine = StartCoroutine(FadeOut(spriteRenderer));
            }

        }
    }
    private IEnumerator FadeIn(SpriteRenderer spriteRenderer)
    {
        float timer = 0.0f;
        while (timer < FADE_DURATION)
        {
            float opacity = Mathf.Lerp(0.0f, 1.0f, timer / FADE_DURATION);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, opacity);
            timer += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1.0f);
    }

    private IEnumerator FadeOut(SpriteRenderer spriteRenderer)
    {
        float timer = 0.0f;
        while (timer < FADE_DURATION)
        {
            float opacity = Mathf.Lerp(1.0f, 0.0f, timer / FADE_DURATION);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, opacity);
            timer += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.0f);
    }

}