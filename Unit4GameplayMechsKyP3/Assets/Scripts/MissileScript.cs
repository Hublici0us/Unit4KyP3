using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    Rigidbody missileRb;

    float speed = 50;
    float impactForce = 7.5f;

    // Start is called before the first frame update
    void Start()
    {
        missileRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 missileDirection = FindAnyObjectByType<EnemyController>().transform.position - this.transform.position;
        transform.Translate(missileDirection * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 missileHit = collision.transform.position - this.transform.position;
            missileRb.AddForce(missileHit * impactForce, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }
}
