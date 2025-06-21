namespace Waffle.DeltaruneSaveApi;

public class WeaponStyle
{
    private string? _1 = null;
    private float? _2Plus = null;
    
    public WeaponStyle(string? oneValue, float? twoPlusValue)
    {
        _1 = oneValue;
        _2Plus = twoPlusValue;
    }

    public T GetValue<T>()
    {
        if (typeof(T) == typeof(string))
        {
            if (_1 == null)
            {
                throw new Exception("Can only get value as string in Chapter1 files");
            }

            return (T)(object)_1;
        }
        
        if (typeof(T) == typeof(float))
        {
            if (_2Plus == null)
            {
                throw new Exception("Can only get value as float in Chapter2+ files");
            }

            return (T)(object)_2Plus;
        }

        throw new Exception("Invalid type argument: string for Ch1, float for Ch2+");
    }
}