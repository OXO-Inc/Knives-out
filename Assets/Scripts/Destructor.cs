using UnityEngine;
using System.Collections;

public class Destructor : MonoBehaviour
{
    public Player player;

    public GameObject minusScore;

    public AudioSource fruitMiss;

    void OnTriggerEnter2D(Collider2D col)
    {
        fruitMiss.Play();
        if (col.gameObject.tag == "Fruit" && player.gameModeVal == 3)
            player.gameOverExpertMode();
        else if (col.gameObject.tag == "Fruit")
        {
            ScoreKeeper.score -= 1;
            minusScore.SetActive(true);
            StartCoroutine(minusScoreInactive());
        }

        Destroy(col.gameObject);
    }

    IEnumerator minusScoreInactive()
    {
        yield return new WaitForSeconds(1f);
        minusScore.SetActive(false);
    }
}
