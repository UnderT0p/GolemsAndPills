using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private Transform patrolPositionOne;
    [SerializeField] private Transform patrolPositionTwo;
    [SerializeField] private Transform player;
    [SerializeField] private AudioClip stepClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private GameObject prefabEnemy;
    [SerializeField] private int timeDestroy;
    private Vector3 spawnPosition;
    private Transform currentPatrolPosition;
    private float timerForRaycast = 2f;
    private bool startRaycast = false;
    int spawnIndex;
    private NavMeshAgent agent;
    private int enemyLives=10;
    private bool apruveFite = false;
    private byte speedNavMeshAgent = 1;
    private byte agrSpeedNavMeshAgent = 2;
    private float maxDistanceRaycast = 15f;
    private float angleForRaycastAgrEffect = 90f;
    public int EnemyLives
    {
        get { return enemyLives; }
        private set { enemyLives = value;
            if (enemyLives<=0)
            {
                gameObject.SetActive(false);
                Invoke(nameof(Respawn), timeDestroy);
               
            }
        }
    }
    void Start()
    {
        spawnPosition = gameObject.transform.position;
        agent = GetComponent<NavMeshAgent>();
        currentPatrolPosition = patrolPositionTwo;
        MoveToNextPatrolLocation();
    }
    private void FixedUpdate()
    {
        if (startRaycast)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Ray ray = new Ray(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), player.position - transform.position);
            if (Physics.Raycast(ray,out RaycastHit hitInfo, maxDistanceRaycast))
            {
                if (hitInfo.collider.gameObject.CompareTag("Player")&& (Vector3.Angle(player.position - transform.position,transform.TransformDirection(Vector3.forward))<= angleForRaycastAgrEffect))
                {
                    agent.destination = player.position;
                    apruveFite = true;
                    gameObject.GetComponent<NavMeshAgent>().speed = agrSpeedNavMeshAgent;
                    gameObject.GetComponent<Animator>().SetBool("Walk", false);
                    gameObject.GetComponent<Animator>().SetBool("Run", true);
                }
                else
                {
                    if (Vector3.Distance(player.position, transform.position)>= maxDistanceRaycast)
                    {
                        apruveFite = false;
                        gameObject.GetComponent<NavMeshAgent>().speed = speedNavMeshAgent;
                        gameObject.GetComponent<Animator>().SetBool("Walk", true);
                        gameObject.GetComponent<Animator>().SetBool("Run", false);
                    }
                }
            }
            else
            {
                if (Vector3.Distance(player.position, transform.position) >= 15)
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
            agent.stoppingDistance =0f;
            if (agent.remainingDistance <= 0.5f && !agent.pathPending)
            {
                MoveToNextPatrolLocation();
            }
            agent.destination = currentPatrolPosition.position;
        }
        
    }
    private void Update()
    {
        if (timerForRaycast<=0)
        {
            startRaycast = true;
            timerForRaycast = 2f;
        }
        else
        {
            timerForRaycast -= Time.deltaTime;
        }
    }
    void Attack()
    {
        agent.destination = player.position;
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (player.position.x!= agent.destination.x || player.position.z != agent.destination.z)
            {
                if (Vector3.Distance(player.position, gameObject.transform.position)<=3f)
                {
                    agent.stoppingDistance = 1.5f;
                    agent.transform.LookAt(player);
                    gameObject.GetComponent<Animator>().SetBool("Attack", true);
                    gameObject.GetComponent<Animator>().SetBool("Run", false);
                }
                else
                {
                    gameObject.GetComponent<Animator>().SetBool("Attack", false);
                    gameObject.GetComponent<Animator>().SetBool("Run", false);
                }
            }
            else
            {
                agent.transform.LookAt(player);
                gameObject.GetComponent<Animator>().SetBool("Attack", true);
                gameObject.GetComponent<Animator>().SetBool("Run", false);
            }
        }
        else if (agent.remainingDistance > agent.stoppingDistance)
        {
            agent.stoppingDistance = 1.8f;
            gameObject.GetComponent<Animator>().SetBool("Run", true);
            gameObject.GetComponent<Animator>().SetBool("Attack", false);
        }
    }
    void MoveToNextPatrolLocation()
    {
        if (currentPatrolPosition == patrolPositionOne)
        {
            currentPatrolPosition = patrolPositionTwo;
        }
        else
        {
            currentPatrolPosition = patrolPositionOne;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            EnemyLives -= 1;
            if (EnemyLives>0)
            {
                gameObject.GetComponentInChildren<ParticleSystem>().transform.position = collision.transform.position;
                gameObject.GetComponentInChildren<ParticleSystem>().transform.rotation = gameObject.transform.rotation;
                gameObject.GetComponentInChildren<ParticleSystem>().Play();
                gameObject.GetComponentInChildren<ParticleSystem>().GetComponent<AudioSource>().clip = hitClip;
                gameObject.GetComponentInChildren<ParticleSystem>().GetComponent<AudioSource>().Play();
                agent.destination = player.position;
                apruveFite = true;
                Destroy(collision.gameObject);
            }
        }
    }
    private void Respawn()
    {
        GameObject copyEnemy = Instantiate(prefabEnemy);
        copyEnemy.transform.position = spawnPosition;
        copyEnemy.GetComponent<EnemyBehaviour>().currentPatrolPosition = patrolPositionOne;
        copyEnemy.SetActive(true);
        Destroy(gameObject);
    }
    private void WalkSoundPlay()
    {
        gameObject.GetComponent<AudioSource>().clip = stepClip;
        gameObject.GetComponent<AudioSource>().Play();
    }

    
}
