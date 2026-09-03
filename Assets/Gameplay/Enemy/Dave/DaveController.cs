using System;
using DG.Tweening;
using Gameplay.Combat;
using Gameplay.Enemy.Dave.Action;
using UnityEngine;
using Zlipacket.Core.HSM;

namespace Gameplay.Enemy.Dave
{
    public class DaveController : MonoBehaviour
    {
        [Header("Components")]
        [field: SerializeField] public GameObject root { get; private set; }
        [field: SerializeField] public EnemyCombat daveCombat { get; private set; }
        [field: SerializeField] public EnemyMovement daveMovement { get; private set; }
        [field: SerializeField] public EnemyAnimator daveAnimator { get; private set; }
        [field: SerializeField] public Health daveHealth { get; private set; }
        
        private StateMachine<DaveController> stateMachine;

        private void Awake()
        {
            stateMachine = new StateMachine<DaveController>(this);
            
            //States
            stateMachine.AddState<DaveIdle>();
            stateMachine.AddState<DaveChase>();
            stateMachine.AddState<DaveHurt>();
            stateMachine.AddState<DaveDead>();
            stateMachine.AddState<DaveNormal>();
            
            //Transition
            stateMachine.AddTransition<DaveIdle, DaveNormal>("Normal");
            
            //Any Transition
            stateMachine.AddAnyTrigger<DaveHurt>("Hurt");
            stateMachine.AddAnyTrigger<DaveDead>("Dead");
            
            stateMachine.Start<DaveIdle>();
        }

        private void OnEnable()
        {
            daveHealth.onDead.AddListener(OnDead);
            daveCombat.onHit.AddListener(OnHit);
        }

        private void OnDisable()
        {
            daveHealth.onDead.RemoveListener(OnDead);
            daveCombat.onHit.RemoveListener(OnHit);
        }

        private void Update()
        {
            stateMachine.Tick(Time.deltaTime);
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
            daveHealth.HealthPoints -= hitData.damage;
            if (!daveHealth.IsDead)
                stateMachine.Fire("Hurt", force: true);
        }
        
        private void OnDead()
        {
            stateMachine.Fire("Dead");
        }

        public void Despawn()
        {
            Tween tween = root.transform.DOScale(0.1f, 0.25f);
            tween.OnComplete(() =>
            {
                Destroy(root);
            });
        }
    }

    public enum DaveAnimationName
    {
        Idle, 
        Move, 
        Normal, 
        Hurt, 
        Dead
    }
}
