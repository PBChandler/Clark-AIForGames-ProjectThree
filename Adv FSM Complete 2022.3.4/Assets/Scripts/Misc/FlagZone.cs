using UnityEngine;

public class FlagZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered flag zone");
            
            other.GetComponent<FlagInteraction>()?.SetInZone(true, GetComponent<Flag>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left flag zone");
            other.GetComponent<FlagInteraction>()?.SetInZone(false, GetComponent<Flag>());
        }
    }
}
