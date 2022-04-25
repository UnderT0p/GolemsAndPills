using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed = 100f;
    public event System.Action punchPlayer;
    public event System.Action shutPlayer;
    public event System.Action coinPlayer;
    private float timerFire=0.2f;
    private bool apruveFire;
   
    private void FixedUpdate()
    {
        if (timerFire<=0)
        {
            if (apruveFire)
            {
                GameObject newBullet = Instantiate(bullet, gameObject.GetComponentInChildren<Camera>().transform.position + gameObject.GetComponentInChildren<Camera>().transform.forward, gameObject.GetComponentInChildren<Camera>().transform.rotation);
                Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();
                bulletRB.velocity = gameObject.GetComponentInChildren<Camera>().transform.forward * bulletSpeed;
                shutPlayer?.Invoke();
                apruveFire = false;
            }
        }
        else
        {
            timerFire -= Time.deltaTime;
        }
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            apruveFire = true;
        }
        else
        {
            apruveFire = false;
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameBehavior.GetInstance().DecrementPlayerHP();
            punchPlayer?.Invoke();
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rock")&& !collision.gameObject.GetComponent<RockPrefab>().OnGround)
        {
            GameBehavior.GetInstance().DecrementPlayerHP();
            punchPlayer?.Invoke();
        }
        if (collision.gameObject.CompareTag("Coin"))
        {
            coinPlayer?.Invoke();
            Destroy(collision.transform.parent.gameObject);
            GameBehavior.GetInstance().Items += 1;
        }
    }


}
