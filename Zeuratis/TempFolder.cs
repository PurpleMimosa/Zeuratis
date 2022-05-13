namespace Zeuratis;

partial class Zeuratis
{
    public static void ClearTemp()
    {
        foreach (FileInfo file in new DirectoryInfo("tempfile").GetFiles())
        {
            file.Delete();
        }
    }
}
