using UnityEngine;

public class FruitDestroyer : MonoBehaviour
{
    public GameObject particleSystem;

    public Player player;

    public AudioSource fruitHit;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Destroyer")
        {
            fruitHit.Play();
            player.plusScoreAnimation(transform);
            Transform particleSystemTransform = particleSystem.GetComponent<Transform>();
            ParticleSystem particleSystemEmission = particleSystem.GetComponent<ParticleSystem>();
            particleSystemTransform.position = transform.position;
            particleSystemEmission.Play();
            ScoreKeeper.score += 1;
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
