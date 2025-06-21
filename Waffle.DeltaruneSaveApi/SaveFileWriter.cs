using System.Globalization;
using System.Text;

namespace Waffle.DeltaruneSaveApi;

public class SaveFileWriter : IDisposable
{
    private BinaryWriter _writer;

    public SaveFileWriter(string saveToPath)
    {
        _writer = new BinaryWriter(File.Open(saveToPath, FileMode.Create), Encoding.UTF8);
    }
    
    public void Dispose()
    {
        _writer.Dispose();
    }

    public void DeleteEof()
    {
        _writer.BaseStream.SetLength(_writer.BaseStream.Length - 2); // usually values are followed by \u000d\u000a, but the last one isnt
    }
    
    public void WriteString(string data)
    {
        foreach (char character in data + SaveFileReader.EndData)
        {
            _writer.Write(character);
        }
    }

    public void WriteReal(float data) 
    {
        if (data < MathF.Pow(10, 6)) // it only does exponents above 10^6
        {
            WriteString(data.ToString(CultureInfo.InvariantCulture) + SaveFileReader.RealEndData);
            return;
        }

        string sciNotation = data.ToString("e5", CultureInfo.InvariantCulture) + SaveFileReader.RealEndData;
        string[] split = sciNotation.Split(SaveFileReader.Exponent);
        WriteString(split[0] + SaveFileReader.Exponent + int.Parse(split[1]).ToString("D2") + SaveFileReader.RealEndData); // this needs to have two digits, default c# does 3
    }

    public void WriteCharacterInfo(CharacterInfo data, Version version)
    {
        WriteReal(data.Hp);
        WriteReal(data.MaxHp);
        WriteReal(data.Attack);
        WriteReal(data.Defence);
        WriteReal(data.Magic);
        WriteReal(data.Guts);
        WriteReal(data.Weapon);
        WriteReal(data.Armor1);
        WriteReal(data.Armor2);
        
        if (version == Version.Chapter1)
        {
            WriteString(data.WeaponStyle.GetValue<string>());
        }
        else
        {
            WriteReal(data.WeaponStyle.GetValue<float>());
        }

        for (int i = 0; i < 4; i++)
        {
            WriteEquippedItemInfo(data.Items[i], version);
        }

        for (int i = 0; i < 12; i++)
        {
            WriteReal(data.Spells[i]);
        }
    }

    private void WriteEquippedItemInfo(EquippedItemInfo data, Version version)
    {
        WriteReal(data.Attack);
        WriteReal(data.Defence);
        WriteReal(data.Magic);
        WriteReal(data.Bolts);
        WriteReal(data.GrazeAmount);
        WriteReal(data.GrazeSize);
        WriteReal(data.BoltSpeed);
        WriteReal(data.Special);

        if (version != Version.Chapter1 && data.Element.HasValue && data.ElementAmount.HasValue)
        {
            WriteReal(data.Element.Value);
            WriteReal(data.ElementAmount.Value);
        }
    }
}