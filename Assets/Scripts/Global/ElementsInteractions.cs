using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    Agua,
    Fuego,
    Hoja,
    Piedra
}

public class ElementsInteractions : MonoBehaviour
{

    private static readonly Dictionary<ElementType, Color> elementColors = new()
    {
        { ElementType.Fuego, Color.red },
        { ElementType.Hoja, Color.green },
        { ElementType.Agua, Color.blue },
        { ElementType.Piedra, Color.gray }
    };


    private static readonly Dictionary<ElementType, ElementType> weakAgainst = new()
    {
        { ElementType.Fuego, ElementType.Hoja },
        { ElementType.Hoja, ElementType.Agua },
        { ElementType.Agua, ElementType.Piedra },
        { ElementType.Piedra, ElementType.Fuego }
    };

    private static readonly Dictionary<ElementType, ElementType> strongAgainst = new()
    {
        { ElementType.Fuego, ElementType.Piedra },
        { ElementType.Hoja, ElementType.Fuego },
        { ElementType.Agua, ElementType.Hoja },
        { ElementType.Piedra, ElementType.Agua }
    };

    public static float GetDamageMultiplier(ElementType attacker, ElementType defender)
    {
        if (weakAgainst[attacker] == defender)
        {
            return 2.0f; // Eficaz
        }
        if (strongAgainst[attacker] == defender)
        {
            return 0.5f; // Resistente
        }

        return 1.0f;
    }

    public static Color GetElementColor(ElementType type)
    {
        return elementColors[type];
    }
}
