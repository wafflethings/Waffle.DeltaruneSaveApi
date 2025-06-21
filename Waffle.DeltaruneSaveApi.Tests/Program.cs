namespace Waffle.DeltaruneSaveApi.Tests;

class Program
{
    static void Main(string[] args)
    {
        TestFile("filech1_0", Version.Chapter1);
        TestFile("filech2_0", Version.Chapter234);
        TestFile("filech3_0", Version.Chapter234);
        TestFile("filech4_0", Version.Chapter234);
    }

    private static void TestFile(string name, Version version)
    {
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DELTARUNE", name);
        
        SaveFile file = SaveFile.Open(filePath, version);
        file.Save();

        byte[] originalFile = File.ReadAllBytes(filePath);
        byte[] modFile = File.ReadAllBytes(filePath + ".mod");

        List<int> errorIndexes = [];
        for (int i = 0; i < modFile.Length; i++)
        {
            if ((modFile[i] == originalFile[i]))
            {
                continue;
            }
            
            errorIndexes.Add(i);
        }

        if (errorIndexes.Count > 0)
        {
            Console.Error.WriteLine($"{name} is inaccurate! Errors at: {string.Join(", ", errorIndexes)}");
            return;
        }
        
        Console.WriteLine($"{name} is accurate!");
    }
}