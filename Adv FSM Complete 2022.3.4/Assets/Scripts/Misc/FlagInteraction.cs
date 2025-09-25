using UnityEngine;

public class FlagInteraction : MonoBehaviour
{
    private bool inZone = false;
    private Flag currentFlag;

    public void SetInZone(bool value, Flag flag = null)
    {
        inZone = value;
        currentFlag = flag;
    }

    private void Update()
    {
        if (inZone && Input.GetKeyDown(KeyCode.E)) 
        {
            Debug.Log("Raising flag!");
            currentFlag?.StartRaising();
            // Lock player shooting
            GetComponent<PlayerTankController>().canShoot = false;
        }

        else if (inZone)
        {
            // Unlock player shooting
            GetComponent<PlayerTankController>().canShoot = true;
        }
    }
}
