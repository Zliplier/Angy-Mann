using Zlipacket.Core.HSM;

namespace Gameplay.Enemy.Dave
{
    public class DaveDead : State<DaveController>
    {
        public override void OnEnter()
        {
            base.OnEnter();
            
            Owner.daveAnimator.Play(nameof(DaveAnimationName.Dead));
            Owner.daveAnimator.SetFaceText("+- +");
            
            Owner.daveCombat.SetActiveAllHitboxes(false);
            Owner.daveCombat.SetActiveAllHurtboxes(false);
        }
    }
}