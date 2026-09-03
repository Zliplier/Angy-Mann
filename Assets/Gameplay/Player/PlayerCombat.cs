using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Combat;
using InputSO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Zlipacket.Core.Input;
using Zlipacket.Core.Tools.Utilities;

namespace Gameplay.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Configs")]
        public float primaryBufferWindow = 0.2f;
        public float damageIFrameWindow = 1f;
        
        [Header("Input")]
        [field: SerializeField] public PlayerMapContext playerInputMap { get; private set; }

        [Header("Components")]
        [SerializeField] private List<Hitbox> hitboxes;
        [SerializeField] private List<HurtBox> hurtBoxes;

        [Header("Events")]
        public UnityEvent<HitData> onHit;
        public UnityEvent onPrimary;
        
        [Header("Enable")]
        public bool combatEnabled = true;
        
        private Timer iFrameTimer;
        
        private void OnEnable()
        {
            playerInputMap.OnPrimary += Primary;
        }
        
        private void OnDisable()
        {
            playerInputMap.OnPrimary -= Primary;
        }

        private void Start()
        {
            foreach (Hitbox hitbox in hitboxes)
                hitbox?.onHitRecieved.AddListener((hitData => onHit.Invoke(hitData)));
        }

        private void Primary(InputAction.CallbackContext context)
        {
            if (combatEnabled && context.started)
                onPrimary?.Invoke();
        }

        private void Update()
        {
            iFrameTimer?.Tick(Time.deltaTime);
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
        
        public void StartIFrame(float time)
        {
            if (iFrameTimer != null && iFrameTimer.IsRunning && iFrameTimer.TimeRemaining > time)
                return;
            
            SetActiveAllHitboxes(false);
            iFrameTimer = new Timer(time);
            iFrameTimer.OnTimerComplete += () => SetActiveAllHitboxes(true);
            iFrameTimer.Start();
        }
    }
}