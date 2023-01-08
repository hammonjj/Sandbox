using UnityEngine;

public static class Constants
{
    //GameObject Tags
    public const string Player = "Player";

    //Animation IDs
    public static int AnimID_Speed = Animator.StringToHash("Speed");
    public static int AnimID_Grounded = Animator.StringToHash("Grounded");
    public static int AnimID_Jump = Animator.StringToHash("Jump");
    public static int AnimID_FreeFall = Animator.StringToHash("FreeFall");
    public static int AnimID_MotionSpeed = Animator.StringToHash("MotionSpeed");

    //Mutant
    public static int AnimID_MutantAttack = Animator.StringToHash("MutantAttack");

    //Scenes
    public static string TrainingGround = "Training Ground";
}

