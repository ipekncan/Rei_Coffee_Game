using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent enemy_1;

    public float startWaitTime = 4;
    public float rotateTime = 2;
    public float speedWalk = 4;   
    public float speedRun = 6;   
    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    public Transform[] waypoints;
    int m_currentWaypointIndex;
    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_playerPosition;

    float m_waitTime;
    float m_rotateTime;
    bool m_playerInRange;
    bool m_playerNear;
    bool m_isPatrol;
    bool m_playerCaught;

    Transform m_playerTransform;
    Animator m_animator;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_playerPosition = Vector3.zero;
        m_waitTime = startWaitTime;
        m_rotateTime = rotateTime;
        m_isPatrol = true;
        m_playerCaught = false;
        m_playerInRange = false;
        m_currentWaypointIndex = 0;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            m_playerTransform = playerObj.transform;

        enemy_1 = GetComponent<NavMeshAgent>();
        enemy_1.isStopped = false;
        enemy_1.speed = speedWalk;
        enemy_1.SetDestination(waypoints[m_currentWaypointIndex].position);
    }

    void Update()
    {
        EnvironmentView();

        if (!m_isPatrol)
            Chasing();
        else
            Patrol();
    }

    void Move(float speed)
    {
        enemy_1.isStopped = false;
        enemy_1.speed = speed;
        m_animator.SetFloat("Speed", speed);
        m_animator.SetBool("IsChasing", !m_isPatrol);
    }

    void Stop()
    {
        enemy_1.isStopped = true;
        enemy_1.speed = 0;
        m_animator.SetFloat("Speed", 0);
    }

    void CaughtPlayer()
    {
        m_playerCaught = true;
    }

    void NextPoint()
    {
        m_currentWaypointIndex = (m_currentWaypointIndex + 1) % waypoints.Length;
        enemy_1.SetDestination(waypoints[m_currentWaypointIndex].position);
    }

    void LookingPlayer(Vector3 player)
    {
        enemy_1.SetDestination(player);

        if (Vector3.Distance(transform.position, player) <= 3)
        {
            if (m_waitTime <= 0)
            {
                m_playerNear = false;
                Move(speedWalk);
                enemy_1.SetDestination(waypoints[m_currentWaypointIndex].position);
                m_waitTime = startWaitTime;
                m_rotateTime = rotateTime;
            }
            else
            {
                Stop();
                m_waitTime -= Time.deltaTime;
            }
        }
    }

    void EnvironmentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
        m_playerInRange = false;

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToPlayer) < viewAngle / 2)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleMask))
                {
                    m_playerInRange = true;
                    m_playerPosition = player.position;

                    m_isPatrol = false;
                }
            }
        }

        if (!m_playerInRange && !m_isPatrol)
        {
        }
    }

    void Patrol()
    {
        if (m_playerNear)
        {
            if (m_rotateTime <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                m_rotateTime -= Time.deltaTime;
            }
        }
        else
        {
            m_playerNear = false;
            playerLastPosition = Vector3.zero;
            enemy_1.SetDestination(waypoints[m_currentWaypointIndex].position);

            if (enemy_1.remainingDistance <= enemy_1.stoppingDistance)
            {
                if (m_waitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_waitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_waitTime -= Time.deltaTime;
                }
            }
        }
    }

    void Chasing()
    {
        m_playerNear = false;
        playerLastPosition = Vector3.zero;

        if (!m_playerCaught)
        {
            Move(speedRun);
            enemy_1.SetDestination(m_playerPosition);
        }

        if (enemy_1.remainingDistance <= enemy_1.stoppingDistance)
        {
            float distToPlayer = m_playerTransform != null
                ? Vector3.Distance(transform.position, m_playerTransform.position)
                : float.MaxValue;

            if (m_waitTime <= 0 && !m_playerCaught && distToPlayer >= 6f)
            {
                m_isPatrol = true;
                m_playerNear = false;
                Move(speedWalk);
                m_rotateTime = rotateTime;
                m_waitTime = startWaitTime;
                enemy_1.SetDestination(waypoints[m_currentWaypointIndex].position);
            }
            else
            {
                if (distToPlayer >= 2.5f)
                {
                    Stop();
                    m_waitTime -= Time.deltaTime;
                }
            }
        }
    }
}