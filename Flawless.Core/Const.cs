namespace Flawless.Core;

public static class Const
{
    public const string RootDirectory = ".flawless";
    
    public static readonly string DepotDirectory = Path.Combine(RootDirectory, "depot");
    
    public static readonly string CommitDirectory = Path.Combine(RootDirectory, "commit");
    
    public static readonly string TrackerFile = Path.Combine(RootDirectory, "tracker");
}