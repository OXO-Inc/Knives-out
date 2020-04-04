using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitDestroyer : MonoBehaviour
{
    public GameObject particleSystem;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Destroyer")
        {
            Transform particleSystemTransform = particleSystem.GetComponent<Transform>();
            ParticleSystem particleSystemEmission = particleSystem.GetComponent<ParticleSystem>();
            particleSystemTransform.position = transform.position;
            particleSystemEmission.Play();
            ScoreKeeper.score += 1;
            Destroy(col.gameObject);
            
        }
    }
}
