using UnityEngine;

public static class Constants
{
    public static class Tags
    {
        public static string Enemy = "Enemy";
        public static string Player = "Player";
        public static string GameManager = "GameManager";
        public static string SceneStateManager = "SceneStateManager";
        public static string PlayerAttack = "PlayerAttack";
        public static string NextZoneText = "NextZoneText";
        public static string TransparentTerrain = "TransparentTerrain";
        public static string PauseMenu = "PauseMenu";
        public static string DoorManager = "DoorManager";
        public static string GameStateManager = "GameStateManager";
        public static string ZoneDoor = "ZoneDoor";
        public static string SpawnPoint = "SpawnPoint";
        public static string OneDoor = "OneDoor";
        public static string TwoDoors = "TwoDoors";
        public static string ThreeDoors = "ThreeDoors";
    }

    public static class Enums
    {
        public enum Scenes
        {
            None,
            TrainingGround,

            Zone1_Any,
            Zone1_Shop,
            Zone1_Rest,
            Zone1_Boss,
            Zone1_Story,
            Zone1_Start,
            Zone1_OneExit,
            Zone1_TwoExits,
            Zone1_ThreeExits
        }

        public enum SceneType
        {
            None,

            Shop,
            Rest,
            Boss,
            Story,
            OneExit,
            TwoExits,
            ThreeExits
        }

        public enum FightType
        {
            None,
            Normal,
            Elite
        }

        public enum RoomReward
        {
            None,
            Combat,
            Cosmetic,
            Currency,
            Shop,
            Story
        }

        public enum Zones
        {
            None,
            Zone1,
            Zone2,
            Zone3
        }
    }

    public static class Layers
    {
        public static string Ground = "Ground";
    }

    public static class Animations
    {
        //Animation IDs
        public static int AnimID_Speed = Animator.StringToHash("Speed");
        public static int AnimID_Grounded = Animator.StringToHash("Grounded");
        public static int AnimID_Jump = Animator.StringToHash("Jump");
        public static int AnimID_FreeFall = Animator.StringToHash("FreeFall");
        public static int AnimID_MotionSpeed = Animator.StringToHash("MotionSpeed");

        //Mutant
        public static int AnimID_MutantAttack = Animator.StringToHash("MutantAttack");
    }
}

