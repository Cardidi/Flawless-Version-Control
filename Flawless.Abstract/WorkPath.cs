using System.Text;
using Flawless.Abstraction.Exceptions;

namespace Flawless.Abstraction;

/// <summary>
/// A platform-independent path system for version controlling which provides a safe and easy used path calculation.
/// Some of those function is a wrapper of <see cref="Path"/> with ensure of correction.
/// </summary>
public static class WorkPath
{
    
    /* What is a valid work path?
     *
     * A worm path is something like this:
     * 
     *     root/subfolder1/subfolder2/file.txt
     *
     * 1. Should being trim at anytime
     * 2. Every separated name should being trim at anytime
     * 3. Should not start or end with '/'
     * 4. Directory separator is '/'
     * 5. Can be converted into and from any platform and self to be platform standalone
     * 6. Without any irregular or invisible character.
     */
    
    private static readonly char[] InvalidPathChars = [
        '\"', '\\', '<', '>', '|', '?', '*', ':', '^', '%',
        (char)0, (char)1, (char)2, (char)3, (char)4, (char)5, (char)6, (char)7, (char)8,
        (char)9, (char)10, (char)11, (char)12, (char)13, (char)14, (char)15, (char)16,
        (char)17, (char)18, (char)19, (char)20, (char)21, (char)22, (char)23, (char)24,
        (char)25, (char)26, (char)27, (char)28, (char)29, (char)30, (char)31];

    private static readonly HashSet<char> InvalidPathCharsQuickTest = new(InvalidPathChars);
    
    public const char DirectorySeparatorChar = '/';

    /// <summary>
    /// Check if path contains any invalid characters. Do not guarantee path is valid. 
    /// </summary>
    /// <param name="workPath">Tested path.</param>
    /// <returns>If there has no invalid path, return true.</returns>
    public static bool IsPathHasInvalidPathChars(string workPath)
    {
        foreach (var c in workPath)
        {
            if (!InvalidPathCharsQuickTest.Contains(c)) return true;
        }

        return false;
    }
    
    /// <summary>
    /// Get an array of invalid characters. Do not guarantee path is valid.
    /// </summary>
    /// <returns>Array of invalid characters</returns>
    public static char[] GetInvalidPathChars()
    {
        var r = new char[InvalidPathChars.Length];
        InvalidPathChars.CopyTo(r, 0);
        return r;
    }

    /// <summary>
    /// Convert a work path into current platform file system path. Do not guarantee path is valid.
    /// </summary>
    /// <param name="workPath">A work path in this repository. This path will not do any validation.</param>
    /// <param name="platformWorkingDirectory">The root path of repository in platform path.</param>
    /// <returns>A platform path mapping from work path.</returns>
    public static string ToPlatformPath(string workPath, string platformWorkingDirectory)
    {
        var sb = new StringBuilder(workPath.Length + platformWorkingDirectory.Length);
        sb.Append(platformWorkingDirectory);
        
        if (!Path.EndsInDirectorySeparator(platformWorkingDirectory)) sb.Append(Path.DirectorySeparatorChar);
        foreach (var c in workPath)
            sb.Append(c == DirectorySeparatorChar ? Path.DirectorySeparatorChar : c);
        
        return sb.ToString();
    }

    /// <summary>
    /// Convert a platform-specific file system path into work path. Do not guarantee path is valid.
    /// </summary>
    /// <param name="platformPath">A platform path.</param>
    /// <param name="platformWorkingDirectory">The root path of repository in platform path.</param>
    /// <returns>Work path mapping from platform path .</returns>
    /// <exception cref="PlatformPathNonManagedException">If platform path is not a sub entity in platform working path,
    /// this path will not being managed. So make error.</exception>
    public static string FromPlatformPath(string platformPath, string platformWorkingDirectory)
    {
        var workPath = Path.GetRelativePath(platformWorkingDirectory, platformPath);
        if (workPath == ".") return string.Empty;
        if (workPath == platformPath) // If not share same root, it will return platform path. So compare it directly.
            throw new PlatformPathNonManagedException(platformPath, platformWorkingDirectory);
        
        var sb = new StringBuilder(workPath.Length);
        foreach (var c in workPath)
        {
            var isSplitChar = c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar;
            if (sb.Length == 0 && isSplitChar) continue;

            sb.Append(isSplitChar ? DirectorySeparatorChar : c);
        }
        
        return sb.ToString();
    }
    
