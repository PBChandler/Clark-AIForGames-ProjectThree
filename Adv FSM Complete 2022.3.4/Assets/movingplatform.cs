using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class movingplatform : MonoBehaviour
{
    public float start, end;
    public float speed;
    private bool atStart;
    public float check;
    public List<Transform> kids = new List<Transform>();
    void Start()
    {
        
    }

    // Update is called once per frame

    public void OnCollisionEnter(Collision collision)
    {
        if(!kids.Contains(collision.collider.gameObject.transform))
        {
            collision.collider.gameObject.transform.parent = transform;
            kids.Add(collision.collider.gameObject.transform);
        }
    }

   
    public void OnCollisionExit(Collision collision)
    {
        if (kids.Contains(collision.collider.gameObject.transform))
        {
            collision.collider.gameObject.transform.parent = null;
            kids.Remove(collision.collider.gameObject.transform);
        }
    }
    void Update()
    {
        foreach (Transform t in kids)
        {
            try
            {
                if (t.localPosition.magnitude > 1)
                {
                    t.parent = null;
                    kids.Remove(t);
                }
            }
            catch
            {
                kids.Remove(t);
            }
            
        }
        if(!atStart)
        {
            if(transform.position.z < start)
            {
                transform.position += new Vector3(0, 0, speed * Time.deltaTime);
            }
            else
            {
                atStart = true;
            }
        }
        else
        {
            if (transform.position.z > end)
            {
                transform.position += new Vector3(0, 0, -speed * Time.deltaTime);
            }
            else
            {
                atStart = false;
            }
        }
    }
}
