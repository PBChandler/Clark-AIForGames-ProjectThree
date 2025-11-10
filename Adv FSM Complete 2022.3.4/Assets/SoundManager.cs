    using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public PlayerTankController Player;
    public float listenerRange;
    public struct SoundResult
    {
        public Vector3 location;
        public int priority;
        public string tag;
    }
    
    public delegate void SoundResultCallback(SoundResult result);
    public SoundResultCallback dg_Publisher;

    public void Start()
    {
        dg_Publisher += Dummy;
    }

    public void Dummy(SoundResult result) { }
    public void Update()
    {
        Listen();
    }
    public void Listen()
    {
        if(Player.isMoving && Vector3.Distance(transform.position, Player.transform.position) <= listenerRange)
        {
            SoundResult result = new SoundResult();
            result.priority = 0;
            result.tag = "PlayerMoving";
            result.location = Player.transform.position;
            dg_Publisher(result);
        }
    }
}
