using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{

    public Sprite[] fruits;

    public GameObject fruit;

    private float spawnTime = 1f;
    private float spawnDelay = .75f;

    private float[] speeds = new float[8] { -4.32f, -4.64f, -4.16f, -4.8f, -4.48f, -4.96f, -4f, -4.16f };

    void Start()
    {
        InvokeRepeating("spawnFruit", spawnTime, spawnDelay);
    }

    public void spawnFruit()
    {
        int i = Random.Range(0, 7);
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-4.5f, 4.5f) * 10f);
        GameObject clone = Instantiate(fruit, new Vector3(Random.Range(-2.25f, 2.25f), 6, 0), rotation);
        SpriteRenderer sr = clone.GetComponent<SpriteRenderer>();
        sr.sprite = fruits[i];
        clone.SetActive(true);
        Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0.0f, speeds[i] * .75f);
        //TODO: Rotate object
        //TODO: CancelInvoke("spwanFruit");
    }
}
