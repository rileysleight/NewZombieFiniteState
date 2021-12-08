using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public float changeMind;
    private StateMachine brain;
    private Animator animator;
    [SerializeField]private UnityEngine.UI.Text stateNote;
    private UnityEngine.AI.NavMeshAgent agent;
    private Player player;
    private bool playerIsNear;
    private bool withinAttackRange;
    private float attackTimer;

    void Start()
    {
        player = FindObjectOfType<Player>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        brain = GetComponent<StateMachine>();
        playerIsNear = false;
        withinAttackRange = false;

        brain.PushState(Idle, OnIdleEnter, OnIdleExit);

    }

    // Update is called once per frame
    void Update()
    {
        playerIsNear = Vector3.Distance(transform.position, player.transform.position) < 5;
        if (playerIsNear)
        {
            //print("PIN");
        }
        withinAttackRange = Vector3.Distance(transform.position, player.transform.position) < 2;
    }

    void OnIdleEnter()
    {
        stateNote.text = "Idle";
        agent.ResetPath();
    }
    void Idle()
    {
        changeMind -= Time.deltaTime;
        if (playerIsNear)
        {
            brain.PushState(Chase, OnChaseEnter, OnChaseExit);
        }
        else if (changeMind <= 0)
        {
            brain.PushState(Wander, OnWanderEnter, OnWanderExit);
            changeMind = Random.Range(4,10);
        }
    }
    void OnIdleExit()
    {

    }
    
    void OnWanderEnter()
    {
        stateNote.text = "Wander";
        animator.SetBool("Chase", true);
        Vector3 wanderDirection = (Random.insideUnitSphere * 4f) + transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(wanderDirection, out navMeshHit, 1f, NavMesh.AllAreas);
        Vector3 destination = navMeshHit.position;
        agent.SetDestination(destination);
    }
    void Wander()
    {
        if (agent.remainingDistance <= .25f)
        {
            agent.ResetPath();
            brain.PushState(Idle, OnIdleEnter, OnIdleExit);
        }
        if (playerIsNear)
        {
            brain.PushState(Chase, OnChaseEnter, OnChaseExit);
        }
    }
    void OnWanderExit()
    {
        animator.SetBool("Chase", false);
    }

    void OnChaseEnter()
    {
        animator.SetBool("Chase", true);
        stateNote.text = "Chase";
    }
    void Chase()
    {
        agent.SetDestination(player.transform.position);
        if(Vector3.Distance(transform.position, player.transform.position) > 5.5f)
        {
            brain.PopState();
            brain.PushState(Idle, OnIdleEnter, OnIdleExit);
        }
        if (withinAttackRange)
        {
            //print("AA");
            brain.PushState(Attack, OnEnterAttack, null);
        }
    }
    void OnChaseExit()
    {
        animator.SetBool("Chase", false);
    }

    void OnEnterAttack()
    {
        agent.ResetPath();
        stateNote.text = "Attack";
    }
    void Attack()
    {
        attackTimer -= Time.deltaTime;
        if (!withinAttackRange)
        {
            brain.PopState();
        }
        else if (attackTimer <= 0)
        {
            animator.SetTrigger("Attack");
            player.Hurt(2,1);
            attackTimer = 2f;
        }
    }
}
