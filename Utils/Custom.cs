using UnityEngine;

using Object = UnityEngine.Object;

public class Custom
{
    public static T GetOrAddComponent<T>(GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();
        if (component == null)
            component = obj.AddComponent<T>();

        return component;
    }

    public static T FindChild<T>(GameObject obj, string name, Transform parent = null) where T : Object
    {
        if (obj == null)
            return null;

        foreach(T component in obj.GetComponentsInChildren<T>())
        {
            if (component.name == name)
                return component;
        }

        return null;
    }

    public static string CalUnit(int value)
    {
        if (value < 0)
            return ConstValue.Max;

        string[] units = { "", "A", "B", "C", "D", "E", "F" };
        int count = 0;
        int remain = 0;
        while (value > 1000)
        {
            remain = value % 1000;
            value /= 1000;
            count++;
        }

        if (value >= 100)
            return value + units[count];
        else if(value >= 10)
            return string.Format("{0}.{1:0}{2}", value, remain / 100, units[count]);
        else
            return string.Format("{0}.{1:00}{2}", value, remain / 10, units[count]);
    }
}