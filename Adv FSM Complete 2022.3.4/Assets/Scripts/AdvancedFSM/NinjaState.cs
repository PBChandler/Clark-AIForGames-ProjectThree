using UnityEngine;

public class NinjaState : FSMState
{
    private Flag targetFlag;
    public NPCTankController variableTracker;
    public float moveSpeed = 30f;
    public float stopDistance = 20f; // how close before it sits still

    public NinjaState()
    {
        stateID = FSMStateID.Ninja;
    }

    public override void Reason(Transform player, Transform npc)
    {
        // Switch to chase if player is close
        if (Vector3.Distance(npc.position, player.position) <= 300.0f)
        {
            Debug.Log("Switch to Chase State");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.SawPlayer);
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        if (targetFlag == null || targetFlag.IsBeingRaised || targetFlag.IsFullMast)
        {
            targetFlag = FindClosestAvailableFlag(npc.position);
        }

        if (targetFlag != null)
        {
            float dist = Vector3.Distance(npc.position, targetFlag.transform.position);

            if (dist > stopDistance)
            {
                // Move toward the flag
                Vector3 dir = (targetFlag.transform.position - npc.position).normalized;
                npc.position += dir * moveSpeed * Time.deltaTime;

                // Optional: rotate to face flag
                npc.rotation = Quaternion.Slerp(
                    npc.rotation,
                    Quaternion.LookRotation(dir),
                    Time.deltaTime * 5f
                );
            }
            else
            {
                // Sit still near flag
            }
        }
    }

    private Flag FindClosestAvailableFlag(Vector3 npcPos)
    {
        Flag[] allFlags = GameObject.FindObjectsOfType<Flag>();
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
