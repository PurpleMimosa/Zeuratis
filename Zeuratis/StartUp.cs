namespace Zeuratis;

[SupportedOSPlatform("windows")]
partial class Zeuratis
{
    public static void AddToStartUp()
    {
        IWshRuntimeLibrary.WshShell wshShell = new();
        IWshRuntimeLibrary.IWshShortcut shortcut;

        String startUpPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

        shortcut =
          (IWshRuntimeLibrary.IWshShortcut)wshShell.CreateShortcut(startUpPath + "\\Zeuratis.lnk");

        shortcut.TargetPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Zeuratis.exe"; // I dont know why methods, such as Application.ExecutablePath, or Assembly class methods dont work to get the path of the exe; they all throw exceptions.
        shortcut.Description = "Zeuratis";

        shortcut.Save();
    }
}
