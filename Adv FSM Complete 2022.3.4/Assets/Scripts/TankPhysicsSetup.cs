using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class TankPhysicsSetup : MonoBehaviour
{
    private void Awake()
    {
        ConfigureRigidbody();
        ConfigureNavMeshAgent();
    }

    private void ConfigureRigidbody()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        
        rb.mass = 5f;
        rb.linearDamping = 2f;
        rb.angularDamping = 5f;
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        
        rb.constraints = RigidbodyConstraints.FreezeRotationX | 
                        RigidbodyConstraints.FreezeRotationZ;
    }

    private void ConfigureNavMeshAgent()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        
        agent.speed = 30f;
        agent.angularSpeed = 120f;
        agent.acceleration = 8f;
        agent.stoppingDistance = 1f;
        agent.autoBraking = true;
        agent.radius = 2f;
        agent.height = 5f;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        agent.avoidancePriority = 50;
        agent.autoTraverseOffMeshLink = true;
        agent.autoRepath = true;
        
        agent.updateRotation = true;
        agent.updatePosition = true;
    }
}
