using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructor : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Fruit")
            ScoreKeeper.score -= 1;

        Destroy(col.gameObject);
    }
}
