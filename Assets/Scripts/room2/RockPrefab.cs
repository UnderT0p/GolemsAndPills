using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPrefab : MonoBehaviour
{
    
    [SerializeField] private AudioClip pushGround;

    public bool OnGround=false;
    void Awake() //метод инициализации всех скриптов, заранее.
    {
        Destroy(gameObject, 1.5f);
    }

    public void SetForceAndRotation(Vector3 force, Vector3 rot)
    {
        gameObject.GetComponent<Rigidbody>().AddTorque(rot);
        gameObject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }
    public void CrashRock()
    {
        gameObject.GetComponentInChildren<ParticleSystem>().Play();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")&&!OnGround)
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Floor")&&!OnGround)
        {
            gameObject.GetComponent<AudioSource>().Play();
            OnGround = true;
            CrashRock();
        }
        
    }
}