    /// <summary>
    /// Split work path into path vector.
    /// </summary>
    /// <param name="workPath">The path will being split.</param>
    /// <param name="result">The list to store result (Non-allocate)</param>
    /// <returns>The count of added elements</returns>
    /// <exception cref="ArgumentNullException">Argument is null</exception>
    /// <exception cref="ArgumentException">Work path is invalid</exception>
    public static int GetPathVector(string workPath, List<string> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (string.IsNullOrWhiteSpace(workPath)) 
            throw new ArgumentNullException(nameof(workPath), "Not a valid work path!");
        
        if (workPath[0] == DirectorySeparatorChar) 
            throw new ArgumentException("Work path cannot start with a DirectorySeparatorChar!", nameof(workPath));

        var start = 0;
        var end = 0;
        var count = 0;
        for (var i = 0; i <= workPath.Length; i++)
        {

            if (i < workPath.Length)
            {
                var c = workPath[i];
                if (InvalidPathChars.Contains(c))
                    throw new ArgumentException("Invalid work path character: " + c);

                if (c != DirectorySeparatorChar)
                {
                    end = i;
                    continue;
                }
            }

            if (start >= end) throw new ArgumentException("Work path contains empty vector!");
            if (workPath[end] == ' ' || workPath[start] == ' ') 
                throw new ArgumentException("Work path vector can not start or end with a space!");

            result.Add(workPath.Substring(start, end - start + 1));
            count++;
            start = i + 1;
        }

        return count;
    }
    
    /// <summary>
    /// Split work path into path vector.
    /// </summary>
    /// <param name="workPath">The path will being split.</param>
    /// <returns>Enumerable of vector</returns>
    /// <exception cref="ArgumentNullException">Argument is null</exception>
    /// <exception cref="ArgumentException">Work path is invalid</exception>
    public static IEnumerable<string> GetPathVector(string workPath)
    {
        if (string.IsNullOrWhiteSpace(workPath)) 
            throw new ArgumentNullException(nameof(workPath), "Not a valid work path!");
        
        if (workPath[0] == DirectorySeparatorChar) 
            throw new ArgumentException("Work path cannot start with a DirectorySeparatorChar!", nameof(workPath));

        var start = 0;
        var end = 0;
        for (var i = 0; i <= workPath.Length; i++)
        {

            if (i < workPath.Length)
            {
                var c = workPath[i];
                if (InvalidPathChars.Contains(c))
                    throw new ArgumentException("Invalid work path character: " + c);

                if (c != DirectorySeparatorChar)
                {
                    end = i;
                    continue;
                }
            }

            if (start >= end) throw new ArgumentException("Work path contains empty vector!");
            if (workPath[end] == ' ' || workPath[start] == ' ') 
                throw new ArgumentException("Work path vector can not start or end with a space!");

            yield return workPath.Substring(start, end - start + 1);
            start = i + 1;
        }
    }
    
    /// <summary>
    /// Check work path is legal but not ensure that existed in platform.
    /// </summary>
    /// <param name="workPath">The path will being tested.</param>
    /// <returns>True when path is valid. If you need more details, consider use
    /// <see cref="WorkPath.GetPathVector(string)"/>.</returns>
    public static bool IsPathValid(string workPath)
    {
        if (string.IsNullOrWhiteSpace(workPath) || workPath[0] == DirectorySeparatorChar) return false;

        var start = 0;
        var end = 0;
        for (var i = 0; i <= workPath.Length; i++)
        {

            if (i < workPath.Length)
            {
                var c = workPath[i];
                if (InvalidPathChars.Contains(c)) return false;

                if (c != DirectorySeparatorChar)
                {
                    end = i;
                    continue;
                }
            }

            if (start >= end || end >= workPath.Length || start >= workPath.Length || 
                workPath[end] == ' ' || workPath[start] == ' ') 
                return false;

            start = i + 1;
        }

        return true;
    }

    /// <summary>
    /// Check if last vector has a valid extension.
    /// </summary>
    /// <param name="workPath">Targeting work path</param>
    /// <returns>Is valid.</returns>
    public static bool HasExtension(string workPath)
    {
        for (var i = workPath.Length - 1; i >= 0; i--)
        {
            var c = workPath[i];
            if (c == DirectorySeparatorChar) break;
            if (c == '.') return true;
        }

        return false;
    }
    
