using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
}
