using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public Slider volumenSlider;
    private AudioSource altavoz;

    private void Start()
    {
        altavoz = FindAnyObjectByType<AudioSource>();
        volumenSlider.value = PlayerPrefs.GetFloat("Volumen");
        altavoz.volume = volumenSlider.value;
    }

    private void Update()
    {
        if (altavoz != null)
        {
            altavoz.volume = volumenSlider.value;
            PlayerPrefs.SetFloat("Volumen", altavoz.volume);
        }
        else
        {
            altavoz = FindAnyObjectByType<AudioSource>();
        }
        
    }
}
