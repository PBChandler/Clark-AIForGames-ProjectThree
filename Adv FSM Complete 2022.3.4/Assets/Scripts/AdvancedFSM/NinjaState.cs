using UnityEngine;
using UnityEngine.AI;

public class NinjaState : FSMState
{
    private Flag targetFlag;
    public NPCTankController variableTracker;
    public float moveSpeed = 30f;
    public float stopDistance = 20f;
    
    private const float MinWaitTimeBeforeImpatience = 3f;
    private const float MaxProbabilityWaitTime = 12f;
    private const float ImpatientCheckInterval = 0.5f;
    
    private float waitTimer = 0f;
    private float nextCheckTime = 0f;

    public NinjaState()
    {
        stateID = FSMStateID.Ninja;
    }

    public override void Reason(Transform player, Transform npc)
    {
        if (Vector3.Distance(npc.position, player.position) <= 300.0f)
        {
            Debug.Log("Switch to Chase State");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.SawPlayer);
            return;
        }

        EvaluateImpatience(npc);
    }
    
    private void EvaluateImpatience(Transform npc)
    {
        if (Time.time < nextCheckTime)
            return;

        nextCheckTime = Time.time + ImpatientCheckInterval;
        
        if (waitTimer < MinWaitTimeBeforeImpatience)
            return;

        float impatienceProbability = CalculateImpatienceProbability(waitTimer);
        
        if (Random.value < impatienceProbability)
        {
            Debug.Log($"Tank got impatient after {waitTimer:F1}s of waiting (probability: {impatienceProbability:P0})");
            
            TankBoredomVisuals visuals = npc.GetComponent<TankBoredomVisuals>();
            if (visuals == null)
            {
                visuals = npc.gameObject.AddComponent<TankBoredomVisuals>();
            }
            visuals.ShowBoredomEffect();
            
            npc.GetComponent<NPCTankController>().SetTransition(Transition.Random);
            ResetImpatienceTimer();
        }
    }

    private float CalculateImpatienceProbability(float currentWaitTime)
    {
        float normalizedTime = Mathf.Clamp01(
            (currentWaitTime - MinWaitTimeBeforeImpatience) / 
            (MaxProbabilityWaitTime - MinWaitTimeBeforeImpatience)
        );
        
        return Mathf.Pow(normalizedTime, 1.5f) * 0.4f;
    }

    private void ResetImpatienceTimer()
    {
        waitTimer = 0f;
        nextCheckTime = 0f;
    }

    public override void Act(Transform player, Transform npc)
    {
        if (targetFlag == null || targetFlag.IsBeingRaised || targetFlag.IsFullMast)
        {
            targetFlag = FindClosestAvailableFlag(npc.position);
        }

        NavMeshAgent agent = npc.GetComponent<NavMeshAgent>();
        
        if (agent == null)
        {
            Debug.LogError("NinjaState requires NavMeshAgent component!");
            return;
        }

        if (targetFlag != null)
        {
            float dist = Vector3.Distance(npc.position, targetFlag.transform.position);

            if (dist > stopDistance)
            {
                agent.isStopped = false;
                agent.destination = targetFlag.transform.position;
                agent.speed = moveSpeed;
                
                ResetImpatienceTimer();
            }
            else
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                
                Vector3 directionToFlag = (targetFlag.transform.position - npc.position).normalized;
                if (directionToFlag != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToFlag);
                    npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * 5f);
                }
                
                waitTimer += Time.deltaTime;
            }
        }
        else
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            waitTimer += Time.deltaTime;
        }
    }

    private Flag FindClosestAvailableFlag(Vector3 npcPos)
    {
        Flag[] allFlags = GameObject.FindObjectsByType<Flag>(FindObjectsSortMode.None);
        Flag closest = null;
        float minDist = Mathf.Infinity;

        foreach (Flag f in allFlags)
        {
            if (f.IsBeingRaised || f.IsFullMast) continue;

            float dist = Vector3.Distance(npcPos, f.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = f;
            }
        }

        return closest;
    }
}
