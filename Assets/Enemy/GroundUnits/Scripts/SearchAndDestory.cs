using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AgentState
{
    Idle,
    Search,
	SearchAndAttack,
    Attack,
	Stun
}

[RequireComponent(typeof(NavMeshAgent))]
public class SearchAndDestory : WeaponBehaviour
{
    public float searchRadius = 20;
    public float attackRadius = 10;

	public float stunnedTime = 1;

    protected NavMeshAgent navMeshAgent;
    
	RaycastHit navHit;
    AgentState agentState;

	protected bool isStunned;

	public Transform player { get; private set; }

	[HideInInspector]
	public bool spawnedFromSpawner;

	public Transform gunPosition;
	protected Transform activeWeapon;

	private GameManager gm;
	private bool addedToList;

	private Animator anim;

	private void OnValidate()
	{
		if (attackRadius > searchRadius)
		{
			attackRadius = searchRadius;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRadius);

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, searchRadius);

	}

	// Start is called before the first frame update
	protected override void Start()
    {
		base.Start();

		gm = FindObjectOfType<GameManager>();

		navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

		anim = GetComponentInChildren<Animator>();

		InstantiateWeapons(gunPosition);
		if (gunPosition.childCount > 0)
			activeWeapon = gunPosition.GetChild(0);
		else
			Debug.Log(transform + "Doesn't have a gun!");
	}

	// Update is called once per frame
	protected virtual void Update()
    {
        float distance = Mathf.Abs((player.position - transform.position).magnitude);

		if (distance > searchRadius)
		{
			agentState = AgentState.Idle;
		}
		else if (Physics.Raycast(transform.position, player.position - transform.position, out navHit, searchRadius))
		{
            if (!navHit.transform.CompareTag("Player"))
			{
				RemoveFromSeenList();
				agentState = AgentState.Search;
			}
			else if (distance > attackRadius)
			{
				AddToSeenList();
				agentState = AgentState.Search;
			}
			else
			{
				AddToSeenList();
				agentState = AgentState.Attack;
			}
		}


        TakeAction(agentState);
    }

	private void AddToSeenList()
	{
		if (!addedToList)
		{
			addedToList = true;
			gm.AddEnemy(gameObject);
		}
	}

	private void RemoveFromSeenList()
	{
		if (addedToList)
		{
			addedToList = false;
			gm.RemoveEnemy(gameObject);
		}
	}

	protected virtual void TakeAction(AgentState state)
	{
		if (navMeshAgent.isOnNavMesh)
		{

			switch (state)
			{
				case AgentState.Idle:
					Idle();
					break;
				case AgentState.Search:
					Search();
					break;
				case AgentState.SearchAndAttack:
					Search();
					Attack();
					break;
				case AgentState.Attack:
					Attack();
					break;
				default:
					break;
			}
		}
		else
		{
			Debug.LogWarning(gameObject.name + " was not on a Nav Mesh, destroying game object.");

			// prevent further spawns
			if (spawnedFromSpawner)
			{
				gameObject.GetComponentInParent<EnemyWaveSpawner>().spawnAnother = false;
				Debug.Log(gameObject.transform.parent.name + " has stopped spawning");
			}

			Destroy(gameObject);
		}
	}

	private void OnDestroy()
	{
		RemoveFromSeenList();
	}

	public virtual void EnemyDefeated()
	{
		RemoveFromSeenList();
		if (spawnedFromSpawner)
		{
			GetComponentInParent<EnemyWaveSpawner>().remainingEnemies--;
		}
	}

	protected virtual void Idle()
	{
		navMeshAgent.isStopped = true;

		// Play idle animation
		if (anim != null)
			anim.SetTrigger("Idle");

		// Maybe patrol a certain route
	}

	protected virtual void Search()
	{
		// Get closer to player to attack
		navMeshAgent.isStopped = false;
		navMeshAgent.SetDestination(player.position);

		if (anim != null)
			anim.SetTrigger("Running");

	}

	protected virtual void Attack()
	{
		if (anim != null)
			anim.SetTrigger("Attacking");


		if (agentState == AgentState.Attack)
			navMeshAgent.isStopped = true;

		if (gunPosition.childCount > 0)
			UseWeapon(activeWeapon);

	}

	/*
	public virtual void Stun()
	{
		navMeshAgent.isStopped = true;

		isStunned = true;
	}
	*/
}