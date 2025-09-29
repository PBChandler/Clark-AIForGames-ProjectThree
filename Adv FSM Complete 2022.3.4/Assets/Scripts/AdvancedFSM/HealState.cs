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
    public override void Reason(Transform player, Transform npc)
    {
        //Set the target position as the player position
        if (healthUp % 2 == 0)
        {
            destPos = healCamp.transform.position + npc.transform.right;
        }
        else
        {
            destPos = healCamp.transform.position - npc.transform.right;
        }
        

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

        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //Go Forward
        npc.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }

   
}
