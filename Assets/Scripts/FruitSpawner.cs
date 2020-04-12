using UnityEngine;

public class FruitSpawner : MonoBehaviour
{

    public Sprite[] fruits;

    public GameObject fruit;

    public Player player;

    private float spawnTime = 0f;
    public float spawnDelay = 1f;
    public float speedMultiplier = .7f;

    public float[] speeds = new float[7] { -4.32f, -4.64f, -4.8f, -4.48f, -4.96f, -4f, -4.16f };

    void Start()
    {
        callInvoke();
    }

    void Update()
    {
        if (player.gameOver)
        {
            CancelInvoke("spawnFruit");
            return;
        }
    }

    public void callInvoke()
    {
        CancelInvoke("spawnFruit");
        InvokeRepeating("spawnFruit", spawnTime, spawnDelay);
    }

    public void spawnFruit()
    {
        int i = Random.Range(0, 6);
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(-9f, 9f) * 10f);
        GameObject clone = Instantiate(fruit, new Vector3(Random.Range(-2.25f, 2.25f), 5.5f, 0), rotation);
        SpriteRenderer sr = clone.GetComponent<SpriteRenderer>();
        sr.sprite = fruits[i];
        clone.SetActive(true);
        Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0.0f, speeds[i] * speedMultiplier);
    }
}