    /// <summary>
    /// Get the last vector extension.
    /// </summary>
    /// <param name="workPath">Targeting work path</param>
    /// <returns>The name of last vector extension. If the last vector is empty or no valid extension existed,
    /// return empty.</returns>
    public static string GetExtension(string workPath)
    {
        for (var i = workPath.Length - 1; i >= 0; i--)
        {
            var c = workPath[i];
            if (c == DirectorySeparatorChar) break;
            if (c == '.') return i + 1 >= workPath.Length ? String.Empty : workPath.Substring(i + 1);
        }

        return string.Empty;
    }

    /// <summary>
    /// Change the last vector extension.
    /// </summary>
    /// <param name="workPath">Targeting work path</param>
    /// <param name="extension">Targeting extension, null means clean the extension</param>
    /// <returns>Modified path.</returns>
    public static string ChangeExtension(string workPath, string? extension)
    {
        var hasExtension = extension != null;
        for (var i = workPath.Length - 1; i >= 0; i--)
        {
            var c = workPath[i];
            if (c == DirectorySeparatorChar) break;
            if (c == '.') return workPath.Substring(0, hasExtension ? i + 1 : i) + extension;
        }

        return hasExtension ? workPath + "." + extension : workPath;
    }
    
    /// <summary>
    /// Get the last vector name.
    /// </summary>
    /// <param name="workPath">Targeting work path</param>
    /// <returns>The name of last vector name. If the last vector is empty, return empty.</returns>
    public static string GetName(string workPath)
    {
        var start = workPath.Length - 1;
        var length = 0;
        for (var i = workPath.Length - 1; i >= 0; i--)
        {
            var c = workPath[i];
            if (c == DirectorySeparatorChar)
            {
                start = i + 1;
                break;
            }

            start = i;
            length += 1;
        }

        return start < 0 ? string.Empty :  workPath.Substring(start, length);
    }
    
    /// <summary>
    /// Get the last vector name without extension.
    /// </summary>
    /// <param name="workPath">Targeting work path</param>
    /// <returns>The name of last vector without extension. If the last vector is empty, return empty.</returns>
    public static string GetNameWithoutExtension(string workPath)
    {
        var start = workPath.Length - 1;
        var end = workPath.Length;
        for (var i = workPath.Length - 1; i >= 0; i--)
        {
            var c = workPath[i];
            start = i;
            if (end == workPath.Length && c == '.') end = i;
            if (c == DirectorySeparatorChar)
            {
                start += 1;
                break;
            }
        }

        return start < 0 || end <= start ? string.Empty : workPath.Substring(start, end - start);
    }
    

    /// <summary>
    /// Check if path is a root path. Do not guarantee path is valid.
    /// </summary>
    /// <param name="workPath">Work path being tested.</param>
    /// <returns>True when is root.</returns>
    public static bool IsRootPath(string workPath)
    {
        return workPath.Contains(DirectorySeparatorChar);
    }

    /// <summary>
    /// Check if path is ended with directory separator. Do not guarantee path is valid.
    /// </summary>
    /// <param name="workPath">Work path being tested.</param>
    /// <returns>True when is ended with directory separator.</returns>
    public static bool EndsInDirectorySeparator(string workPath)
    {
        return workPath.Length > 0 && workPath[^1] == DirectorySeparatorChar;
    }

    /// <summary>
    /// Get a relative work path from another work path in case first one is a sub path of second one. It will raise
    /// exception if path is invalid.
    /// </summary>
    /// <param name="relatedTo">The parent one.</param>
    /// <param name="workPath">The child one.</param>
    /// <returns>Null if workPath is not a child of relatedTo, or empty if workPath equals to relatedTo, or a new path
    /// when workPath is child of relatedTo.</returns>
    public static string? GetRelativePath(string relatedTo, string workPath)
    {
        if (workPath.Length == 0 || relatedTo.Length == 0) return null;
        using var parentTester = GetPathVector(relatedTo).GetEnumerator();
        using var childTester = GetPathVector(workPath).GetEnumerator();
        
        while (true)
        {
            var parentMoveNext = parentTester.MoveNext();
            var childMoveNext = childTester.MoveNext();

            // Both are not reach end
            if (parentMoveNext && childMoveNext)
            { 
                if (parentTester.Current == childTester.Current) continue;
                // Or going to break if they are not equal.
            }
            // Only if child has left but parent has all pass away, this means they share a same parent.
            else if (childMoveNext && !parentMoveNext)
            {
                var sb = new StringBuilder();

                sb.Append(childTester.Current);
                while (childTester.MoveNext())
                    sb.Append(DirectorySeparatorChar).Append(childTester.Current);

                return sb.ToString();
            }
            // If they are same path, return empty to indicate that.
            else if (!childMoveNext && !parentMoveNext) return string.Empty;
            
            break;
        }
        
        return null;
    }

