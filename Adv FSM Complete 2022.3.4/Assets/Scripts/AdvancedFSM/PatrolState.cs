using UnityEngine;using System.Collections;public class PatrolState : FSMState{    public float time = 0;    public float timeToGoGambling;    public PatrolState(Transform[] wp)     {         waypoints = wp;        stateID = FSMStateID.Patrolling;        time = 0;        timeToGoGambling = 2;        curRotSpeed = 1.0f;        curSpeed = 100.0f;    }    public override void Reason(Transform player, Transform npc)    {
        time += Time.deltaTime;
        //1. Check the distance with player tank
        if (Vector3.Distance(npc.position, player.position) <= 300.0f)
        {
            //2. Since the distance is near, transition to chase state
            Debug.Log("Switch to Chase State");
            npc.GetComponent<NPCTankController>().SetTransition(Transition.SawPlayer);
        }        int rareNumber = Random.Range(0, 1000); // 0.01% chance per Reason() call
        if (rareNumber == 777)
        {
            npc.GetComponent<NPCTankController>().SetTransition(Transition.NinjaCamp); 
        }        if(time > timeToGoGambling && !GameManager.CheckOffDuty())
        {
            npc.GetComponent<NPCTankController>().SetTransition(Transition.Random);
            GameManager.agentOffDuty = true;
        }        //int rarerNumber = Random.Range(0, 10000);        //if(rarerNumber < 300)
        //{
        //    npc.GetComponent<NPCTankController>().SetTransition(Transition.Random);
        //}    }    public override void Act(Transform player, Transform npc)    {
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
        npc.Translate(Vector3.forward * Time.deltaTime * curSpeed);    }}