using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "Crear Elemento Player")]
public class ElementSprite : ScriptableObject
{
    public Sprite sprite;
    public Sprite spriteMuerto;
    public ElementType elementType;
    public ElementType debilidad;
    public ElementType resistente;


    public float GetMultiplierDamage(ElementType enemyElement)
    {
        if (enemyElement == debilidad)
        {
            return 2.0f;
        }
        else if (enemyElement == resistente)
        {
            return 0.5f;
        }

        return 1.0f;


    }

}
