using System;

public class Enums
{
    [Flags]
    public enum LoggingMask
    {
        Debug = (1 << 0),
        Warning = (1 << 1),
        Error = (1 << 2),
        Verbose = (1 << 3),
    }

    public enum RoomOpenings
    {
        LR,
        LRB,
        LRT,
        LRTB,
        TB,
        LT,
        LB,
        RT,
        RB
    }

    public enum Ability
    {
        WallJump,
        DoubleJump,
        Dash,
        Bash,
        Float
    }

    public enum ItemType
    {
        Key
    }

    public enum Direction 
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    public enum Follow
    {
        Player,
        Enemy
    }
}
