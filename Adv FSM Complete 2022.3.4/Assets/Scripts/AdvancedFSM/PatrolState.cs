using UnityEngine;using System.Collections;public class PatrolState : FSMState{    public float time = 0;    public float timeToGoGambling;    public PatrolState(Transform[] wp)
    {
        waypoints = wp;        stateID = FSMStateID.Patrolling;        time = 0;        timeToGoGambling = 2;        curRotSpeed = 1.0f;        curSpeed = 100.0f;    }    public bool playerHit = false;        public override void Reason(Transform player, Transform npc)    {
        time += Time.deltaTime;
        switch (owner.mySense)
        {
            case NPCTankController.Senses.NULL:
                SmellReasoning(player, npc);
                break;
            case NPCTankController.Senses.Sight:
                SightReasoning(player, owner.transform);
                break;
            case NPCTankController.Senses.Touch:
                TouchReasoning(player, npc);
                break;
            default:
                break;
        }
        int rareNumber = Random.Range(0, 1000); // 0.01% chance per Reason() call
        if (rareNumber == 777)
        {
            //npc.GetComponent<NPCTankController>().SetTransition(Transition.NinjaCamp); 
        }        if (time > timeToGoGambling && !GameManager.CheckOffDuty())
        {
            // npc.GetComponent<NPCTankController>().SetTransition(Transition.Random);
            GameManager.agentOffDuty = true;
        }

        //int rarerNumber = Random.Range(0, 10000);
        //if(rarerNumber < 300)
        //{
        //    npc.GetComponent<NPCTankController>().SetTransition(Transition.Random);
        //}

    }    public void TouchReasoning(Transform player, Transform npc)
    {
        if (owner.playerHit)
        {
            owner.Sauron.SetActive(true);
            owner.Explode();
        }
    }    public void SmellReasoning(Transform Player, Transform npc)
    {
        //1. Check the distance with player tank
        if (Vector3.Distance(npc.position, Player.position) <= 300.0f)
        {
            //2. Since the distance is near, transition to chase state
            Debug.Log("Switch to Chase State");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.SawPlayer);
        }
    }    public void SightReasoning(Transform player, Transform npc)
    {
            Vector3 direction = npc.forward;
            Ray r = new Ray(npc.transform.position, npc.forward);
            Vector3 npcOffset = npc.transform.position + new Vector3(0, 10, 0);
            Debug.DrawLine(npcOffset, npcOffset + (direction * 1000), Color.red);
            RaycastHit ri;
            Physics.Raycast(npcOffset, npcOffset + (direction * 1000), out ri);
            if(ri.collider != null)
            {
                if(ri.collider.transform.gameObject.tag == "Player")
                {
                    Debug.Log("WOW A PLAYER!!!");
                    npc.GetComponent<NPCTankController>().SetTransition(Transition.SawPlayer);
                }
            }
    }    public override void Act(Transform player, Transform npc)    {
        //1. Find another random patrol point if the current point is reached
        if (Vector3.Distance(npc.position, destPos) <= 100.0f)
        {
            Debug.Log("Reached to the destination point, calculating the next point");
            FindNextPoint();
        }

        //2. Rotate to the target point

        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);



        //3. Go Forward
        npc.Translate(Vector3.forward * Time.deltaTime * curSpeed);

    }}