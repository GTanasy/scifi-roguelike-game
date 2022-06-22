using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CG.SFRL.Enemy;
using CG.SFRL.Characters;

public class AIMovement : MonoBehaviour
{
    public BasicEnemy enemyStats;
    float _attackSpeed;
    float _timeBetweenShots;
    float _engagementRange;

    RoundSpawner roundManager;

    bool inLOS;

    Transform _target;
    NavMeshAgent _agent;

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
            _target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        roundManager = GameObject.Find("RoundManager").GetComponent<RoundSpawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _engagementRange = enemyStats.engagementRange;
        _attackSpeed = enemyStats.attackSpeed;
        _agent.speed = enemyStats.speed;
        if (roundManager.looped == true)
        {
            _agent.speed += 2;
        }

        _state = State.ChaseTarget;
        _timeBetweenShots = 1 / _attackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyStateMachine();

    }

    void FindTarget()
    {
        float _targetRange = 10f;
        if (_target != null)
        {
            if (Vector3.Distance(transform.position, _target.position) < _targetRange)
            {
                _state = State.ChaseTarget;
            }
        }       
    }

    static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    void EnemyStateMachine()
    {
        switch (_state)
        {
            default:
            case State.Idle:
                FindTarget();
                break;
            case State.ChaseTarget:
                if (_target != null)
                {
                    _agent.SetDestination(_target.position);
                    Vector3 _lookDirection = (_target.position - transform.position).normalized;
                    float _angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg - 0f;
                    RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, GetVectorFromAngle(_angle));

                    if (raycastHit2D.collider != null)
                    {
                        if (raycastHit2D.collider.CompareTag("Player"))
                        {
                            inLOS = true;
                        }
                        else
                        {
                            inLOS = false;
                        }

                    }
                    Vector3 _scale = Vector3.one;
                    Vector3 _scaleBar = new Vector3();

                    if (_angle < -90 || _angle > 90)
                    {
                        _scale.x = +1.0f;
                        _scaleBar.x = -0.01f;
                        _scaleBar.y = 0.01f;
                        _scaleBar.z = 0.01f;
                    }
                    else
                    {
                        _scale.x = -1.0f;
                        _scaleBar.x = +0.01f;
                        _scaleBar.y = 0.01f;
                        _scaleBar.z = 0.01f;
                    }
                    gameObject.GetComponent<Transform>().localScale = _scale;
                    gameObject.GetComponentInChildren<RectTransform>().localScale = _scaleBar;



                    if (Vector3.Distance(transform.position, _target.position) < _engagementRange && inLOS == true)
                    {
                        if (Time.time > _timeBetweenShots)
                        {                            
                            _agent.isStopped = true;
                            _enemyAimWeapon.Shoot();
                            _timeBetweenShots = Time.time + (1 / _attackSpeed);
                        }

                    }
                    else
                    {
                        _agent.isStopped = false;
                    }
                }
                break;
        }
    }
}
