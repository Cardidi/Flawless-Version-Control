using Flawless.Abstraction;

namespace Flawless.Abstract.Test;

[TestClass]
public sealed class WorkPathTestUnit
{
    [TestMethod]
    public void CombinationTest()
    {
        Assert.AreEqual("abc/def", WorkPath.Combine("abc", "def"));
        Assert.AreEqual("/abc/def", WorkPath.Combine("/abc", "def"));
        Assert.AreEqual("abc/def/", WorkPath.Combine("abc", "def/"));
        Assert.AreEqual("/abc/def/", WorkPath.Combine("/abc", "def/"));
        Assert.AreEqual("/abc/def/", WorkPath.Combine("/abc/", "/def/"));
        Assert.AreEqual("abc/def", WorkPath.Combine("abc/", "def"));
        Assert.AreEqual("abc/def", WorkPath.Combine("abc", "/def"));
        Assert.AreEqual("abc/def", WorkPath.Combine("abc/", "/def"));
        Assert.AreEqual("abc/", WorkPath.Combine("abc", "/"));
        Assert.AreEqual("abc//", WorkPath.Combine("abc", "//"));
        Assert.AreEqual("abc/def/ghi", WorkPath.Combine("abc", "def", "ghi"));
        Assert.AreEqual("abc/def/ghi/nmp", WorkPath.Combine("abc", "def", "ghi", "nmp"));
        Assert.AreEqual("abc/def/ghi/nmp/uvw", WorkPath.Combine("abc", "def", "ghi", "nmp", "uvw"));
        Assert.AreEqual("abc/def/ghi/nmp/uvw", WorkPath.Combine("abc", "def/ghi/nmp/uvw"));
    }

    [TestMethod]
    public void ValidPathValidateTest()
    {
        Assert.IsTrue(WorkPath.IsPathValid("abc"));
        Assert.IsTrue(WorkPath.IsPathValid("abc/def"));
        Assert.IsTrue(WorkPath.IsPathValid("abc/d ef"));
        Assert.IsTrue(WorkPath.IsPathValid("a  b c/d ef"));
        Assert.IsTrue(WorkPath.IsPathValid("abc/def/ghi/k.lmn"));
        Assert.IsTrue(WorkPath.IsPathValid(".flawless.ignore"));
        Assert.IsTrue(WorkPath.IsPathValid("sub/.flawless.ignore"));
    }

    [TestMethod]
    public void InvalidPathValidateTest()
    {
        Assert.IsFalse(WorkPath.IsPathValid("ab\\c/def"));
        Assert.IsFalse(WorkPath.IsPathValid("ab?c/def"));
        Assert.IsFalse(WorkPath.IsPathValid(" abc/def/ghi/k.lmn"));
        Assert.IsFalse(WorkPath.IsPathValid("/abc/def"));
        Assert.IsFalse(WorkPath.IsPathValid("/abc/def/"));
        Assert.IsFalse(WorkPath.IsPathValid(""));
        Assert.IsFalse(WorkPath.IsPathValid(null!));
        Assert.IsFalse(WorkPath.IsPathValid("abc/ de f/ghi/k.lmn"));
        Assert.IsFalse(WorkPath.IsPathValid("abc/def/"));
        Assert.IsFalse(WorkPath.IsPathValid("abc/def "));
    }


    [TestMethod]
    public void ValidPathSplitTest()
    {
        Assert.IsTrue(ValidPathVectorMatch("abc", "abc"));
        Assert.IsTrue(ValidPathVectorMatch("abc/def", "abc", "def"));
        Assert.IsTrue(ValidPathVectorMatch("abc/d ef", "abc", "d ef"));
        Assert.IsTrue(ValidPathVectorMatch("a  b c/d ef", "a  b c", "d ef"));
        Assert.IsTrue(ValidPathVectorMatch("abc/def/ghi/k.lmn", "abc", "def", "ghi", "k.lmn"));
    }

