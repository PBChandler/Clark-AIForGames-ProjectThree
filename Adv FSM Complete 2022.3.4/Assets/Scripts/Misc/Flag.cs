using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private float raiseHeight = 88f;
    [SerializeField] private float raiseSpeed = 5f;
    private bool raising = false;

    public void StartRaising()
    {
        raising = true;
    }

    private void Update()
    {
        if (raising)
        {
            Vector3 pos = transform.position;
            pos.y = Mathf.MoveTowards(pos.y, raiseHeight, raiseSpeed * Time.deltaTime);
            transform.position = pos;
        }
    }

    public void OnAnimatorIK(int layerIndex)
    {
        
    }
}

