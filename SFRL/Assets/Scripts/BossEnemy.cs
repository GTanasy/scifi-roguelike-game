using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CG.SFRL.Enemy;

public class BossEnemy : MonoBehaviour
{
    public BasicEnemy _enemyStats;
    float _attackSpeed;
    float _timeBetweenShots;
    float _engagementRange;

    Transform target;
    NavMeshAgent agent;

    EnemyAimWeapon _enemyAimWeapon;


    State _state;

    enum State
    {
        Idle,
        ChaseTarget,
        RunRandom,
        Attack,
        Enraged,
        EnragedAttack,
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

        _engagementRange = _enemyStats.engagementRange;
        _attackSpeed = _enemyStats.attackSpeed;
        agent.speed = _enemyStats.speed;

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

                    _state = State.Attack;
                }
                break;
            case State.Attack:
                if (Vector3.Distance(transform.position, target.position) < _engagementRange)
                {
                    if (Time.time > _timeBetweenShots)
                    {
                        _enemyAimWeapon.HandleAim();
                        agent.isStopped = true;
                        _enemyAimWeapon.Shoot();
                        _timeBetweenShots = Time.time + (1 / _attackSpeed);
                    }

                }
                else
                {
                    agent.isStopped = false;
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