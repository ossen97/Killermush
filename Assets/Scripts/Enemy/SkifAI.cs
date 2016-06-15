using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

[RequireComponent(typeof(NavMeshAgent))]
public class SkifAI : StateBehaviour
{
    Transform target;
    public Transform skifBall;
    public Transform shootPoint;
    public ParticleRenderer smoke;
    public float smokeDamage;
    public float smokeRange;
    public float moveSpeed;
    public float rotSpeed;
    public float attackRange;
    public float minDistanceToTarget;
    public float explosionForce;
    public float attackTimer;
    public float shootAngle;

    private float timer;
    private float gravity;
    private float distance; 
    private Health myHealth;
    private NavMeshAgent agent;

    enum States
    {
        Init,
        Approach,
        Attack,
        Die
    }

    void Awake()
    {
        Initialize<States>();
        ChangeState(States.Init);
    }

    void Init_Enter()
    {
        Debug.Log("INIZIALIZZO");
        target = GameObject.FindGameObjectWithTag("Player").transform;
        myHealth = transform.GetComponent<Health>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.angularSpeed = rotSpeed;
        agent.stoppingDistance = minDistanceToTarget;
        gravity = Physics.gravity.magnitude;
        smoke.enabled = false;
        ChangeState(States.Approach);
    }

    void Approach_Enter()
    {
        Debug.Log("APPROACH");
    }

    void Approach_FixedUpdate()
    {
        agent.SetDestination(target.position);
        distance = Vector3.Distance(transform.position, target.position);
        if (distance < attackRange)
            ChangeState(States.Attack);
        if (myHealth.currentHealth <= 0)
            ChangeState(States.Die);
    }

    void Attack_Enter()
    {
        Debug.Log("ATTACK");
    }

    void Attack_Update()
    {
        RotateToTarget(rotSpeed);
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Transform ball = (Transform)Instantiate(skifBall, shootPoint.position, Quaternion.identity);
            ball.transform.GetComponent<Rigidbody>().velocity = BallisticVel(target, shootAngle);
            timer = attackTimer;
        }

        if (myHealth.currentHealth <= 0)
            ChangeState(States.Die);

        distance=Vector3.Distance(transform.position, target.position);
        if (distance > attackRange)
            ChangeState(States.Approach);

        if (distance < smokeRange)
        {
            smoke.enabled = true;
            target.GetComponent<Health>().Damage(smokeDamage);
        }
        else
            smoke.enabled = false;
        
    }

    void Die_Enter()
    {
        Debug.Log("MORTO");
        Destroy(gameObject);
    }

    Vector3 BallisticVel(Transform target, float angle)
    {
        Vector3 dir = target.position - transform.position;  // get target direction
        float h = dir.y;  // get height difference
        dir.y = 0;  // retain only the horizontal direction
        float dist = dir.magnitude;  // get horizontal distance
        float a = angle * Mathf.Deg2Rad;  // convert angle to radians
        dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
        dist += h / Mathf.Tan(a);  // correct for small height differences
        // calculate the velocity magnitude
        float vel = Mathf.Sqrt(dist * gravity / Mathf.Sin(2 * a));
        return vel * dir.normalized;
    }

    void RotateToTarget(float rotSpeed)
    {
        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
        rotation.x = 0; rotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotSpeed);
    }
}
