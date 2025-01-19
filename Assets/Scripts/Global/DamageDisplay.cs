using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;

public class DamageDisplay : MonoBehaviour
{
    [Header("Configuración del texto")]
    public GameObject damageTextPrefab; // Prefab del texto a mostrar
    public Vector2 randomAreaSize = new Vector2(1f, 1f); // Tamaño del área de aparición aleatoria (ancho x alto)
    public float textLifetime = 1f; // Duración del texto visible

    [Header("Animación del texto")]
    public float moveSpeed = 1f; // Velocidad de movimiento hacia arriba
    public float fadeSpeed = 2f; // Velocidad de desvanecimiento

    private Transform displayArea; // Área base donde aparecerá el texto (generalmente sobre el enemigo)

    private void Start()
    {
        displayArea = GetComponent<Transform>();
    }

    [PunRPC]
    public void ShowDamage(float damage, float multiplier)
    {

        // Crear el texto del daño
        //TMP_Text damageText = Instantiate(damageTextPrefab, displayArea.position, Quaternion.identity, displayArea);

        damageTextPrefab.SetActive(true);

        TMP_Text damageText = damageTextPrefab.GetComponent<TMP_Text>();
        damageText.text = Mathf.RoundToInt(damage).ToString();

        // Asignar color según el multiplicador del daño
        if (multiplier > 1f) damageText.color = Color.red; // Daño crítico (débil al elemento)
        else if (multiplier < 1f) damageText.color = Color.blue; // Daño reducido (resistente al elemento)
        else damageText.color = Color.white; // Daño normal

        // Ajustar posición inicial con un desplazamiento aleatorio dentro del área definida
        Vector3 randomPosition = GetRandomPositionInArea();
        damageText.transform.position = randomPosition;

        // Iniciar la animación del texto
        StartCoroutine(AnimateDamageText(damageText));
    }

    private Vector3 GetRandomPositionInArea()
    {
        Vector2 offset = new Vector2(
            Random.Range(-randomAreaSize.x / 2f, randomAreaSize.x / 2f),
            Random.Range(-randomAreaSize.y / 2f, randomAreaSize.y / 2f)
        );

        return displayArea.position + new Vector3(offset.x, offset.y, 0);
    }

    private IEnumerator AnimateDamageText(TMP_Text damageText)
    {
        float elapsedTime = 0f;
        Color originalColor = damageText.color;

        while (elapsedTime < textLifetime)
        {
            // Mover el texto hacia arriba
            damageText.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            // Reducir opacidad
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / textLifetime);
            damageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Destruir el texto al final de la animación
        //Destroy(damageText.gameObject);
        damageTextPrefab.SetActive(false);
    }
}
