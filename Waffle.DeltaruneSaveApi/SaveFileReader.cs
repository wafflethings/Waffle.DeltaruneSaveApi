using System.Globalization;
using System.Text;

namespace Waffle.DeltaruneSaveApi;

public class SaveFileReader : IDisposable
{
    public const string EndData = "\u000d\u000a";
    public const string RealEndData = "\u0020";
    public const string Exponent = "e+";

    private BinaryReader _reader;

    public SaveFileReader(FileStream stream)
    {
        _reader = new BinaryReader(stream, Encoding.UTF8);
    }

    public void Dispose()
    {
        _reader.Dispose();
    }

    public string ReadString()
    {
        string currentString = string.Empty;

        while (!currentString.EndsWith(EndData) && _reader.BaseStream.Position != _reader.BaseStream.Length)
        {
            currentString += _reader.ReadChar();
        }

        return currentString[..^(_reader.BaseStream.Position == _reader.BaseStream.Length ? 0 : EndData.Length)];
    }

    public float ReadReal()
    {
        string data = ReadString()[..^1];

        if (!data.Contains(Exponent))
        {
            return float.Parse(data, CultureInfo.InvariantCulture);
        }

        string[] split = data.Split("e+");
        return float.Parse(split[0], CultureInfo.InvariantCulture) * (MathF.Pow(10, float.Parse(split[1])));
    }

    public CharacterInfo ReadCharacterInfo(Version version)
    {
        CharacterInfo info = new()
        {
            Hp = ReadReal(),
            MaxHp = ReadReal(),
            Attack = ReadReal(),
            Defence = ReadReal(),
            Magic = ReadReal(),
            Guts = ReadReal(),
            Weapon = (int)ReadReal(),
            Armor1 = (int)ReadReal(),
            Armor2 = (int)ReadReal(),
            WeaponStyle = new WeaponStyle(version == Version.Chapter1 ? ReadString() : null, version == Version.Chapter234 ? ReadReal() : null),
            Items = new EquippedItemInfo[4],
            Spells = new int[12]
        };
        
        for (int i = 0; i < 4; i++)
        {
            info.Items[i] = ReadEquippedItemInfo(version);
        }

        for (int i = 0; i < 12; i++)
        {
            info.Spells[i] = (int)ReadReal();
        }

        return info;
    }
    
    private EquippedItemInfo ReadEquippedItemInfo(Version version) => new()
    {
        Attack = ReadReal(),
        Defence = ReadReal(),
        Magic = ReadReal(),
        Bolts = ReadReal(),
        GrazeAmount = ReadReal(),
        GrazeSize = ReadReal(),
        BoltSpeed = ReadReal(),
        Special = ReadReal(),
        Element = version == Version.Chapter1 ? null : ReadReal(),
        ElementAmount = version == Version.Chapter1 ? null : ReadReal()
    };
}