namespace Flawless.Abstraction.Exceptions;

/// <summary>
/// When doing path transformation between platform and work, if given path is not inside of working directory, trigger
/// this exception to let user know given path is incorrect.
/// </summary>
public class PlatformPathNonManagedException(string issuePlatformPath, string platformWorkingDirectory)
    : FlawlessException(
        $"Platform path '{issuePlatformPath}' is not inside of working directory '{platformWorkingDirectory}'")
{
    public readonly string IssuePlatformPath = issuePlatformPath;
    
    public readonly string PlatformWorkingDirectory = platformWorkingDirectory;
}