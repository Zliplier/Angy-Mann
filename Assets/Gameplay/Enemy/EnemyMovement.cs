using System;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        [Header("Components")]
        [field: SerializeField] public NavMeshAgent agent { get; private set; }
        [SerializeField] private Rigidbody rb;
        
        public bool IsFacingRight { get; private set; } = true;

        public Vector3 velocity => agent.desiredVelocity + additionalVelocity;
        
        public Vector3 additionalVelocity { get; private set; } = Vector3.zero;
        
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
            //Debug.Log(agent.desiredVelocity);
        }
        
        private void LateUpdate()
        {
            agent.nextPosition = rb.position;
        }

        private void ApplyMovement()
        {
            rb.linearVelocity = velocity;
        }

        public void MoveTo(Vector3 destination)
        {
            agent.SetDestination(destination);
        }

        public void SetEnableMovement(bool enable)
        {
            agent.isStopped = !enable;
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