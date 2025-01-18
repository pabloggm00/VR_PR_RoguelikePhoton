using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [Header("UI Elements")]
    public TMP_Text playerNameText;
    public Image characterImage;
    public Button nextButton;
    public Button previousButton;

    private Photon.Realtime.Player player; // Referencia al jugador
    private PlayerSprites playerSprites; // Referencia al ScriptableObject con los sprites
    private int selectedCharacterIndex = 0;

    /// <summary>
    /// Configura la entrada del jugador.
    /// </summary>
    public void SetPlayer(Photon.Realtime.Player player, PlayerSprites sprites)
    {
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

        // Actualizar la selección actual desde las propiedades del jugador
        if (player.CustomProperties.ContainsKey("CharacterIndex"))
        {
            selectedCharacterIndex = (int)player.CustomProperties["CharacterIndex"];
        }

        UpdateCharacterSprite();
    }

    /// <summary>
    /// Cambia al siguiente personaje.
    /// </summary>
    public void NextCharacter()
    {
        selectedCharacterIndex = (selectedCharacterIndex + 1) % playerSprites.elementSprites.Count;
        UpdateCharacterSprite();
        UpdateCharacterSelection();
    }

    /// <summary>
    /// Cambia al personaje anterior.
    /// </summary>
    public void PreviousCharacter()
    {
        selectedCharacterIndex = (selectedCharacterIndex - 1 + playerSprites.elementSprites.Count) % playerSprites.elementSprites.Count;
        UpdateCharacterSprite();
        UpdateCharacterSelection();
    }

    /// <summary>
    /// Actualiza el sprite del personaje mostrado.
    /// </summary>
    private void UpdateCharacterSprite()
    {
        characterImage.sprite = playerSprites.elementSprites[selectedCharacterIndex].sprite;
    }

    /// <summary>
    /// Sincroniza la selección del personaje en las propiedades del jugador.
    /// </summary>
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
}
