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

    private void Exp()
    {
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DELTARUNE", "filech4_0");
        SaveFile file = SaveFile.Open(filePath, Version.Chapter234);

        file.TrueName = "Waffle";                           // change file name
        file.Gold = 999999;                                 // infinite money glitch
        file.CharacterInfo[Character.Kris].MaxHp = 9999;    // more health
        file.Items[0] = Item.ReviveBrite;                   // setting inventory slots

        foreach (int i in Enumerable.Range(662, 7))
        {
            file.Flags[i] = 1;                              // recruit all enemies from chapter 4
        }
        
        file.Save();                                        // save changes
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