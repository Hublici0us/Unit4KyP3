using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private float kbPower = 1.0f;
    public bool activePower = false;

    private GameObject focalPoint;
    public GameObject powerUpIndicator;

    private Rigidbody playerRb;
    private Vector3 playerSize = new Vector3(1.5f, 1.5f, 1.5f);

    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = playerSize;
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
            
            GetBigPower();
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
        transform.localScale = playerSize;
        
    }

    void RotatePowerupIndicator()
    {
        if (powerUpIndicator != null)
        powerUpIndicator.transform.Rotate(Vector3.up);
    }
    
    void KnockbackPower()
    {
        kbPower = 10;
    }

    void GetBigPower()
    {
        Vector3 getbig = new Vector3(2.5f, 2.5f, 2.5f);
        transform.localScale = getbig;
    }
}
