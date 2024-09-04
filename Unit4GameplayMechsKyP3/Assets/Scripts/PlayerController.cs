using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float kbPower = 10;
    public bool activePower = false;

    public float hangTime;
    public float smashSpeed;
    public float explosionForce;
    public float explosionRadius;
    private bool smashing = false;
    private float floorY;

    public float cooldown;

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

        cooldown = cooldown - Time.deltaTime;
        if (currentPower == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F) && cooldown <= 0)
        {
            ProjectilePush();
            cooldown = 0.5f;
        }

        if (currentPower == PowerUpType.SizeUp)
        {
            GetBigPower();
        }
        else
        {
            transform.localScale = playerSize;
        }

        if (currentPower == PowerUpType.Smash && Input.GetKeyDown(KeyCode.Space) && !smashing)
        {
            smashing = true;
            StartCoroutine(SmashPower());
        }
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


    void RotatePowerupIndicator()
    {
        if (powerUpIndicator != null)
        powerUpIndicator.transform.Rotate(Vector3.up);
    }
    

    void GetBigPower()
    {
        Vector3 getbig = new Vector3(2.5f, 2.5f, 2.5f);
        transform.localScale = getbig;
        playerRb.mass = 2;
        kbPower = 4;
    }

    void ProjectilePush()
    {
        foreach (var enemy in FindObjectsOfType<EnemyController>())
        {
            tmpRocket = Instantiate(homingRockets, transform.position + Vector3.up, Quaternion.identity);
            tmpRocket.GetComponent<MissileScript>().Fire(enemy.transform);
        }
    }

    IEnumerator SmashPower()
    {
        var enemies = FindObjectsOfType<EnemyController>();

        //saves the y position of the floor
        floorY = transform.position.y;

        //calculates the amount of time we go up
        float jumpTime = Time.time + hangTime;

        while(Time.time < jumpTime)
        {
            //this moves the player up while keeping their x velocity.
            playerRb.velocity = new Vector2(playerRb.velocity.x, smashSpeed);
            yield return null;
        }

        while (transform.position.y > floorY) 
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -smashSpeed * 2);
            yield return null;
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i] != null )
            {
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
            }
        }
        smashing = false;
    }
}
