using System;
using InputSO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Zlipacket.Core.Input;

namespace Gameplay.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Configs")]
        public float primaryBufferWindow = 0.2f;
        
        [Header("Input")]
        [SerializeField] private PlayerMapContext playerInputMap;

        [Header("Events")]
        public UnityEvent OnPrimary;
        
        [Header("Enable")]
        public bool combatEnabled = true;
        
        private void OnEnable()
        {
            playerInputMap.OnPrimary += Primary;
        }

        private void OnDisable()
        {
            playerInputMap.OnPrimary -= Primary;
        }

        private void Primary(InputAction.CallbackContext context)
        {
            if (combatEnabled && context.started)
                OnPrimary?.Invoke();
        }
    }
}