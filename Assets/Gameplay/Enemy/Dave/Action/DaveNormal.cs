using UnityEngine;
using Zlipacket.Core.HSM;
using Zlipacket.Core.Tools.Utilities;

namespace Gameplay.Enemy.Dave.Action
{
    public class DaveNormal : State<DaveController>
    {
        private Timer timer;
        
        public override void OnEnter()
        {
            base.OnEnter();
            AnimatorClipInfo clipInfo = Owner.daveAnimator.Play(nameof(DaveAnimationName.Normal));
            
            //Owner.playerCombat.StartIFrame(Owner.playerCombat.damageIFrameWindow);
            
            timer = new Timer(clipInfo.clip.length);
            timer.OnTimerComplete += () =>
            {
                Machine.ChangeState<DaveIdle>();
            };
            timer.Start();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            timer.Tick(Time.deltaTime);
        }

        public override void OnExit()
        {
            base.OnExit();
            
            Owner.daveCombat.SetActiveAllHurtboxes(false);
            
            timer?.Stop();
        }
    }
}