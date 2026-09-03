using UnityEngine;
using Zlipacket.Core.HSM;
using Zlipacket.Core.Tools.Utilities;

namespace Gameplay.Enemy.Dave
{
    public class DaveDead : State<DaveController>
    {
        private float despawnTime = 10f;
        
        private Timer despawnTimer;
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            Owner.daveAnimator.Play(nameof(DaveAnimationName.Dead));
            Owner.daveAnimator.SetFaceText("+- +");
            
            Owner.daveCombat.SetActiveAllHitboxes(false);
            Owner.daveCombat.SetActiveAllHurtboxes(false);
            
            despawnTimer = new Timer(despawnTime);
            despawnTimer.OnTimerComplete += () => Owner.Despawn();
            despawnTimer.Start();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            despawnTimer?.Tick(Time.deltaTime);
        }
    }
}