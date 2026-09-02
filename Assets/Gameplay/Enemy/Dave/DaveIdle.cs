using UnityEngine;
using Zlipacket.Core.HSM;
using Zlipacket.Core.Tools.Utilities;

namespace Gameplay.Enemy.Dave
{
    public class DaveIdle : State<DaveController>
    {
        private float minIdleTime = 0.5f;
        private float maxIdleTime = 1f;

        private Timer idleTimer;
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            Owner.daveAnimator.Play(nameof(DaveAnimationName.Idle));
            
            idleTimer = new Timer(Random.Range(minIdleTime, maxIdleTime));
            idleTimer.Start();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            idleTimer.Tick(Time.deltaTime);
        }

        public override void OnExit()
        {
            base.OnExit();
            idleTimer.Stop();
        }
    }
}