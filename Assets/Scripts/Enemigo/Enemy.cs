using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hP;
    public ElementType element;
    public SpriteRenderer spriteRenderer;

    public float currentHP;

    private DamageEffect damageEffect;

    private void Start()
    {
        currentHP = hP;
        spriteRenderer.color = ElementsInteractions.GetElementColor(element);

        damageEffect = GetComponent<DamageEffect>();
        damageEffect.Init(spriteRenderer);
    }


    public void TakeDamage(int dmg, ElementType bulletElement)
    {
        float elementDamage = dmg * ElementsInteractions.GetDamageMultiplier(bulletElement, element);

        currentHP -= elementDamage;

        damageEffect.ApplyDamageBlink();

        if (currentHP <= 0)
        {
            currentHP = 0; //Muere
        }
    }

}
