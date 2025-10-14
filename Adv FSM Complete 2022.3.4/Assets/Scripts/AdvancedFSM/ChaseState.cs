using UnityEngine;
using System.Collections;
using System.Linq;

public class ChaseState : FSMState
{
    public bool playerHit;
    public ChaseState(Transform[] wp) 
    { 
        waypoints = wp;
        stateID = FSMStateID.Chasing;

        curRotSpeed = 1.0f;
        curSpeed = 100.0f;

        //find next Waypoint position
        FindNextPoint();
    }

    public override void Reason(Transform player, Transform npc)
    {
        //Set the target position as the player position
        destPos = player.position;
        switch (owner.mySense)
        {
            case NPCTankController.Senses.NULL:
                SmellReasoning(player, npc);
                break;
            case NPCTankController.Senses.Sight:
                SightReasoning(player, npc);
                break;
            case NPCTankController.Senses.Touch:
                TouchReasoning(player, npc);
                break;
            default:
                break;
        }

    }

    public void TouchReasoning(Transform player, Transform npc)
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerHit = true;
        }
    }
    public void SmellReasoning(Transform player, Transform npc)
    {
        //Check the distance with player tank
        //When the distance is near, transition to attack state
        float dist = Vector3.Distance(npc.position, destPos);
        if (dist <= 200.0f)
        {
            Debug.Log("Switch to Attack state");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.ReachPlayer);
        }
        //Go back to patrol is it become too far
        else if (dist >= 300.0f)
        {
            Debug.Log("Switch to Patrol state");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.LostPlayer);
        }
    }
    float timer = 0;
    public void SightReasoning(Transform player, Transform npc)
    {
        float dist = Vector3.Distance(npc.position, destPos);
        if (dist <= 200.0f)
        {
            //Debug.Log("Switch to Attack state");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.ReachPlayer);
            owner.Sauron.SetActive(false);
        }
        Vector3 direction = npc.forward;

        Vector3 npcOffset = npc.transform.position + new Vector3(0, 10, 0);
        Debug.DrawLine(npcOffset, npcOffset + (direction * 1000));
        RaycastHit ri;
        Physics.Raycast(npcOffset, npcOffset + (direction * 1000), out ri);

        if (ri.collider.transform.gameObject.tag != "Player")
        {
            timer += 2 * Time.deltaTime;
           
        }
        else
        {
            timer = 0;
        }
        owner.Sauron.SetActive(true);
       
        if(timer > 10)
        {
            npc.GetComponent<NPCTankController>().SetTransition(Transition.LostPlayer);
            timer = 0;
            owner.Sauron.SetActive(false);
        }
    }
    public override void Act(Transform player, Transform npc)
    {
        //Rotate to the target point
        destPos = player.position;

        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //Go Forward
        npc.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }
}
