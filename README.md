# Waffle.DeltaruneSaveApi

A low-level NET8.0 library for modifying [DELTARUNE](https://deltarune.com) savedata.

Note that since it's pretty low-level and there is no GUI, this isn't designed for users, and only for programmers who actually know what they're doing.

Back up save data before usage -- I've tested it, but it might not work 100% of the time.

## Useful features
- All items are named in enums (e.g. `Armor.LodeStone`, `Item.ReviveMint`, `KeyItem.Lancer`, `Weapon.ManeAx`)

## Missing features
- Flags aren't named, since there are way too many of them. You will need to use [UTMT](https://github.com/UnderminersTeam/UndertaleModTool) to find out what the flag's ID is
- Some fields lack descriptive names (e.g. `Inv`, `Invc`) as I couldn't figure out their purpose

## Example usage

```
string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DELTARUNE", "filech4_0");
SaveFile file = SaveFile.Open(filePath, Version.Chapter234)

file.TrueName = "Waffle";                           // change file name
file.Gold = 999999;                                 // infinite money glitch
file.CharacterInfo[Character.Kris].MaxHp = 9999;    // more health
file.Items[0] = Item.ReviveBrite;                   // setting inventory slot

foreach (int i in Enumerable.Range(662, 7))
{
    file.Flags[i] = 1;                              // recruit all enemies from chapter 4
}

file.Save();                                        // save changes
```