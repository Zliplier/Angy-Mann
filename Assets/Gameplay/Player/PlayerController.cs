using System;
using Gameplay.Combat;
using Gameplay.Player.PlayerState;
using Gameplay.Player.PlayerState.Action;
using UnityEngine;
using UnityEngine.Events;
using Zlipacket.Core.HSM;
using Zlipacket.Core.Input;
using Zlipacket.Core.Tools.Utilities;

namespace Gameplay.Player
{
    public class PlayerController : Singleton<PlayerController>
    {
        [Header("Components")]
        [field: SerializeField] public PlayerHealth playerHealth { get; private set; }
        [field: SerializeField] public PlayerMovement playerMovement { get; private set; }
        [field: SerializeField] public PlayerAnimator playerAnimator { get; private set; }
        [field: SerializeField] public PlayerCombat playerCombat { get; private set; }

        private StateMachine<PlayerController> stateMachine;
        public InputBuffer inputBuffer { get; } = new InputBuffer();

        public override void Awake()
        {
            base.Awake();

            stateMachine = new StateMachine<PlayerController>(this);
            
            //States
            stateMachine.AddState<PlayerIdle>();
            stateMachine.AddState<PlayerMove>();
            stateMachine.AddState<PlayerHurt>();
            stateMachine.AddState<PlayerDead>();
            
            //Transition
            stateMachine.AddState<PlayerNormal1>();
            stateMachine.AddState<PlayerNormal2>();
            stateMachine.AddState<PlayerNormal3>();
            stateMachine.AddTransition<PlayerIdle, PlayerNormal1>("Primary");
            stateMachine.AddTransition<PlayerMove, PlayerNormal1>("Primary");
            
            //Any Transition
            stateMachine.AddAnyTrigger<PlayerHurt>("Hurt");
            stateMachine.AddAnyTrigger<PlayerDead>("Dead");
            
            stateMachine.Start<PlayerIdle>();
        }

        public void NormalAction()
        {
            inputBuffer.Buffer("Primary", playerCombat.primaryBufferWindow, () => stateMachine.Fire("Primary"));
        }

        private void OnEnable()
        {
            playerHealth.onDead.AddListener(OnDead);
            
            playerCombat.onHit.AddListener(OnHit);
            playerCombat.onPrimary.AddListener(NormalAction);
        }

        private void OnDisable()
        {
            playerHealth.onDead.RemoveListener(OnDead);
            
            playerCombat.onHit.RemoveListener(OnHit);
            playerCombat.onPrimary.RemoveListener(NormalAction);
        }

        private void Update()
        {
            stateMachine.Tick(Time.deltaTime);
            
            inputBuffer.TryConsume("Primary", () => stateMachine.CanFire("Primary"));
        }

        private void FixedUpdate()
        {
            stateMachine.FixedTick();
        }

        private void LateUpdate()
        {
            stateMachine.LateTick();
        }

        private void OnHit(HitData hitData)
        {
            playerHealth.HealthPoints -= hitData.damage;
            if (!playerHealth.IsDead)
                stateMachine.Fire("Hurt");
        }

        private void OnDead()
        {
            stateMachine.Fire("Dead");
        }
    }
}