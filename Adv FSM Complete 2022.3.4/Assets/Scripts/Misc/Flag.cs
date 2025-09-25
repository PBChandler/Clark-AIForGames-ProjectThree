using UnityEngine;
using System; 

public class Flag : MonoBehaviour
{
    [SerializeField] private float raiseHeight = 88f;
    [SerializeField] private float raiseSpeed = 5f;
    [SerializeField] private float startHeight = 8f;

    private bool raising = false;

    public event Action OnFullMast;  

    private void Start()
    {
        
        Vector3 pos = transform.position;
        pos.y = startHeight;
        transform.position = pos;
    }

    private void Update()
    {
        if (raising)
        {
            Vector3 pos = transform.position;
            pos.y = Mathf.MoveTowards(pos.y, raiseHeight, raiseSpeed * Time.deltaTime);
            transform.position = pos;

            if (Mathf.Approximately(pos.y, raiseHeight))
            {
                raising = false;
                OnFullMast?.Invoke(); 
            }
        }
    }

    public void StartRaising()
    {
        raising = true;
    }

    public void StopRaising()
    {
        raising = false;
    }
}
