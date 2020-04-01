using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
    public GameObject player;
    public GameObject playerPrefab;

    public FireButton fireButton;

    void Update()
    {
        /*
        if (fireButton.isFirePressed)
        {
            fireButton.isFirePressed = false;
            StartCoroutine(fireTriggerCoroutine());
        }
        */
    }

    IEnumerator fireTriggerCoroutine()
    {
        fireTrigger();
        yield return new WaitForSeconds(1f);
    }

    public void fireTrigger()
    {
        Transform transform = player.transform;
        //Debug.Log(transform.position);
        GameObject clone;
        clone = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        clone.SetActive(true);
        Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0.0f, 10f);
    }
}
