using UnityEngine;
using Zlipacket.Core.HSM;
using Zlipacket.Core.Tools.Utilities;

namespace Gameplay.Enemy.Dave
{
    public class DaveHurt : State<DaveController>
    {
        private Timer timer;
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            AnimatorClipInfo clipInfo = Owner.daveAnimator.Play(nameof(DaveAnimationName.Hurt));
            
            Owner.daveAnimator.SetFaceText("> • <");
            
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
            
            Owner.daveAnimator.SetFaceText("•́ - •̀");
            timer.Stop();
        }
    }
}