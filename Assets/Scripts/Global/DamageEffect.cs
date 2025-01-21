using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public float blinkDuration = 0.4f;
    public int blinkCount = 2;
    public float blinkInterval = 0.2f;
    public Color blinkColor = Color.red;

    void Start()
    {

    }

    public void Init(SpriteRenderer sprite)
    {
        spriteRenderer = sprite;
        originalColor = spriteRenderer.color;
        
    }


    public void ApplyDamageBlink()
    {
        if (spriteRenderer != null)
        {
            StartCoroutine(BlinkDamageEffect());
        }
    }

    public void StopApply()
    {
        StopCoroutine(BlinkDamageEffect());
    }


    private IEnumerator BlinkDamageEffect()
    {
        float timer = 0f;
        int blinkTimes = 0;


        while (blinkTimes < blinkCount)
        {

            spriteRenderer.color = (timer % blinkInterval < blinkInterval / 2f) ? blinkColor : originalColor;


            timer += Time.deltaTime;


            if (timer >= blinkInterval)
            {
                timer = 0f;
                blinkTimes++;
            }


            yield return null;
        }

        spriteRenderer.color = originalColor;
    }
}
