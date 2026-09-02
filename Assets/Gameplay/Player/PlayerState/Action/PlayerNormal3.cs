using UnityEngine;
using Zlipacket.Core.HSM;
using Zlipacket.Core.Tools.Utilities;

namespace Gameplay.Player.PlayerState.Action
{
    public class PlayerNormal3 : State<PlayerController>
    {
        private Timer timer;
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            AnimatorClipInfo clipInfo = Owner.playerAnimator.Play(nameof(PlayerAnimationName.Normal3));

            Owner.playerMovement.moveEnabled = false;
            Owner.playerMovement.jumpEnabled = false;
            
            timer = new Timer(clipInfo.clip.length);
            timer.OnTimerComplete += () =>
            {
                Machine.ChangeState<PlayerIdle>();
            };
            timer.Start();
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            
            timer?.Tick(Time.deltaTime);
        }

        public override void OnExit()
        {
            base.OnExit();
            
            Owner.playerMovement.moveEnabled = true;
            Owner.playerMovement.jumpEnabled = true;
            
            Owner.playerCombat.SetActiveAllHurtboxes(false);

            timer?.Stop();
        }
    }
}