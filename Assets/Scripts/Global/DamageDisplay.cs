using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;

public class DamageDisplay : MonoBehaviour
{
    [Header("Configuraci�n del texto")]
    public GameObject damageTextPrefab; 
    public Vector2 randomAreaSize = new Vector2(1f, 1f); 
    public float textLifetime = 1f; 

    [Header("Animaci�n del texto")]
    public float moveSpeed = 1f; 
    public float fadeSpeed = 2f; 

    private Transform displayArea; 

    private void Start()
    {
        displayArea = GetComponent<Transform>();
    }

    
    public void ShowDamage(float damage, float multiplier)
    {

        damageTextPrefab.SetActive(true);

        // Crear el texto del da�o
        TMP_Text damageText = Instantiate(damageTextPrefab, displayArea.position, Quaternion.identity, displayArea).GetComponent<TMP_Text>();
        damageText.text = Mathf.RoundToInt(damage).ToString();

        Color originalColor = damageText.color;
        damageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        // Asignar color seg�n el multiplicador del da�o
        if (multiplier > 1f) damageText.color = Color.red; // Da�o cr�tico (d�bil al elemento)
        else if (multiplier < 1f) damageText.color = Color.blue; // Da�o reducido (resistente al elemento)
        else damageText.color = Color.white; // Da�o normal

        // Ajustar posici�n inicial con un desplazamiento aleatorio dentro del �rea definida
        Vector3 randomPosition = GetRandomPositionInArea();
        damageText.transform.position = randomPosition;


        StopCoroutine(AnimateDamageText(damageText));
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
        Color originalColor = damageText.color;
        float elapsedTime = 0f;

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

        // Destruir el texto al final de la animaci�n
        Destroy(damageText.gameObject);
        
    }
}
