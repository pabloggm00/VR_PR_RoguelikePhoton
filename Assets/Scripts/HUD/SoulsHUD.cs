using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulsHUD : MonoBehaviour
{
    public Image fillAgua;
    public Image fillFuego;
    public Image fillHoja;
    public Image fillPiedra;

    int maxValue;

    private void OnEnable()
    {
        PlayerController.UpdateSoulHUD += UpdateSoul;
    }

    private void OnDisable()
    {
        PlayerController.UpdateSoulHUD -= UpdateSoul;
    }

    private void Start()
    {
        maxValue = GameplayManager.instance.soulsNeeded;

        Init();
    }

    public void UpdateSoul(ElementType tipo, int quantity)
    {


        switch (tipo)
        {
            case ElementType.Agua:
                fillAgua.fillAmount = (float)quantity/maxValue;
                break;
            case ElementType.Fuego:
                fillFuego.fillAmount = (float)quantity / maxValue;
                break;
            case ElementType.Hoja:
                fillHoja.fillAmount = (float)quantity / maxValue;
                break;
            case ElementType.Piedra:
                fillPiedra.fillAmount = (float)quantity / maxValue;
                break;
            default:
                break;
        }
    }

    void Init()
    {
        fillAgua.fillAmount = 0;
        fillFuego.fillAmount = 0;
        fillHoja.fillAmount = 0;
        fillPiedra.fillAmount = 0;
    }
}
