namespace Zeuratis;

class ZeuratisSetup
{
    public static int DefaultId { get; set; } = -1;

    public static int GetDefaultId()
    {
        return DefaultId = new Random().Next(10000, 100000);
    }
}
