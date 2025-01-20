using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SoulParticle : MonoBehaviourPunCallbacks
{
    public ElementType soulType; // Tipo de soul (agua, fuego, etc.)
    //private int soulCount = 0;   // Cantidad de partículas recolectadas

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            // Agregar la cantidad correcta de souls al jugador
            player.AddSoul(soulType, 1);

            // Destruir la partícula
            Destroy(gameObject);
        }
    }




}
