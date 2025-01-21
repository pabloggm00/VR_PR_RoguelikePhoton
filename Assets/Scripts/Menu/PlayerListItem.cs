using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [Header("UI Elements")]
    public TMP_Text playerNameText;
    public Image characterImage;
    public Button nextButton;
    public Button previousButton;

    [HideInInspector]
    public Photon.Realtime.Player player; 
    private PlayerSprites playerSprites; 
    private int selectedCharacterIndex = 0;

    private bool isInitialized = false; // Para evitar múltiples inicializaciones

    public void SetPlayer(Photon.Realtime.Player player)
    {
        this.player = player;
        playerNameText.text = player.NickName;
    }

    /*public void SetPlayer(Photon.Realtime.Player player, PlayerSprites sprites)
    {
        if (isInitialized) return; // Evita reinicializar

        this.player = player;
        this.playerSprites = sprites;

        playerNameText.text = player.NickName;

        // Si este jugador es el local, habilita los botones
        if (player.IsLocal)
        {
            nextButton.gameObject.SetActive(true);
            previousButton.gameObject.SetActive(true);
        }
        else
        {
            nextButton.gameObject.SetActive(false);
            previousButton.gameObject.SetActive(false);
        }

        // Recuperar el índice de carácter desde las propiedades personalizadas
        if (player.CustomProperties.TryGetValue("CharacterIndex", out object characterIndex))
        {
            selectedCharacterIndex = (int)characterIndex;
        }
        else
        {
            selectedCharacterIndex = 0; // Valor predeterminado
        }

        UpdateCharacterSprite();
        isInitialized = true; // Marca como inicializado
    }

    private void Start()
    {
        if (player.IsLocal) UpdateCharacterSelection(); // Solo actualiza si es el jugador local
    }


    public void NextCharacter()
    {
        selectedCharacterIndex = (selectedCharacterIndex + 1) % playerSprites.elementSprites.Count;
        UpdateCharacterSprite();
        UpdateCharacterSelection();
    }


    public void PreviousCharacter()
    {
        selectedCharacterIndex = (selectedCharacterIndex - 1 + playerSprites.elementSprites.Count) % playerSprites.elementSprites.Count;
        UpdateCharacterSprite();
        UpdateCharacterSelection();
    }


    private void UpdateCharacterSprite()
    {
        characterImage.sprite = playerSprites.elementSprites[selectedCharacterIndex].sprite;
    }


    private void UpdateCharacterSelection()
    {
        if (player.IsLocal)
        {
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
        {
            { "CharacterIndex", selectedCharacterIndex }
        };
            player.SetCustomProperties(properties);
        }
    }


    public override void OnPlayerPropertiesUpdate(Player player,ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("CharacterIndex"))
        {
            selectedCharacterIndex = (int)changedProps["CharacterIndex"];
            UpdateCharacterSprite();
        }
    }*/
}