    [TestMethod]
    public void InvalidPathSplitTest()
    {
        Assert.IsTrue(InvalidPathVectorMatch<ArgumentException>("ab\\c/def", "Invalid work path character"));
        Assert.IsTrue(InvalidPathVectorMatch<ArgumentException>("ab?c/def", "Invalid work path character"));
        Assert.IsTrue(InvalidPathVectorMatch<ArgumentException>(" abc/def/ghi/k.lmn  ", "Work path vector can not start or end with a space!"));
        Assert.IsTrue(InvalidPathVectorMatch<ArgumentException>("/abc/def", "Work path cannot start with a DirectorySeparatorChar!"));
        Assert.IsTrue(InvalidPathVectorMatch<ArgumentException>("/abc/def/", "Work path cannot start with a DirectorySeparatorChar!"));
        Assert.IsTrue(InvalidPathVectorMatch<ArgumentNullException>("", "Not a valid work path!"));
        Assert.IsTrue(InvalidPathVectorMatch<ArgumentNullException>(null!, "Not a valid work path!"));
        Assert.IsTrue(InvalidPathVectorMatch<ArgumentException>("abc/ de f/ghi/k.lmn", "Work path vector can not start or end with a space!"));
        Assert.IsTrue(InvalidPathVectorMatch<ArgumentException>("abc/def/", "Work path contains empty vector!"));
        Assert.IsTrue(InvalidPathVectorMatch<ArgumentException>("abc/def ", "Work path vector can not start or end with a space!"));
    }

    [TestMethod]
    public void RelativePathTest()
    {
        Assert.AreEqual("def", WorkPath.GetRelativePath("abc", "abc/def"));
        Assert.AreEqual("ghi", WorkPath.GetRelativePath("abc/def", "abc/def/ghi"));
        Assert.AreEqual("def/ghi", WorkPath.GetRelativePath("abc", "abc/def/ghi"));
        Assert.AreEqual(string.Empty, WorkPath.GetRelativePath("abc/def", "abc/def"));
        Assert.AreEqual(string.Empty, WorkPath.GetRelativePath("abc", "abc"));
        Assert.AreEqual(null, WorkPath.GetRelativePath("abc/def", "abc"));
    }

