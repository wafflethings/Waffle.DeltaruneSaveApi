using System.Text;

namespace Waffle.DeltaruneSaveApi;

public class SaveFile
{
    public string TrueName = "Kris";
    public readonly string[] OtherNames = ["", "", "", "", "", ""]; // must be size 6
    public readonly Character[] Characters = [Character.Kris, Character.Susie, Character.Ralsei]; // must be size 3
    public float Gold;
    public float Xp;
    public float Lv;
    public float Inv;
    public float Invc;
    public bool InDarkWorld;
    public Dictionary<Character, CharacterInfo> CharacterInfo; // ch1 4, ch2+ 5    // element and elementamount missing until ch2
    public float BoltSpeed;
    public float GrazeAmount;
    public float GrazeSize;
    public readonly Item[] Items = new Item[13];
    public readonly KeyItem[] KeyItems = new KeyItem[13];
    public readonly Weapon[] Weapons; // ch1 13, ch2+ 48
    public readonly Armor[] Armors; // ch1 13, ch2+ 48
    public readonly Item[] PocketItems; // ch1 nonexistant, ch2+ 72 
    public float Tension;
    public float MaxTension;
    public int LightWorldWeapon;
    public int LightWorldArmor;
    public float LightWorldXp;
    public float LightWorldLv;
    public float LightWorldGold;
    public float LightWorldHp;
    public float LightWorldMaxHp;
    public float LightWorldAttack;
    public float LightWorldDefence;
    public float LightWorldWeaponStrength;
    public float LightWorldArmorDefence;
    public readonly LightWorldItem[] LightWorldItems = new LightWorldItem[8];
    public readonly int[] PhoneNumbers = new int[8];
    public readonly float[] Flags; // ch1 9999, ch4 2500
    public float Plot;
    public int CurrentRoom;
    public float Time;

    private Version _version;
    private FileStream _dataStream;
    private bool _overwrite;

    public static SaveFile Open(string path, Version version, bool overwrite = false)
    {
        SaveFile file = new(path, version, overwrite);
        using SaveFileReader reader = new(file._dataStream);

        file.TrueName = reader.ReadString();
        for (int i = 0; i < 6; i++)
        {
            file.OtherNames[i] = reader.ReadString();
        }

        for (int i = 0; i < 3; i++)
        {
            file.Characters[i] = (Character)reader.ReadReal();
        }

        file.Gold = reader.ReadReal();
        file.Xp = reader.ReadReal();
        file.Lv = reader.ReadReal();
        file.Inv = reader.ReadReal();
        file.Invc = reader.ReadReal();
        file.InDarkWorld = reader.ReadReal() != 0;

        for (int i = 0; i < file.CharacterCount; i++)
        {
            file.CharacterInfo.Add((Character)i, reader.ReadCharacterInfo(file._version));
        }

        file.BoltSpeed = reader.ReadReal();
        file.GrazeAmount = reader.ReadReal();
        file.GrazeSize = reader.ReadReal();
        
        for (int i = 0; i < 13; i++)
        {
            file.Items[i] = (Item)reader.ReadReal();
            file.KeyItems[i] = (KeyItem)reader.ReadReal();
        }
        
        for (int i = 0; i < file.WeaponAndArmorCount; i++)
        {
            file.Weapons[i] = (Weapon)reader.ReadReal();
            file.Armors[i] = (Armor)reader.ReadReal();
        }
        
        for (int i = 0; i < file.PocketItemCount; i++)
        {
            file.PocketItems[i] = (Item)reader.ReadReal();
        }

        file.Tension = reader.ReadReal();
        file.MaxTension = reader.ReadReal();
        file.LightWorldWeapon = (int)reader.ReadReal();
        file.LightWorldArmor = (int)reader.ReadReal();
        file.LightWorldXp = reader.ReadReal();
        file.LightWorldLv = reader.ReadReal();
        file.LightWorldGold = reader.ReadReal();
        file.LightWorldHp = reader.ReadReal();
        file.LightWorldMaxHp = reader.ReadReal();
        file.LightWorldAttack = reader.ReadReal();
        file.LightWorldDefence = reader.ReadReal();
        file.LightWorldWeaponStrength = reader.ReadReal();
        file.LightWorldArmorDefence = reader.ReadReal();
        
        for (int i = 0; i < 8; i++)
        {
            file.LightWorldItems[i] = (LightWorldItem)reader.ReadReal();
            file.PhoneNumbers[i] = (int)reader.ReadReal();
        }
        
        for (int i = 0; i < file.FlagCount; i++)
        {
            file.Flags[i] = reader.ReadReal();
        }

        file.Plot = reader.ReadReal();
        file.CurrentRoom = (int)reader.ReadReal();
        file.Time = reader.ReadReal();
        
        return file;
    }

