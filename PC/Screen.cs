namespace Zeuratis;

[SupportedOSPlatform("windows")]
internal class Display
{
    public static String Capture()
    {
        String filePath = @"C:\Users\" + Environment.UserName + @"\Pictures\Screenshots\png" + new Random().Next(100, 1000) + ".png";
        Rectangle bounds = Screen.GetBounds(Point.Empty);

        using Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
        using Graphics gfx = Graphics.FromImage(bitmap);

        gfx.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
        bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);

        Thread.Sleep(500);

        return filePath;
    }
}
