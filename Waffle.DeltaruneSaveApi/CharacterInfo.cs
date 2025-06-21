namespace Waffle.DeltaruneSaveApi;

public class CharacterInfo
{
    public float Hp;
    public float MaxHp;
    public float Attack;
    public float Defence;
    public float Magic;
    public float Guts;
    public int Weapon;
    public int Armor1;
    public int Armor2;
    public WeaponStyle WeaponStyle;
    public EquippedItemInfo[] Items = new EquippedItemInfo[4];
    public int[] Spells = new int[12];
}