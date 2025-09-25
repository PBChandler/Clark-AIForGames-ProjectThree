using UnityEngine;

public class FlagInteraction : MonoBehaviour
{
    private bool inZone = false;
    private Flag currentFlag;
    public PlayerTankController player;

    public void SetInZone(bool value, Flag flag = null)
    {
        inZone = value;
        currentFlag = flag;

        if (!inZone)
        {
            // player left zone → stop raising + allow shooting again
            currentFlag?.StopRaising();
            player.Raising(false);
        }
    }

    private void Update()
    {
        if (inZone && currentFlag != null)
        {
            if (Input.GetKey(KeyCode.E))
            {
                currentFlag.StartRaising();
                player.Raising(true); // lock shooting
            }
            else
            {
                currentFlag.StopRaising();
                player.Raising(false); // unlock shooting
            }
        }
    }
}
