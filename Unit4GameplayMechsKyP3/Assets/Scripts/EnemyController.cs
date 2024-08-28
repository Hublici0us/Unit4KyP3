using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;

    private Rigidbody enemyRb;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //normalized prevents the enemy from moving to the player too fast. w/o it, the larger the distance bettween the enemy and tthe player, the faster it'lll go.
        Vector3 facePlayerDirection = (player.transform.position - transform.position).normalized;

        enemyRb.AddForce(facePlayerDirection * speed);
        DestroyOutOfBounds();
    }

    void DestroyOutOfBounds()
    {
        if (this.transform.position.y > 7.5f)
        {
            Destroy(gameObject);
        }
    }
}
