using System;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [Header("Configs")]
        public float deceleration;
        
        [Header("Components")]
        [field: SerializeField] public NavMeshAgent agent { get; private set; }
        [SerializeField] private Rigidbody rb;

        public bool IsFacingRight = true;

        public Vector3 velocity => agent.desiredVelocity + additionalVelocity;
        
        public Vector3 additionalVelocity = Vector3.zero;
        
        private void Start()
        {
            agent.updatePosition = false;
            agent.updateRotation = false;
        }

        private void Update()
        {
            TurnCheck();
        }

        private void FixedUpdate()
        {
            HandleImpulse();

            ApplyMovement();
        }

        private void ApplyMovement()
        {
            rb.linearVelocity = velocity;
        }

        public void MoveTo(Vector3 destination)
        {
            agent.SetDestination(destination);
        }

        public void Stop()
        {
            agent.isStopped = true;
        }

        private void TurnCheck()
        {
            if (agent.desiredVelocity.x > 0)
                IsFacingRight = true;
            else if (agent.desiredVelocity.x < 0)
                IsFacingRight = false;
        }

        private void HandleImpulse()
        {
            additionalVelocity = Vector3.Lerp(
                additionalVelocity, 
                Vector3.zero, 
                agent.acceleration * Time.fixedDeltaTime);
            agent.velocity += additionalVelocity;
        }

        public void AddImpulse(Vector3 impulse)
        {
            additionalVelocity += impulse;
        }
    }
}