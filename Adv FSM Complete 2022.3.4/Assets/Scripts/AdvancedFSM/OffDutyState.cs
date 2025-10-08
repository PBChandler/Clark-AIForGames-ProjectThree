using UnityEngine;using System.Collections;public class OffDutyState : FSMState{    public float goBackToWork;    public OffDutyState(Transform[] wp)     {         waypoints = wp;        stateID = FSMStateID.OffDuty;        curRotSpeed = 1.0f;        curSpeed = 900.0f;        goBackToWork = 10;        time = 0;    }    float time = 0;    public override void Reason(Transform player, Transform npc)    {

            }    public override void Act(Transform player, Transform npc)    {
        time += Time.deltaTime;
        if(time > goBackToWork)
        {
            npc.GetComponent<NPCTankController>().SetTransition(Transition.GoBackToWork);
            npc.transform.position = Vector3.zero;
            GameManager.UpdateOffDuty(false);
            time = 0;
        }
        curSpeed = 300.0f;
        dontClampMovement = true;
        destPos = new Vector3(-1000, 0, -1000);
        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //3. Go Forward
        npc.Translate(npc.transform.forward * Time.deltaTime * curSpeed);    }}