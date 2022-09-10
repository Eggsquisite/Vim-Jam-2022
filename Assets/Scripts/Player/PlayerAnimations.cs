using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator anim;
    private string currentState;
    private RuntimeAnimatorController ac;

    void Awake() {
        anim = GetComponent<Animator>();
        ac = anim.runtimeAnimatorController;
    }

    // Animation Helper Functions ////////////////////////////////////////
    private void PlayAnimation(string newAnim) {
        AnimHelper.ChangeAnimationState(anim, ref currentState, newAnim);
    }
    private void ReplayAnimation(string newAnim) {
        AnimHelper.ReplayAnimation(anim, ref currentState, newAnim);
    }
    public float GetAnimationLength(string newAnim) {
        return AnimHelper.GetAnimClipLength(ac, newAnim);
    }

    public void Idle() { PlayAnimation(PlayAnims.IDLE); }
    public void Run() { PlayAnimation(PlayAnims.RUN); }
    public void Jump() { PlayAnimation(PlayAnims.JUMP); }
    public void Fall() { PlayAnimation(PlayAnims.FALL); }
    public void Land() { PlayAnimation(PlayAnims.LAND); }
    public void Attack() { PlayAnimation(PlayAnims.FIRE_ATTACK); }
    public void Hurt() { PlayAnimation(PlayAnims.HURT); }
    public void Death() { PlayAnimation(PlayAnims.DEATH); }

}
