using System;
using System.Linq;
using UnityEngine;
using Zlipacket.Core.Tools.Extension;

namespace Gameplay.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject root;
        
        public AnimatorClipInfo GetCurrentClip(int layerIndex = 0)
            => animator.GetCurrentAnimatorClipInfo(layerIndex)[0];
        
        public void Flip(bool isRight)
        {
            root.transform.localScale = root.transform.localScale.Insert(x: Math.Abs(root.transform.localScale.x) * (isRight ? 1 : -1));
        }

        public AnimatorClipInfo Play(string animationName, float speed = 1f, int layerIndex = 0)
        {
            animator.speed = speed;

            if (animator.HasState(layerIndex, Animator.StringToHash(animationName)))
            {
                animator.Play(animationName, layerIndex);
                animator.Update(0f); 
            }
            else
            {
                Debug.LogError($"Animation {animationName} not found.");
                return default;
            }
            
            AnimatorClipInfo clipInfo = animator.GetCurrentAnimatorClipInfo(layerIndex).FirstOrDefault(a => a.clip.name == animationName);
            return clipInfo;
        }
        
        public AnimatorClipInfo PlayByDuration(string animationName, float duration = 1f, int layerIndex = 0)
        {
            AnimatorClipInfo clipInfo = Play(animationName, layerIndex: layerIndex);
            
            animator.speed = clipInfo.clip.length / duration;
            
            return clipInfo;
        }
    }

    public enum PlayerAnimationName
    {
        Idle, 
        Move, 
        Normal
    }
}