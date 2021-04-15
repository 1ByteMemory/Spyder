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

	public Transform Player { get; private set; }

	[HideInInspector]
	public bool spawnedFromSpawner;

	public Transform gunPosition;
	protected Transform activeWeapon;

	private GameManager gm;
	private bool addedToList;

	private Animator anim;


	[HideInInspector]
	public bool hasLineOfSight;

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
        Player = GameObject.FindGameObjectWithTag("Player").transform;

		anim = GetComponentInChildren<Animator>();

		if (gunPosition != null)
		{
			InstantiateWeapons(gunPosition);
			if (gunPosition.childCount > 0)
				activeWeapon = gunPosition.GetChild(0);
		}
	}

	// Update is called once per frame
	protected virtual void Update()
    {
		if (!GameManager.IsPaused)
		{
			float distance = Mathf.Abs((Player.position - transform.position).magnitude);

			if (distance > searchRadius)
			{
				agentState = AgentState.Idle;
			}
			else if (Physics.Raycast(transform.position, Player.position - transform.position, out navHit, searchRadius))
			{
				hasLineOfSight = navHit.transform.CompareTag("Player");

				if (!hasLineOfSight)
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
		else
		{
			// Stop playing animations
			anim.speed = 0;
		}
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
		Debug.Log("Destroying");
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

	bool IsPositionInRange(float a, float b, float d)
	{
		if (a > b - d && a < b + d)
			return true;
		else
			return false;

	}

	protected virtual void Search()
	{

		navMeshAgent.SetDestination(Player.position);

		bool x = IsPositionInRange(navMeshAgent.pathEndPosition.x, Player.position.x, 2);
		bool y = IsPositionInRange(navMeshAgent.pathEndPosition.y, Player.position.y, 2);
		bool z = IsPositionInRange(navMeshAgent.pathEndPosition.z, Player.position.z, 2);

		if (x && y && z)
		{
			// Get closer to player to attack
			navMeshAgent.isStopped = false;


			if (anim != null)
				anim.SetTrigger("Running");
		}
		else
		{
			navMeshAgent.isStopped = true;

			if (hasLineOfSight)
			{
				Attack();
			}
		}
	}

	protected virtual void Attack()
	{
		if (anim != null)
			anim.SetTrigger("Attacking");


		if (agentState == AgentState.Attack)
			navMeshAgent.isStopped = true;


		transform.LookAt(Player);

		gunPosition.LookAt(Player);

		if (gunPosition.childCount > 0)
			UseWeapon(activeWeapon);

	}
}