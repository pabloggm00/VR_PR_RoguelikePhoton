using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSprites",menuName = "Player Data Sprites")]
public class PlayerSprites : ScriptableObject
{

    public List<ElementSprite> elementSprites;

    public void AgregarSprites(List<ElementSprite> playerSprites)
    {
        foreach (ElementSprite element in elementSprites)
        {
            playerSprites.Add(element);
        }
    }
}
