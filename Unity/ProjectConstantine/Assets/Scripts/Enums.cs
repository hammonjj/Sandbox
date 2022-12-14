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
}
