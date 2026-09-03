using Gameplay.Enemy.Dave.Action;
using Gameplay.Player;
using Zlipacket.Core.HSM;

namespace Gameplay.Enemy.Dave
{
    public class DaveChase : State<DaveController>
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Owner.daveAnimator.Play(nameof(DaveAnimationName.Move));
            Owner.daveMovement.SetEnableMovement(true);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            Owner.daveAnimator.Flip(Owner.daveMovement.IsFacingRight);
            
            Owner.daveMovement.MoveTo(PlayerController.Instance.root.transform.position);
            
            if (Owner.daveMovement.agent.remainingDistance <= Owner.daveMovement.agent.stoppingDistance)
            {
                Machine.ChangeState<DaveNormal>();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            
            Owner.daveMovement.SetEnableMovement(false);
        }
    }
}