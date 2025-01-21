using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    [Header("Configuración del HUD")]
    public Image healthFillImage; // Imagen que representa la barra de vida

    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHealthHUD;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHealthHUD;
    }

    private void UpdateHealthHUD(float currentHealth, float maxHealth)
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = currentHealth / maxHealth;
        }
    }
}
