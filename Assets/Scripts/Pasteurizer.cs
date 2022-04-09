using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pasteurizer : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            particle.Play();
        }
    }
}
