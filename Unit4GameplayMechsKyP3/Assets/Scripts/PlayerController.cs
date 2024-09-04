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
    public GameObject homingRockets;
    private GameObject tmpRocket;
    private Coroutine powerupCountdown;

    private Rigidbody playerRb;
    private Vector3 playerSize = new Vector3(1.5f, 1.5f, 1.5f);

    public PowerUpType currentPower = PowerUpType.None;

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
            activePower = true;
            currentPower = other.gameObject.GetComponent<PowerUp>().powerUpType;
            powerUpIndicator.SetActive(true);
            Destroy(other.gameObject);

            if (powerupCountdown != null)
            {
                StopCoroutine(powerupCountdown);
            }

            powerupCountdown = StartCoroutine(PowerUpCountdown());

            /*    // checks if the object that triggers is the sizeup powerup
                if (other.gameObject == GameObject.Find("SizeUp(Clone)"))
                {
                    GetBigPower();
                    Debug.Log("YESSSS");
                    activePower = true;
                    powerUpIndicator.SetActive(true);
                    Destroy(other.gameObject);
                    StartCoroutine(BigBoyCountdown());
                }

                // checks if the powerup is knockback powerup
                if (other.gameObject == GameObject.Find("KnockbackPower(Clone)"))
                {
                    Debug.Log("YASSSS");
                    activePower = true;
                    increaseKb = true;
                    powerUpIndicator.SetActive(true);
                    Destroy(other.gameObject);
                    StartCoroutine(KBCountdown());
                }

               /* if (other.gameObject == GameObject.Find("MissilePower(Clone)"))
                {
                    if (other.gameObject == GameObject.Find("KnockbackPower(Clone)"))
                    {
                        Debug.Log("YASSSS");
                        activePower = true;
                        powerUpIndicator.SetActive(true);
                        Destroy(other.gameObject);
                        StartCoroutine(ProjectilePush());
                    }
                } */
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && currentPower == PowerUpType.Knockback)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRb.AddForce(awayFromPlayer * kbPower, ForceMode.Impulse);
            Debug.Log($"Collision has occured with {collision.gameObject.name} with powerup set to {currentPower.ToString()}");
        }
    }

    IEnumerator PowerUpCountdown()
    {
        yield return new WaitForSeconds(7);
        activePower = false;
        currentPower = PowerUpType.None;
        powerUpIndicator.SetActive(false);
    }

    IEnumerator BigBoyCountdown()
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
        playerRb.mass = 2;
    }

    void ProjectilePush()
    {
        foreach (var enemy in FindObjectsOfType<EnemyController>())
        {
            tmpRocket = Instantiate(homingRockets, transform.position + Vector3.up, transform.rotation);
            
        }
    }
}
