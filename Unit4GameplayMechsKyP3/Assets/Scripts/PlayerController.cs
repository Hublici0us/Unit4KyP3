using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float kbPower = 10;
    public bool activePower = false;

    public bool increaseKb = false;
    public bool bigboy = false;

    private GameObject focalPoint;
    public GameObject powerUpIndicator;

    private Rigidbody playerRb;
    private Vector3 playerSize = new Vector3(1.5f, 1.5f, 1.5f);

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
        // checks if the object that triggers has the "PowerUp" tag
        if (other.gameObject.CompareTag("PowerUp"))
        {
            
            if (other.gameObject == GameObject.Find("SizeUp(Clone)"))
            {
                GetBigPower();
                Debug.Log("YESSSS");
                activePower = true;
                powerUpIndicator.SetActive(true);
                Destroy(other.gameObject);
                StartCoroutine(PowerUpCountdown());
            }

            if (other.gameObject == GameObject.Find("KnockbackPower(Clone)"))
            {
                Debug.Log("YASSSS");
                activePower = true;
                increaseKb = true;
                powerUpIndicator.SetActive(true);
                Destroy(other.gameObject);
                StartCoroutine(PowerUpCountdown());
            }

            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && increaseKb)
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

        bigboy = false;
        increaseKb = false;

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
        playerRb.mass = 2;
    }
}
