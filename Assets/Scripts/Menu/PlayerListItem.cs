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

    [HideInInspector]
    public Photon.Realtime.Player player; // Referencia al jugador
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
            // Establece la propiedad personalizada "CharacterIndex"
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
        {
            { "CharacterIndex", selectedCharacterIndex }
        };

            player.SetCustomProperties(properties);

            // Verifica si la clave existe antes de intentar acceder a ella
            if (player.CustomProperties.TryGetValue("CharacterIndex", out object characterIndex))
            {
                Debug.Log((int)characterIndex); // Loguea el índice
            }
            else
            {
                Debug.LogWarning("CharacterIndex no está definido en las propiedades personalizadas del jugador.");
            }
        }
    }

    /// <summary>
    /// Maneja la actualización de propiedades personalizadas.
    /// </summary>
    public void OnPlayerPropertiesUpdated(ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("CharacterIndex"))
        {
            selectedCharacterIndex = (int)changedProps["CharacterIndex"];
            UpdateCharacterSprite();
        }
    }
}
