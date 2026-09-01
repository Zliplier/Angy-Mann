using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Combat
{
    public class Health : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private float health = 100f;
        [SerializeField] private float maxHealth = 100f;

        [Header("Events")]
        public UnityEvent<float, float> onHealthChanged;
        public UnityEvent onDeath;

        [Header("Enable")]
        public bool invincibilityEnabled = false;

        public bool IsDead = false;
        
        public float HealthPoints
        {
            get => health;
            set
            {
                health = Mathf.Clamp(value, 0, maxHealth);
                onHealthChanged?.Invoke(health, maxHealth);
                if (health <= 0f && !IsDead)
                {
                    IsDead = true;
                    onDeath?.Invoke();
                }
            }
        }
        public float HealthPercentage => health / maxHealth;
    }
}