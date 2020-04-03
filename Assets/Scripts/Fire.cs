using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
    public GameObject player;
    public GameObject playerPrefab;

    public void fireTrigger()
    {
        Transform transform = player.transform;

        GameObject clone;
        clone = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        clone.SetActive(true);

        Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0.0f, 10f);
    }
}
