using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public abstract class Enemy : MonoBehaviour, IDamageable
{
 
    protected float _health ;
    internal Rigidbody2D _rb2D;
    protected PlayerController player;
    protected Vector3 target;
    protected NavMeshAgent agent;
    protected WeaponBase curWeapon;
    protected State _currentState;
    protected RaycastHit2D _hit;
    protected bool playerInRange;
    protected bool playerInSightRange;
  [SerializeField]  protected EnemyStatsSo stats;
  protected bool playerInLOS;
 private RaycastHit2D hit;
    private  float SightRange;
    private  float AttackRange;
    protected LayerMask whatIsPlayer, whatIsWall, whatIsBoth;
    protected bool Generated;

    public  enum EnemyType
    {
      Melee,
      Ranged,
    }
    protected enum State
    {
        Patrol,
        Pursuit,
        Attack 
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        _health = stats.health;
        SightRange = stats.SightRange;
        AttackRange = stats.AttackRange;
        _currentState = State.Patrol;
        whatIsWall = LayerMask.GetMask("Wall");
        whatIsPlayer = LayerMask.GetMask("Player");
        whatIsBoth = LayerMask.GetMask("Player", "Wall");
        _rb2D = GetComponent<Rigidbody2D>();
        player = FindFirstObjectByType<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        curWeapon = GetComponentInChildren<WeaponBase>();
        
    }

    

    protected void Awake()
    {
      

    }

    private void Start()
    {
   
    }

    protected void Update()
    {
        playerInRange = Physics2D.OverlapCircle(transform.position, AttackRange, whatIsPlayer);
        playerInSightRange = Physics2D.OverlapCircle(transform.position, SightRange, whatIsPlayer);
        hit = Physics2D.Linecast(transform.position, player.transform.position, whatIsBoth);
        playerInLOS = hit.collider.gameObject.CompareTag("Player") && playerInSightRange;
        
       if(GameManager.CurGameMode == GameManager.GameMode.Kill)
       {
           if (!playerInRange && !playerInSightRange) _currentState = State.Patrol;
           if (playerInSightRange && !playerInRange) _currentState = State.Pursuit;
           if (playerInRange && playerInSightRange) _currentState = State.Attack;
       }
       else
       {
           if (!playerInRange && !playerInSightRange) _currentState = State.Pursuit;
           if (playerInRange && playerInSightRange) _currentState = State.Attack;
       }
     
        
       
        
        
        
        
        if (_health >= 0)
        {
            switch (_currentState)
            {
                case State.Pursuit:
                    Pursuit();
                    break;
                case State.Patrol:
                    Patrol();
                    break;
                case State.Attack:
                    Attack();
                    break;
                default:
                    Debug.LogWarning("Null state");
                   break;
            }
        }
        
        UpdateRotation();

        
    }


    private void Patrol()
    {
        if (!(agent.remainingDistance <= agent.stoppingDistance)) return;
        if (agent.hasPath && agent.velocity.sqrMagnitude != 0f) return;
        if (!Generated)
        {
            StartCoroutine(PathCD());
        } 
    }
    private IEnumerator PathCD()
    {
        Generated = true;
        yield return new WaitForSeconds(0.5f);
        if (GetRandPoint(transform.position, 10, out var point))
        {
            agent.destination = point;
        }
        Generated = false;

    }

    private bool GetRandPoint(Vector3 center, float range, out Vector3 result)
    {
        var randomPoint =  center + Random.insideUnitSphere * range;
        if (NavMesh.SamplePosition(randomPoint, out var navMeshHit, 1.0f, NavMesh.AllAreas))
        {
            result = navMeshHit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }

    protected abstract void Pursuit();
    
    protected abstract void Attack();

    protected void UpdateRotation()
    {
        var rotation = Vector3.zero;
        
        if (_currentState == State.Attack)
        {
           rotation = player.transform.position - transform.position;
        }
        else
        {
            rotation = agent.steeringTarget - transform.position;
        }

        if (rotation.magnitude > 0.01f)
        {
            var rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
        print("rotating");
   
    }
    
    public abstract void TakeDamage(float damage);
}
