using System;
using System.Collections.Generic;
using Gameplay.Combat;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Enemy
{
    public class EnemyCombat : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private List<Hitbox> hitboxes;
        [SerializeField] private List<HurtBox> hurtBoxes;
        
        [Header("Events")]
        public UnityEvent<HitData> onHit;

        private void Start()
        {
            foreach (Hitbox hitbox in hitboxes)
                hitbox?.onHitRecieved.AddListener((hitData => onHit.Invoke(hitData)));
        }

        public void SetActiveAllHitboxes(bool active = true)
        {
            foreach (Hitbox hitbox in hitboxes)
                hitbox?.gameObject.SetActive(active);
        }
        
        public void SetActiveAllHurtboxes(bool active = false)
        {
            foreach (HurtBox hurtbox in hurtBoxes)
                hurtbox?.gameObject.SetActive(active);
        }
    }
}