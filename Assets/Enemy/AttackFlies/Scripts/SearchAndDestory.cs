using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AgentState
{
    Idle,
    Search,
    Attack
}

[RequireComponent(typeof(NavMeshAgent))]
public class SearchAndDestory : MonoBehaviour
{
    public float searchRadius = 20;
    public float attackRadius = 10;

    protected NavMeshAgent navMeshAgent;
    
	RaycastHit navHit;
    AgentState agentState;
	AgentState prevState;

    Transform player;
	public Transform Player { get { return player; } }


	public Material matDebug;

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
		MatDebug(Color.grey);
		navMeshAgent.isStopped = true;
		// Play idle animation

		// Maybe patrol a certain route
	}

	protected virtual void Search()
	{
		MatDebug(Color.green);
		// Get closer to player to attack
		navMeshAgent.isStopped = false;
		navMeshAgent.SetDestination(Player.position);
	}

	protected virtual void Attack()
	{
		MatDebug(Color.red);
		navMeshAgent.isStopped = true;


	}

	protected virtual void MatDebug(Color color)
	{
		if (matDebug != null)
			matDebug.color = color;
	}
}