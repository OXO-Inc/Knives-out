using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject player;
    public GameObject playerPrefab;

    public Player playerScript;

    public AudioSource fireKnife;

    public void fireTrigger()
    {
        if (Time.timeScale != 0)
        {
            fireKnife.Play();
            Transform transform = player.transform;

            GameObject clone;
            clone = Instantiate(playerPrefab, transform.position, Quaternion.identity);
            clone.SetActive(true);
            //TODO: Change to decrease when knife gets destroyed then only.
            playerScript.numberOfKnives -= 1;
            Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(0.0f, 10f);
        }
    }
}
