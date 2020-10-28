using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AgentState
{
    Idle,
    Search,
    Attack,
	Stun
}

[RequireComponent(typeof(NavMeshAgent))]
public class SearchAndDestory : MonoBehaviour
{
    public float searchRadius = 20;
    public float attackRadius = 10;

	public float stunnedTime = 1;

    protected NavMeshAgent navMeshAgent;
    
	RaycastHit navHit;
    AgentState agentState;
	AgentState prevState;

	protected bool isStunned;

	public Transform player { get; private set; }


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
	protected virtual void Start()
    {
		navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
		
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
				agentState = AgentState.Search;
			}
			else if (distance > attackRadius)
			{
				agentState = AgentState.Search;
			}
			else
			{
				agentState = AgentState.Attack;
			}
		}


        TakeAction(agentState);
    }



	protected virtual void TakeAction(AgentState state)
	{
		switch (state)
		{
			case AgentState.Idle:
				Idle();
				break;
			case AgentState.Search:
                Search();
				break;
			case AgentState.Attack:
                Attack();
				break;
			default:
				break;
		}
	}


	protected virtual void Idle()
	{
		navMeshAgent.isStopped = true;
		// Play idle animation

		// Maybe patrol a certain route
	}

	protected virtual void Search()
	{
		// Get closer to player to attack
		navMeshAgent.isStopped = false;
		navMeshAgent.SetDestination(player.position);
	}

	protected virtual void Attack()
	{
		navMeshAgent.isStopped = true;


	}

	public virtual void Stun()
	{
		navMeshAgent.isStopped = true;

		isStunned = true;
	}
}