    [TestMethod]
    public void PlatformToWorkPathTransformTest()
    {
        Assert.AreEqual("abc/def", WorkPath.FromPlatformPath("/root/test/abc/def", "/root/test/"));
        Assert.AreEqual("abc/def", WorkPath.FromPlatformPath("/root/test/abc/def", "/root/test"));
        Assert.AreEqual("", WorkPath.FromPlatformPath("/root/test", "/root/test"));
        Assert.AreEqual("", WorkPath.FromPlatformPath("/root/test/", "/root/test"));
        Assert.AreEqual("", WorkPath.FromPlatformPath("/root/test", "/root/test/"));
        Assert.AreEqual("", WorkPath.FromPlatformPath("/root/test/", "/root/test/"));
        Assert.AreEqual("abc/def", WorkPath.FromPlatformPath("./root/test/abc/def", "./root/test"));
        Assert.AreEqual("", WorkPath.FromPlatformPath(@".\root\test", @".\root\test"));
        Assert.AreEqual("", WorkPath.FromPlatformPath(@".\root\test", @".\root/test"));
        Assert.AreEqual("abc/def", WorkPath.FromPlatformPath(@".\root\test\abc\def", @".\root\test"));
        Assert.AreEqual("abc/def", WorkPath.FromPlatformPath(@"C:\root\test\abc\def", @"C:\root\test\"));
        Assert.AreEqual("abc/def", WorkPath.FromPlatformPath(@"C:\root\test\abc\def", @"C:\root\test"));
        Assert.AreEqual("abc/def", WorkPath.FromPlatformPath(@"C:\root/test\abc/def", @"C:\root\test\"));
        Assert.AreEqual("abc/def", WorkPath.FromPlatformPath(@"C:\root\test\abc\def", @"C:\root/test"));
    }

    [TestMethod]
    public void WorkToPlatformPathTransformTest()
    {
        Assert.AreEqual(".", Path.GetRelativePath("/root/test/abc/def", WorkPath.ToPlatformPath("abc/def", "/root/test")));
        Assert.AreEqual(".", Path.GetRelativePath("/root/test/abc/def", WorkPath.ToPlatformPath("/abc/def", "/root/test/")));
        Assert.AreEqual(".", Path.GetRelativePath("/root/test/abc/def", WorkPath.ToPlatformPath("/abc/def/", "/root/test")));
        Assert.AreEqual(".", Path.GetRelativePath("/root/test/abc/def", WorkPath.ToPlatformPath("/abc/def/", "/root/test/")));
        Assert.AreEqual(".", Path.GetRelativePath("/root/test/abc/def", WorkPath.ToPlatformPath("abc/def", "C:/root/test")));
        Assert.AreEqual(".", Path.GetRelativePath("/root/test/abc/def", WorkPath.ToPlatformPath("/abc/def", "C:/root/test/")));
        Assert.AreEqual(".", Path.GetRelativePath("/root/test/abc/def", WorkPath.ToPlatformPath("/abc/def/", "C:/root/test")));
        Assert.AreEqual(".", Path.GetRelativePath("/root/test/abc/def", WorkPath.ToPlatformPath("/abc/def/", "C:/root/test/")));
        Assert.AreEqual(".", Path.GetRelativePath("/root/test/abc/def", WorkPath.ToPlatformPath("abc/def", @"C:/root\test")));
        Assert.AreEqual(".", Path.GetRelativePath("/root/test/abc/def", WorkPath.ToPlatformPath("abc/def", @"/root\test")));
    }

    [TestMethod]
    public void WorkPathGetExtensionTest()
    {
        Assert.AreEqual("", WorkPath.GetExtension(""));
        Assert.AreEqual("", WorkPath.GetExtension("/"));
        Assert.AreEqual("git", WorkPath.GetExtension(".git"));
        Assert.AreEqual("git", WorkPath.GetExtension("abc.git"));
        
        Assert.AreEqual("", WorkPath.GetExtension("abc/def"));
        Assert.AreEqual("", WorkPath.GetExtension("abc/"));
        Assert.AreEqual("ght", WorkPath.GetExtension("abc/def.ght"));
        Assert.AreEqual("ght", WorkPath.GetExtension(".abc/.ght"));
        Assert.AreEqual("ght", WorkPath.GetExtension("abc/.ght"));
        
        Assert.AreEqual("", WorkPath.GetExtension("/abc/def"));
        Assert.AreEqual("", WorkPath.GetExtension("/abc/"));
        Assert.AreEqual("ght", WorkPath.GetExtension("/abc/def.ght"));
        Assert.AreEqual("ght", WorkPath.GetExtension("/abc/.ght"));
        Assert.AreEqual("ght", WorkPath.GetExtension("/.abc/.ght"));
    }

    
    [TestMethod]
    public void WorkPathChangeExtensionTest()
    {
        Assert.AreEqual(".png", WorkPath.ChangeExtension("", "png"));
        Assert.AreEqual("/.png", WorkPath.ChangeExtension("/", "png"));
        Assert.AreEqual(".png", WorkPath.ChangeExtension(".git", "png"));
        Assert.AreEqual("abc.png", WorkPath.ChangeExtension("abc.git", "png"));
        
        Assert.AreEqual("abc/def.png", WorkPath.ChangeExtension("abc/def", "png"));
        Assert.AreEqual("abc/.png", WorkPath.ChangeExtension("abc/", "png"));
        Assert.AreEqual("abc/def.png", WorkPath.ChangeExtension("abc/def.ght", "png"));
        Assert.AreEqual(".abc/.png", WorkPath.ChangeExtension(".abc/.ght", "png"));
        Assert.AreEqual("abc/.png", WorkPath.ChangeExtension("abc/.ght", "png"));
        
        Assert.AreEqual("/abc/def.png", WorkPath.ChangeExtension("/abc/def", "png"));
        Assert.AreEqual("/abc/.png", WorkPath.ChangeExtension("/abc/", "png"));
        Assert.AreEqual("/abc/def.png", WorkPath.ChangeExtension("/abc/def.ght", "png"));
        Assert.AreEqual("/abc/.png", WorkPath.ChangeExtension("/abc/.ght", "png"));
        Assert.AreEqual("/.abc/.png", WorkPath.ChangeExtension("/.abc/.ght", "png"));
    }
    
    [TestMethod]
    public void WorkPathExistExtensionTest()
    {
        Assert.IsFalse(WorkPath.HasExtension(""));
        Assert.IsFalse(WorkPath.HasExtension("/"));
        Assert.IsTrue(WorkPath.HasExtension(".git"));
        Assert.IsTrue(WorkPath.HasExtension("abc.git"));
        
        Assert.IsFalse(WorkPath.HasExtension("abc/def"));
        Assert.IsFalse(WorkPath.HasExtension("abc/"));
        Assert.IsTrue(WorkPath.HasExtension("abc/def.ght"));
        Assert.IsTrue(WorkPath.HasExtension(".abc/.ght"));
        Assert.IsTrue(WorkPath.HasExtension("abc/.ght"));
        
        Assert.IsFalse(WorkPath.HasExtension("/abc/def"));
        Assert.IsFalse(WorkPath.HasExtension("/abc/"));
        Assert.IsTrue(WorkPath.HasExtension("/abc/def.ght"));
        Assert.IsTrue(WorkPath.HasExtension("/abc/.ght"));
        Assert.IsTrue(WorkPath.HasExtension("/.abc/.ght"));
    }

    [TestMethod]
    public void WorkPathGetLastVectorTest()
    {
        Assert.AreEqual("", WorkPath.GetName(""));
        Assert.AreEqual("", WorkPath.GetName("/"));
        Assert.AreEqual(".git", WorkPath.GetName(".git"));
        Assert.AreEqual("abc.git", WorkPath.GetName("abc.git"));
        
        Assert.AreEqual("def", WorkPath.GetName("abc/def"));
        Assert.AreEqual("", WorkPath.GetName("abc/"));
        Assert.AreEqual("def.ght", WorkPath.GetName("abc/def.ght"));
        Assert.AreEqual(".ght", WorkPath.GetName(".abc/.ght"));
        Assert.AreEqual(".ght", WorkPath.GetName("abc/.ght"));
        
        Assert.AreEqual("def", WorkPath.GetName("/abc/def"));
        Assert.AreEqual("", WorkPath.GetName("/abc/"));
        Assert.AreEqual("def.ght", WorkPath.GetName("/abc/def.ght"));
        Assert.AreEqual(".ght", WorkPath.GetName("/abc/.ght"));
        Assert.AreEqual(".ght", WorkPath.GetName("/.abc/.ght"));
    }
    
    [TestMethod]
    public void WorkPathGetLastVectorNoExtensionTest()
    {
        Assert.AreEqual("", WorkPath.GetNameWithoutExtension(""));
        Assert.AreEqual("", WorkPath.GetNameWithoutExtension("/"));
        Assert.AreEqual("", WorkPath.GetNameWithoutExtension(".git"));
        Assert.AreEqual("abc", WorkPath.GetNameWithoutExtension("abc.git"));
        
        Assert.AreEqual("def", WorkPath.GetNameWithoutExtension("abc/def"));
        Assert.AreEqual("", WorkPath.GetNameWithoutExtension("abc/"));
        Assert.AreEqual("def", WorkPath.GetNameWithoutExtension("abc/def.ght"));
        Assert.AreEqual("", WorkPath.GetNameWithoutExtension(".abc/.ght"));
        Assert.AreEqual("", WorkPath.GetNameWithoutExtension("abc/.ght"));
        
        Assert.AreEqual("def", WorkPath.GetNameWithoutExtension("/abc/def"));
        Assert.AreEqual("", WorkPath.GetNameWithoutExtension("/abc/"));
        Assert.AreEqual("def", WorkPath.GetNameWithoutExtension("/abc/def.ght"));
        Assert.AreEqual("", WorkPath.GetNameWithoutExtension("/abc/.ght"));
        Assert.AreEqual("", WorkPath.GetNameWithoutExtension("/.abc/.ght"));
    }

    #region HelperMethod

    private bool ValidPathVectorMatch(string workPath, params string[] subPaths)
    {
        var count = 0;
        if (WorkPath.GetPathVector(workPath).Any(se => se != subPaths[count++]))
            return false;

        var list = new List<string>();
        WorkPath.GetPathVector(workPath, list);

        return list.Count == subPaths.Length && count == subPaths.Length;
    }

    private bool InvalidPathVectorMatch<T>(string workPath, string errorMessageStarter) where T : Exception
    {
        try
        {
            foreach (var _ in WorkPath.GetPathVector(workPath))
            {
            }
        }
        catch (T e)
        {
            return e.Message.StartsWith(errorMessageStarter);
        }
        catch (Exception)
        {
            // ignored
        }

        return false;
    }

    private bool InvalidPathVectorMatch<T>(string workPath) where T : Exception
    {
        try
        {
            foreach (var _ in WorkPath.GetPathVector(workPath))
            {
            }
        }
        catch (T e)
        {
            return true;
        }
        catch (Exception)
        {
            // ignored
        }

        return false;
    }

    #endregion
}