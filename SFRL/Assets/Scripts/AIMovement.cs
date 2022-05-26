using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CG.SFRL.Enemy;
using CG.SFRL.Characters;

public class AIMovement : MonoBehaviour
{
    Transform target;
    [SerializeField] float _timeBetweenShots;
    [SerializeField] float _attackSpeed;
    NavMeshAgent agent;

    Vector3 _startingPosition;
    EnemyAimWeapon _enemyAimWeapon;
    TestEnemy _testEnemy;

    State _state;

    private enum State
    {
        Idle,
        ChaseTarget,
    }

    void Awake()
    {
        _enemyAimWeapon = gameObject.GetComponent<EnemyAimWeapon>();
        _testEnemy = gameObject.GetComponent<TestEnemy>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        _startingPosition = transform.position;
        _state = State.Idle;       
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            default:
            case State.Idle:
                FindTarget();
                break;
            case State.ChaseTarget:
                if (target != null)
                {
                    agent.SetDestination(target.position);
                    _enemyAimWeapon.HandleAim();

                    float _attackRange = 5f;
                    if (Vector3.Distance(transform.position, target.position) < _attackRange)
                    {
                        if (Time.time > _timeBetweenShots)
                        {
                            agent.isStopped = true;
                            _testEnemy.Shoot();
                            _timeBetweenShots = Time.time + (1 / _attackSpeed);                           
                        }

                    }
                    else
                    {
                        agent.isStopped = false;
                    }
                }
                break;
        }
    }

    void FindTarget()
    {
        float _targetRange = 10f;
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.position) < _targetRange)
            {
                _state = State.ChaseTarget;
            }
        }       
    }
    
}
