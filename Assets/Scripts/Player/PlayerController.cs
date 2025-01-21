using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[Serializable]
public class ElementSoul
{
    public ElementType elementType; // Tipo de elemento (agua, fuego, etc.)
    public int currentSouls; // Cantidad actual de souls
    public bool isUnlocked; // Indica si el elemento está desbloqueado
}

public class PlayerController : MonoBehaviourPunCallbacks
{

    public SpriteRenderer spriteRenderer;
    public PlayerSprites spritesSettings;


    public ElementSprite elementCurrent;
    List<ElementSprite> elementSpritePlayer;

    public List<ElementSoul> elementSouls;
    int maxSouls;
    public static event Action<ElementType, int> UpdateSoulHUD;

    public bool canChangeToWater = true;
    public bool canChangeToFire = true;
    public bool canChangeToLeaf = true;
    public bool canChangeToRock = true;

    public override void OnEnable()
    {
        InputManager.playerControls.Player.ChangeElement1.performed += ctx => OnChangeElement(0, canChangeToWater);
        InputManager.playerControls.Player.ChangeElement2.performed += ctx => OnChangeElement(1, canChangeToFire);
        InputManager.playerControls.Player.ChangeElement3.performed += ctx => OnChangeElement(2, canChangeToLeaf);
        InputManager.playerControls.Player.ChangeElement4.performed += ctx => OnChangeElement(3, canChangeToRock);
    }

    public override void OnDisable()
    {
        InputManager.playerControls.Player.ChangeElement1.performed -= ctx => OnChangeElement(0, canChangeToWater);
        InputManager.playerControls.Player.ChangeElement2.performed -= ctx => OnChangeElement(1, canChangeToFire);
        InputManager.playerControls.Player.ChangeElement3.performed -= ctx => OnChangeElement(2, canChangeToLeaf);
        InputManager.playerControls.Player.ChangeElement4.performed -= ctx => OnChangeElement(3, canChangeToRock);
    }

    private void OnChangeElement(int elementIndex, bool canChange)
    {
        if (!photonView.IsMine) return; // Solo el propietario puede cambiar de elemento

        if (canChange && elementIndex >= 0 && elementIndex < elementSpritePlayer.Count)
        {
            photonView.RPC("ChangeElement", RpcTarget.All, elementIndex);
            ResetElementState(elementIndex);
        }
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            //int selectedSpriteIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties["CharacterIndex"];
            photonView.RPC("Init", RpcTarget.All);
        }
    }


    [PunRPC]
    public void ChangeElementRPC(int elementIndex)
    {
        elementCurrent = elementSpritePlayer[elementIndex];
        spriteRenderer.sprite = elementCurrent.sprite;
    }

    [PunRPC]
    public void Init()
    {
        elementSpritePlayer = new List<ElementSprite>();
        spritesSettings.AgregarSprites(elementSpritePlayer);

        spriteRenderer.sprite = elementCurrent.sprite;

        maxSouls = GameplayManager.instance.soulsNeeded;

        // Inicializar el estado de las souls
        InitElementSouls();
    }

    private void InitElementSouls()
    {
        elementSouls = new List<ElementSoul>();

        // Inicializar las "souls" para cada tipo de elemento
        foreach (ElementSprite element in spritesSettings.elementSprites)
        {
            elementSouls.Add(new ElementSoul
            {
                elementType = element.elementType,
                currentSouls = maxSouls, // Inicia con el máximo de souls
                isUnlocked = true        // Inicia como desbloqueado
            });

            // Notificar al HUD sobre el estado inicial
            UpdateSoulHUD?.Invoke(element.elementType, maxSouls);
        }
    }

    private void ResetElementState(int elementIndex)
    {
        ElementSoul soul = elementSouls[elementIndex];

        // Reiniciar las souls y bloquear el cambio
        soul.currentSouls = 0;
        soul.isUnlocked = false;

        // Notificar al HUD sobre el cambio
        UpdateSoulHUD?.Invoke(soul.elementType, 0);
    }

    [PunRPC]
    public void ChangeElement(int index)
    {
        ElementSoul soul = elementSouls[index];

        elementCurrent = spritesSettings.elementSprites.Find(e => e.elementType == soul.elementType);
        spriteRenderer.sprite = elementCurrent.sprite;
    }

    public void AddSoul(ElementType type, int amount)
    {
        // Buscar el elemento en la lista
        ElementSoul soul = elementSouls.Find(e => e.elementType == type);
        if (soul == null) return;

        // Actualizar la cantidad de souls
        soul.currentSouls += amount;
        if (soul.currentSouls >= maxSouls)
        {
            soul.currentSouls = maxSouls;
            soul.isUnlocked = true;
        }

        UpdateSoulHUD?.Invoke(soul.elementType, soul.currentSouls); // Notificar al HUD
    }

    private void CheckElementUnlock(ElementType tipo)
    {

        switch (tipo)
        {
            case ElementType.Agua:
                canChangeToWater = true;
                break;
            case ElementType.Fuego:
                canChangeToFire = true;
                break;
            case ElementType.Hoja:
                canChangeToLeaf = true;
                break;
            case ElementType.Piedra:
                canChangeToRock = true;
                break;
        }
        
    }

}