    public int CharacterCount => _version == Version.Chapter1 ? 4 : 5;

    public int WeaponAndArmorCount => _version == Version.Chapter1 ? 13 : 48;

    public int PocketItemCount => _version == Version.Chapter1 ? 0 : 72;

    public int FlagCount => _version == Version.Chapter1 ? 9999 : 2500;

    public void Save()
    {
        using SaveFileWriter writer = new(_dataStream, _overwrite);
        
        writer.WriteString(TrueName);
        
        for (int i = 0; i < 6; i++)
        {
            writer.WriteString(OtherNames[i]);
        }

        for (int i = 0; i < 3; i++)
        {
            writer.WriteReal((float)Characters[i]);
        }
        
        writer.WriteReal(Gold);
        writer.WriteReal(Xp);
        writer.WriteReal(Lv);
        writer.WriteReal(Inv);
        writer.WriteReal(Invc);
        writer.WriteReal(InDarkWorld ? 1 : 0);
        
        for (int i = 0; i < CharacterCount; i++)
        {
            writer.WriteCharacterInfo(CharacterInfo[(Character)i], _version);
        }
        
        writer.WriteReal(BoltSpeed);
        writer.WriteReal(GrazeAmount);
        writer.WriteReal(GrazeSize);
        
        for (int i = 0; i < 13; i++)
        {
            writer.WriteReal((int)Items[i]);
            writer.WriteReal((int)KeyItems[i]);
        }
        
        for (int i = 0; i < WeaponAndArmorCount; i++)
        {
            writer.WriteReal((int)Weapons[i]);
            writer.WriteReal((int)Armors[i]);
        }
        
        for (int i = 0; i < PocketItemCount; i++)
        {
            writer.WriteReal((int)PocketItems[i]);
        }
        
        writer.WriteReal(Tension);
        writer.WriteReal(MaxTension);
        writer.WriteReal(LightWorldWeapon);
        writer.WriteReal(LightWorldArmor);
        writer.WriteReal(LightWorldXp);
        writer.WriteReal(LightWorldLv);
        writer.WriteReal(LightWorldGold);
        writer.WriteReal(LightWorldHp);
        writer.WriteReal(LightWorldMaxHp);
        writer.WriteReal(LightWorldAttack);
        writer.WriteReal(LightWorldDefence);
        writer.WriteReal(LightWorldWeaponStrength);
        writer.WriteReal(LightWorldArmorDefence);
        
        for (int i = 0; i < 8; i++)
        {
            writer.WriteReal((int)LightWorldItems[i]);
            writer.WriteReal(PhoneNumbers[i]);
        }
        
        for (int i = 0; i < FlagCount; i++)
        {
            writer.WriteReal(Flags[i]);
        }
        
        writer.WriteReal(Plot);
        writer.WriteReal(CurrentRoom);
        writer.WriteReal(Time);
        writer.DeleteEof();
    }

    private SaveFile(string path, Version version, bool overwrite)
    {
        _dataStream = File.Open(path, FileMode.Open);
        _version = version;
        _overwrite = overwrite;
        
        CharacterInfo = new Dictionary<Character, CharacterInfo>(CharacterCount);
        Weapons = new Weapon[WeaponAndArmorCount];
        Armors = new Armor[WeaponAndArmorCount];
        PocketItems = new Item[PocketItemCount];
        Flags = new float[FlagCount];
    }
}