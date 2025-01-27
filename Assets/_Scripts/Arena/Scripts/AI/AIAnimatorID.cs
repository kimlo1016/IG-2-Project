using UnityEngine;

public static class AIAnimatorID
{
    // 기본자세 모음
    public static readonly int isIdle = Animator.StringToHash("isIdle");

    // 기본 이동 모음
    public static readonly int isRun = Animator.StringToHash("isRun");

    // 기본공격 모음
    public static readonly int isAttack = Animator.StringToHash("isAttack");
    public static readonly int isAttack1 = Animator.StringToHash("isAttack1");
    public static readonly int isAttack2 = Animator.StringToHash("isAttack2");
    public static readonly int isAttack3 = Animator.StringToHash("isAttack3");

    // 스킬 모음
    public static readonly int isSkill = Animator.StringToHash("isSkill");

    // 피격 모음
    public static readonly int isDamage = Animator.StringToHash("isDamage");

    // 사망 모음
    public static readonly int isDeath = Animator.StringToHash("isDeath");
}


//        HighClassKnight,
//        HighClassAdventurer,
//        FireWizard,
//        IceWizard,