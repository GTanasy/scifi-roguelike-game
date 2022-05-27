using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CG.SFRL.Enemy;
using CG.SFRL.Characters;

public class AIMovement : MonoBehaviour
{
    [SerializeField] float _attackSpeed;
    float _timeBetweenShots;

    Transform target;
    NavMeshAgent agent;

    EnemyAimWeapon _enemyAimWeapon;
    
    State _state;

    enum State
    {
        Idle,
        ChaseTarget,
    }

    void Awake()
    {
        _enemyAimWeapon = gameObject.GetComponent<EnemyAimWeapon>();
        if (GameObject.FindGameObjectWithTag("Player").transform != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }      
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        _state = State.Idle;
        _timeBetweenShots = 1 / _attackSpeed;
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
                            _enemyAimWeapon.Shoot();
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