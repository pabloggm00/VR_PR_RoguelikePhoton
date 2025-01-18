using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public PlayerController controller;
    public PlayerShoot playerShoot;
    public PlayerHealth playerHealth;
    public PlayerMove playerMove;

    public string playerName;
    public TMP_Text nicknameText;

    public void IsLocalPlayer()
    {
        controller.enabled = true;
        playerShoot.enabled = true;
        playerHealth.enabled = true;
        playerMove.enabled = true;


        Rigidbody2D rb = this.AddComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.gravityScale = 0;

    }

    [PunRPC]
    public void SetNickname(string _name)
    {
        playerName = _name;

        nicknameText.text = _name;
    }
}
