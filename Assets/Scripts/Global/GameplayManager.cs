using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;

    public RoomGenerator roomGenerator;
    public PhotonView player;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        InitWorld();
    }

    void InitWorld()
    {
        // roomGenerator.GenerateRoom();
        roomGenerator.InitGame(player);
    }

    public float GetMultiplierDamage(ElementSprite myElement, ElementType enemyElement)
    {
        if (enemyElement == myElement.debilidad)
        {
            return 2.0f;
        }
        else if (enemyElement == myElement.resistente)
        {
            return 0.5f;
        }

        return 1.0f;


    }
}
