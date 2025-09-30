using UnityEngine;
using System.Collections;

public class HealState : FSMState
{
    public NPCTankController variableTracker;
    public HealState(Transform[] wp) 
    { 
        waypoints = wp;
        stateID = FSMStateID.Healing;

        curRotSpeed = 1.0f;
        curSpeed = 100.0f;

        //find next Waypoint position
        FindNextPoint();
    }
    float healthUp;
    float timeElapsed;
    public override void Reason(Transform player, Transform npc)
    {
        
      

        //Check the distance with player tank
        //When the distance is near, transition to attack state
        float dist = Vector3.Distance(npc.position, destPos);
        if (dist <= 200.0f)
        {
           if(variableTracker.health >= 100)
            {
                npc.GetComponent<NPCTankController>().SetTransition(Transition.LostPlayer);
                healthUp = 0;
            }
            else
            {
                healthUp += 0.2f * Time.deltaTime;
                variableTracker.health += (int)healthUp;
                if (healthUp > 2)
                    healthUp = 0;
            }
        }
        
    }

    public override void Act(Transform player, Transform npc)
    {
        //Rotate to the target point
        destPos = healCamp.transform.position;
        //Zig, but also Zag
        
        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //Go Forward
        if ((int)timeElapsed % 2 == 0)
        {
            npc.Translate((Vector3.forward + Vector3.right) * Time.deltaTime * curSpeed);
        }
        else
        {
            npc.Translate((Vector3.forward - Vector3.right) * Time.deltaTime * curSpeed);
        }
        
        timeElapsed += Time.deltaTime;
        
    }

   
}
