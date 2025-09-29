using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCTankController : AdvancedFSM
{
    public GameObject Bullet;
    private int health;
    new private Rigidbody rigidbody;

    [System.Serializable]
    public struct StateColor
    {
        public FSMStateID state;
        public Color color;
    }

    public StateColor[] stateColors;
    private Dictionary<FSMStateID, Color> _colorMap;
    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
    private FSMStateID _curState;

    public FSMStateID CurState
    {
        get { return _curState; }
        set
        {
            if (_curState == value) return;
            _curState = value;
            UpdateStateColor();
        }
    }

    protected override void Initialize()
    {
        health = 100;
        elapsedTime = 0.0f;
        shootRate = 2.0f;

        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;
        rigidbody = GetComponent<Rigidbody>();
        if (!playerTransform)
            print("Player doesn't exist.. Please add one with Tag named 'Player'");

        turret = gameObject.transform.GetChild(0).transform;
        bulletSpawnPoint = turret.GetChild(0).transform;

        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
        _colorMap = new Dictionary<FSMStateID, Color>();
        foreach (var stateColor in stateColors) _colorMap[stateColor.state] = stateColor.color;

        ConstructFSM();
    }

    protected override void FSMUpdate()
    {
        elapsedTime += Time.deltaTime;
        CurState = CurrentState.ID;
    }

    protected override void FSMFixedUpdate()
    {
        CurrentState.Reason(playerTransform, transform);
        CurrentState.Act(playerTransform, transform);
    }

    private void UpdateStateColor()
    {
        if (_renderer == null || !_colorMap.ContainsKey(CurState)) return;
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", _colorMap[CurState]);
        _renderer.SetPropertyBlock(_propBlock);
    }

    public void SetTransition(Transition t)
    {
        PerformTransition(t);
    }

    private void ConstructFSM()
    {
        pointList = GameObject.FindGameObjectsWithTag("WandarPoint");
        Transform[] waypoints = new Transform[pointList.Length];
        int i = 0;
        foreach (GameObject obj in pointList)
        {
            waypoints[i] = obj.transform;
            i++;
        }

        PatrolState patrol = new PatrolState(waypoints);
        patrol.AddTransition(Transition.SawPlayer, FSMStateID.Chasing);
        patrol.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        ChaseState chase = new ChaseState(waypoints);
        chase.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
        chase.AddTransition(Transition.ReachPlayer, FSMStateID.Attacking);
        chase.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        AttackState attack = new AttackState(waypoints);
        attack.AddTransition(Transition.LostPlayer, FSMStateID.Patrolling);
        attack.AddTransition(Transition.SawPlayer, FSMStateID.Chasing);
        attack.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        DeadState dead = new DeadState();
        dead.AddTransition(Transition.NoHealth, FSMStateID.Dead);

        AddFSMState(patrol);
        AddFSMState(chase);
        AddFSMState(attack);
        AddFSMState(dead);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            health -= 50;
            if (health <= 0)
            {
                Debug.Log("Switch to Dead State");
                SetTransition(Transition.NoHealth);
                Explode();
            }
        }
    }

    protected void Explode()
    {
        float rndX = Random.Range(10.0f, 30.0f);
        float rndZ = Random.Range(10.0f, 30.0f);
        for (int i = 0; i < 3; i++)
        {
            rigidbody.AddExplosionForce(10000.0f, transform.position - new Vector3(rndX, 10.0f, rndZ), 40.0f, 10.0f);
            rigidbody.linearVelocity = transform.TransformDirection(new Vector3(rndX, 20.0f, rndZ));
        }
        Destroy(gameObject, 1.5f);
    }

    public void ShootBullet()
    {
        if (elapsedTime >= shootRate)
        {
            Instantiate(Bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            elapsedTime = 0.0f;
        }
    }
}
