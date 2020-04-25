using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHit : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();
        if(gameObject.tag == "left")
            rigidbody2D.transform.position = new Vector3(-2.55f, -2.75f, 0);
        else
            rigidbody2D.transform.position = new Vector3(+2.55f, -2.75f, 0);
    }
}
