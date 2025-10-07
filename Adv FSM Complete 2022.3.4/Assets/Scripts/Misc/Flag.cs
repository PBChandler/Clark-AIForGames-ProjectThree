using UnityEngine;
using System; 

public class Flag : MonoBehaviour
{
    [SerializeField] private float raiseHeight = 88f;
    [SerializeField] private float raiseSpeed = 5f;
    [SerializeField] private float startHeight = 8f;


    public bool IsBeingRaised { get; private set; }
    public bool IsFullMast { get; private set; }

    private bool raising = false;

    public event Action OnFullMast;  

    private void Start()
    {
        
        Vector3 pos = transform.position;
        pos.y = startHeight;
        transform.position = pos;
    }

    public void StartRaising()
    {
        raising = true;
        IsBeingRaised = true;
    }

    public void StopRaising()
    {
        raising = false;
        IsBeingRaised = false;
    }

    private void Update()
    {
        if (raising && !IsFullMast)
        {
            Vector3 pos = transform.position;
            pos.y = Mathf.MoveTowards(pos.y, raiseHeight, raiseSpeed * Time.deltaTime);
            transform.position = pos;

            if (Mathf.Approximately(pos.y, raiseHeight))
            {
                raising = false;
                IsBeingRaised = false;
                IsFullMast = true;
                OnFullMast?.Invoke();
            }
        }
    }

}