    private static void CombineInternal(StringBuilder sb, string addon)
    {
        if (addon.Length <= 0) return;
        
        if (sb.Length <= 0)
        {
            sb.Append(addon);
            return;
        }

        var endSep = sb[^1] == DirectorySeparatorChar;
        var startSep = addon[0] == DirectorySeparatorChar;
        
        if (!endSep && !startSep)
        {
            sb.Append(DirectorySeparatorChar);
            sb.Append(addon);
        }
        else if (endSep && startSep)
        {
            sb.Append(addon, 1, addon.Length - 1);
        }
        else
        {
            sb.Append(addon);
        }
    }

    /// <summary>
    /// Combine path one by one. Do not guarantee path is valid.
    /// </summary>
    /// <example>
    /// It will connect those path in correct order and separator.
    /// <code>
    /// Combine("abc", "def") => "abc/def";
    /// Combine("abc", "def/") => "abc/def/";
    /// Combine("abc", "def/gg") => "abc/def/gg";
    /// Combine("abc/", "/def/gg") => "abc/def/gg";
    /// </code>
    /// </example>
    /// <returns>The connected path</returns>
    public static string Combine(string path1, string path2)
    {
        var sb = new StringBuilder();
        CombineInternal(sb, path1);
        CombineInternal(sb, path2);
        return sb.ToString();
    }
    
    /// <summary>
    /// Combine path one by one. Do not guarantee path is valid.
    /// </summary>
    /// <example>
    /// It will connect those path in correct order and separator.
    /// <code>
    /// Combine("abc", "def") => "abc/def";
    /// Combine("abc", "def/") => "abc/def/";
    /// Combine("abc", "def/gg") => "abc/def/gg";
    /// Combine("abc/", "/def/gg") => "abc/def/gg";
    /// </code>
    /// </example>
    /// <returns>The connected path</returns>
    public static string Combine(string path1, string path2, string path3)
    {
        var sb = new StringBuilder();
        CombineInternal(sb, path1);
        CombineInternal(sb, path2);
        CombineInternal(sb, path3);
        return sb.ToString();
    }

    /// <summary>
    /// Combine path one by one. Do not guarantee path is valid.
    /// </summary>
    /// <example>
    /// It will connect those path in correct order and separator.
    /// <code>
    /// Combine("abc", "def") => "abc/def";
    /// Combine("abc", "def/") => "abc/def/";
    /// Combine("abc", "def/gg") => "abc/def/gg";
    /// Combine("abc/", "/def/gg") => "abc/def/gg";
    /// </code>
    /// </example>
    /// <returns>The connected path</returns>
    public static string Combine(string path1, string path2, string path3, string path4)
    {
        var sb = new StringBuilder();
        CombineInternal(sb, path1);
        CombineInternal(sb, path2);
        CombineInternal(sb, path3);
        CombineInternal(sb, path4);
        return sb.ToString();
    }

    /// <summary>
    /// Combine path one by one. Do not guarantee path is valid.
    /// </summary>
    /// <example>
    /// It will connect those path in correct order and separator.
    /// <code>
    /// Combine("abc", "def") => "abc/def";
    /// Combine("abc", "def/") => "abc/def/";
    /// Combine("abc", "def/gg") => "abc/def/gg";
    /// Combine("abc/", "/def/gg") => "abc/def/gg";
    /// </code>
    /// </example>
    /// <returns>The connected path</returns>
    public static string Combine(params string[] paths)
    {
        var sb = new StringBuilder();
        foreach (var path in paths) CombineInternal(sb, path);
        return sb.ToString();
    }
}