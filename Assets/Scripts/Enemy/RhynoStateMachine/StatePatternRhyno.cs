using UnityEngine;
using System.Collections;

public class StatePatternRhyno : MonoBehaviour
{
    public float damage;
    public float moveSpeed;
    public float attackSpeed;
    public float rotSpeed;
    public float range;
    public float hitForce;
    public int points;
    public float prepareAttackTimer;
    [HideInInspector] public float attackTimer;
    [HideInInspector] public float restTimer;
    public float shortRestTimer;
    public float longRestTimer;
    public float updatePathTimer;
    [HideInInspector] public float pathTimer;
    [HideInInspector] public float timer;
    [HideInInspector] public Rigidbody myRigidbody;
    [HideInInspector] public Health myHealth;
    [HideInInspector] public float distance;
    [HideInInspector] public Transform target;
    [HideInInspector] public NavMeshAgent agent;

    public IRhynoState currentState;
    public RestStateRhyno restState;
    public ApproachStateRhyno approachState;
    public PrepareAttackStateRhyno preAttackState;
    public AttackStateRhyno attackState;

    void Awake ()
    {
        restState = new RestStateRhyno(this);
        approachState = new ApproachStateRhyno(this);
        attackState = new AttackStateRhyno(this);
        preAttackState = new PrepareAttackStateRhyno(this);

        if (GameObject.FindGameObjectWithTag("Player"))
            target = GameObject.FindGameObjectWithTag("Player").transform;
        else
            target = transform;
        myHealth = transform.GetComponent<Health>();
        myRigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        //agent.stoppingDistance = range;
        agent.speed = moveSpeed;
    }
	
    void Start()
    {
        currentState = approachState;
        currentState.EnterState();
    }

	void Update ()
    {
        currentState.UpdateState();
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }

    void OnDestroy()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
            GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlayerPoints>().AddPoints(points);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.transform.name != "Floor")
        {
            if (currentState == attackState)
            {
                if (col.collider.transform.GetComponent<Rigidbody>())
                {
                    col.collider.transform.GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * hitForce, col.collider.transform.position, ForceMode.Impulse);
                }
                if (col.collider.transform.GetComponent<HardObstacle>())
                {
                    myRigidbody.AddForce(-transform.forward * hitForce / 5, ForceMode.Impulse);
                    restTimer = longRestTimer;
                    restState.EnterState();
                    currentState = restState;
                }
            }
        }
    }

    void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
