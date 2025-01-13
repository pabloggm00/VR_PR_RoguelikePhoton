using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSprites",menuName = "Player Data Sprites")]
public class PlayerSprites : ScriptableObject
{

    public List<Sprite> sprites;

    public void AgregarSprites(List<Sprite> playerSprites)
    {
        foreach (Sprite sprite in sprites)
        {
            playerSprites.Add(sprite);
        }
    }
}
