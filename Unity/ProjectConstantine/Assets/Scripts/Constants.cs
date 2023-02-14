using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static class ObjectNames
    {
        public static string FiringPosition = "FiringPosition";
        public static string EnemyHealthBarCanvas = "EnemyHealthBarCanvas";
        public static string Health = "Health";
    }

    public static class Tags
    {
        public static string Enemy = "Enemy";
        public static string Player = "Player";
        public static string SceneStateManager = "SceneStateManager";
        public static string PlayerAttack = "PlayerAttack";
        public static string NextZoneText = "NextZoneText";
        public static string TransparentTerrain = "TransparentTerrain";
        public static string PauseMenu = "PauseMenu";
        public static string DoorManager = "DoorManager";
        public static string GameStateManager = "GameStateManager";
        public static string ZoneDoor = "ZoneDoor";
        public static string SpawnPoint = "SpawnPoint";
        public static string OneDoor = "OneDoor"; //Remove
        public static string TwoDoors = "TwoDoors"; //Remove
        public static string ThreeDoors = "ThreeDoors"; //Remove
        public static string DoorConfiguration = "DoorConfiguration"; //Remove doors and replace with this
        public static string RoomVariation = "RoomVariation";
        public static string OrbStartPos = "OrbStartPos";
        public static string DialogueBox = "DialogueBox";
        public static string PlayerBodyAttackTarget = "PlayerBodyAttackTarget";
        public static string PlayerHeathBar = "PlayerHeathBar";
        public static string NavMeshSurface = "NavMeshSurface";
        public static string PrimaryRing = "PrimaryRing";
        public static string SecondaryRing = "SecondaryRing";
        public static string RingManager = "RingManager";
        public static string Projectile = "Projectile";
        public static string PlayerEnergyBar = "PlayerEnergyBar";
    }

    public static class Enums
    {
        public enum EnemyType
        {
            Dynamic,
            Static
        }

        public enum UpgradeType
        {
            PrimaryAttack,
            PrimaryRing,
            PrimaryOrbSpawn,
            SecondaryAttack,
            SupportAbility,
            All
        }

        //This is dirty. Will need to abstract this into its own object
        public enum AttackUpgrade
        {
            None,
            IncreaseCritChance,
            ProjectilePassThrough,
            PrimaryIncreaseOrbs,
            SecondaryExplosiveOrb
        }

        public enum SupportAbilities
        {
            None,
            PersonalForceField,
            TimeSlowDown
        }

        public enum AttackType
        {
            Primary,
            Secondary
        }

        public enum Characters
        {
            None,
            Test,
            Player,
            Guide
        }

        public enum Scenes
        {
            None,
            WorldHub,

            //Zone 1
            Zone1_Any,
            Zone1_Shop_2,
            Zone1_Rest,
            Zone1_Rest_2,
            Zone1_Boss,
            Zone1_Story_2,
            Zone1_Start,
            Zone1_Chair,
            Zone1_Large,
            Zone1_Square,
            Zone1_HourGlass,
            Zone1_Square_Small,
            Zone1_FunkyHouse,
            Zone1_FunkyHouse_180,
            Zone1_Chair_90

            //Zone 2

            //Zone 3
        }

        public static readonly List<Scenes> Zone1FightRooms = new List<Scenes>()
        {
            Scenes.Zone1_Chair,
            Scenes.Zone1_Large,
            Scenes.Zone1_Square,
            Scenes.Zone1_HourGlass,
            Scenes.Zone1_Square_Small,
            Scenes.Zone1_FunkyHouse,
            Scenes.Zone1_FunkyHouse_180,
            Scenes.Zone1_Chair_90
        };

        public static readonly List<Scenes> Zone1EliteRooms = new List<Scenes>()
        {
        };

        public enum DoorType
        {
            Smart,
            Dumb
        }

        public enum SceneType
        {
            None,
            WorldHub,
            Shop,
            Rest,
            Boss,
            Story,
            Fight,
            Elite,
        }

        public enum FightType
        {
            None,
            Normal,
            Elite
        }

        public enum RoomType
        {
            None,
            Normal,
            Elite,
            Rest,
            Shop,
            Story
        }

        public enum RoomReward
        {
            None,
            Combat,
            Cosmetic,
            Currency,
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

