using UnityEngine;using System.Collections;public class OffDutyState : FSMState{    public OffDutyState(Transform[] wp)     {         waypoints = wp;        stateID = FSMStateID.OffDuty;        curRotSpeed = 1.0f;        curSpeed = 100.0f;    }    public override void Reason(Transform player, Transform npc)    {

            }    public override void Act(Transform player, Transform npc)    {
        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //3. Go Forward
        npc.Translate(Vector3.right * Time.deltaTime * curSpeed);    }}