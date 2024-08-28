using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private float kbPower = 8.0f;
    public bool activePower = false;

    private GameObject focalPoint;
    public GameObject powerUpIndicator;

    private Rigidbody playerRb;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
    }

    // Update is called once per frame
    void Update()
    {
        float fwdInput = Input.GetAxis("Vertical");
        float sideInput = Input.GetAxis("Horizontal");

        //AddForce adds an amount of force to the object in the direction of the first parameter.
        playerRb.AddForce(focalPoint.transform.forward * fwdInput * speed);
        playerRb.AddForce(focalPoint.transform.right * sideInput * speed);

        powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.35f, 0);
        RotatePowerupIndicator();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PowerUp"))
        {
            activePower = true;
            powerUpIndicator.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountdown());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRb.AddForce(awayFromPlayer * kbPower, ForceMode.Impulse);
            Debug.Log($"Collision has occured with {collision.gameObject.name} with powerup set to {activePower}");
        }
    }

    IEnumerator PowerUpCountdown()
    {
        yield return new WaitForSeconds(7);
        activePower = false;
        powerUpIndicator.SetActive(false);
    }

    void RotatePowerupIndicator()
    {
        if (powerUpIndicator != null)
        powerUpIndicator.transform.Rotate(Vector2.up);
    }
}
