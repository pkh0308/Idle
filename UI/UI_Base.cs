using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Object = UnityEngine.Object;

public class UI_Base : MonoBehaviour
{
    public enum EventType
    {
        OnClick,
        OnClickDown,
        OnClickUp,
        OnPressed,
    }

    Dictionary<Type, Object[]> _objects = new Dictionary<Type, Object[]>();

    protected bool _initialized = false;
    public virtual bool Init()
    {
        if (_initialized)
            return false;

        _initialized = true;
        return true;
    }

    void Start()
    {
        Init();
    }

    #region 오브젝트 바인드
    public void BindText(Type type) { Bind<TextMeshProUGUI>(type); }
    public void BindButton(Type type) { Bind<Button>(type); }
    public void BindImage(Type type) { Bind<Image>(type); }
    public void BindObject(Type type) { Bind<GameObject>(type); }

    void Bind<T>(Type type) where T : Object
    {
        string[] names = Enum.GetNames(type);
        Object[] objects = new Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            T child = Custom.FindChild<T>(gameObject, names[i]);
            objects[i] = child;

            // 해당 오브젝트가 없는 경우 로그 출력
            if (objects[i] == null)
                Debug.Log($"Bind Error: Wrong name {names[i]}");
        }
    }
    #endregion

    #region 오브젝트 찾기
    public Button GetButton(int idx) { return Get<Button>(idx); }
    public TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }

    T Get<T>(int idx) where T : Object
    {
        Object[] target = null;
        if (_objects.TryGetValue(typeof(T), out target) == false)
            return null;

        return target[idx] as T;
    }
    #endregion

    #region 이벤트 바인드
    public void BindEvent(Action callback, EventType type = EventType.OnClick)
    {
        UI_EventHandler handler = Custom.GetOrAddComponent<UI_EventHandler>(gameObject);
        switch(type)
        {
            case EventType.OnClick:
                handler.OnClickedHandler -= callback;
                handler.OnClickedHandler += callback;
                break;
            case EventType.OnClickDown:
                handler.OnClickDownHandler -= callback;
                handler.OnClickDownHandler += callback;
                break;
            case EventType.OnClickUp:
                handler.OnClickUpHandler -= callback;
                handler.OnClickUpHandler += callback;
                break;
            case EventType.OnPressed:
                handler.OnPressedHandler -= callback;
                handler.OnPressedHandler += callback;
                break;
        }
    }
    #endregion
}
