using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRedGolem : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject prefabEnemy;
    [SerializeField] private int timeDestroy;
    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject RockPrefab;
    [SerializeField] private float force;
    [SerializeField] private ParticleSystem bulletInsert;
    private Vector3 spawnPosition;
    private float timerForRaycast = 1f;
    private bool startRaycast = false;
    private bool apruveFite = false;
    private int enemyLives = 50;
    private float maxDistanceForRaycast = 25f;
    public int EnemyLives
    {
        get { return enemyLives; }
        private set
        {
            enemyLives = value;
            if (enemyLives <= 0)
            {
                gameObject.SetActive(false);
                Invoke(nameof(Respawn), timeDestroy);
            }
        }
    }
    private void Start()
    {
        spawnPosition = transform.position;
    }
    private void FixedUpdate()
    {
        if (startRaycast)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), player.position - transform.position);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 25f))
            {
                if (hitInfo.collider.gameObject.CompareTag("Player") && (Vector3.Angle(player.position - transform.position, transform.TransformDirection(Vector3.forward)) <= 90f))
                {
                    apruveFite = true;
                }
                else
                {
                    if (Vector3.Distance(player.position, transform.position) >= maxDistanceForRaycast)
                    {
                        apruveFite = false;
                    }
                }
            }
            else
            {
                if (Vector3.Distance(player.position, transform.position) >= maxDistanceForRaycast)
                {
                    apruveFite = false;
                }
            }

            startRaycast = false;
        }
        if (apruveFite)
        {
            Attack();
        }
        else
        {
            Wait();
        }
    }

    private void Wait()
    {
        gameObject.GetComponent<Animator>().SetBool("Attack", false);
    }
    void Update()
    {
        if (timerForRaycast <= 0)
        {
            startRaycast = true;
            timerForRaycast = 2f;
        }
        else
        {
            timerForRaycast -= Time.deltaTime;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            EnemyLives -= 1;
            if (EnemyLives > 0)
            {
                gameObject.GetComponentInChildren<ParticleSystem>().GetComponent<AudioSource>().Play();
                apruveFite = true;
                Destroy(collision.gameObject);
            }
        }
    }
    void Attack()
    {
        gameObject.transform.LookAt(player);
        gameObject.GetComponent<Animator>().SetBool("Attack", true);              
    }
    private void Respawn()
    {
        GameObject copyEnemy = Instantiate(prefabEnemy);
        copyEnemy.transform.position = spawnPosition;
        copyEnemy.SetActive(true);
        Destroy(gameObject);
    }
    private void EnableRockMesh() // for anim event
    {
        rock.GetComponent<MeshRenderer>().enabled = true;
    }
    private void ThrowRock() // for anim event
    {
        rock.GetComponent<MeshRenderer>().enabled = false;
        RockPrefab newRock = Instantiate(RockPrefab, rock.transform.position, Quaternion.identity).GetComponent<RockPrefab>();
        newRock.SetForceAndRotation((player.position-newRock.transform.position).normalized * force, new Vector3(5, 0, 0));
    }
}
