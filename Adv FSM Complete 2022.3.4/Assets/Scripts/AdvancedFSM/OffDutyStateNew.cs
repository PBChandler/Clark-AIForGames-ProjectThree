using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class OffDutyStateNew : FSMState
{
    public float goBackToWork;
    private Vector3 rollAwayDestination;
    private bool hasSetDestination = false;
    private Vector3 originalPosition;
    
    public OffDutyStateNew(Transform[] wp) 
    { 
        waypoints = wp;
        stateID = FSMStateID.OffDuty;
        curRotSpeed = 1.0f;
        curSpeed = 50.0f;
        goBackToWork = 10;
        time = 0;
    }
    
    float time = 0;
    
    public override void Reason(Transform player, Transform npc)
    {
        if (!hasSetDestination)
        {
            originalPosition = npc.position;
            SetRollAwayDestination(npc);
            hasSetDestination = true;
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        time += Time.deltaTime;
        
        if (time > goBackToWork)
        {
            NavMeshAgent agent = npc.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.Warp(originalPosition);
                agent.isStopped = false;
            }
            
            npc.GetComponent<NPCTankController>().SetTransition(Transition.GoBackToWork);
            npc.GetComponent<NPCTankController>().health = 100;
            GameManager.UpdateOffDuty(false);
            
            time = 0;
            hasSetDestination = false;
            return;
        }
        
        NavMeshAgent defaultAgent = npc.GetComponent<NavMeshAgent>();
        if (defaultAgent != null)
        {
            defaultAgent.speed = curSpeed;
            defaultAgent.destination = rollAwayDestination;
            defaultAgent.isStopped = false;
        }
    }
    
    private void SetRollAwayDestination(Transform npc)
    {
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0;
        randomDirection = randomDirection.normalized;
        
        float rollDistance = Random.Range(80f, 150f);
        rollAwayDestination = npc.position + randomDirection * rollDistance;
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(rollAwayDestination, out hit, 200f, NavMesh.AllAreas))
        {
            rollAwayDestination = hit.position;
        }
        else
        {
            rollAwayDestination = npc.position + npc.forward * 100f;
            if (NavMesh.SamplePosition(rollAwayDestination, out hit, 200f, NavMesh.AllAreas))
            {
                rollAwayDestination = hit.position;
            }
        }
    }
}
