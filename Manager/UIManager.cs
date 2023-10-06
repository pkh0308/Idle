using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager 
{
    Stack<UI_PopUp> _stack = new Stack<UI_PopUp>();
    Transform _baseTransform;

    public void Init()
    {
        
    }

    // 씬의 기본이 되는 UI(캔버스 포함) 오픈
    // 공지 팝업을 띄울 _baseTransform을 해당 객체의 트랜스폼으로 설정
    public void OpenScene<T>(Action callback = null) where T: UI_PopUp
    {
        Managers.Resc.Instantiate(typeof(T).Name, null, (op) => {
            _baseTransform = op.transform;
            Custom.GetOrAddComponent<T>(op);
            callback?.Invoke();
        });
    }

    // 기타 UI 오픈
    // 닫지 않을 팝업이라면 doStack = false로 호출
    public void OpenPopUp<T>(string name = null, Transform parent = null, bool doStack = true) where T : UI_PopUp
    {
        if (name == null)
            name = typeof(T).Name;
        if (parent == null)
            parent = _baseTransform;
        
        Managers.Resc.Instantiate(name, parent, (op) => {
            op.transform.SetParent(parent);
            T target = Custom.GetOrAddComponent<T>(op);
            if(doStack)
                _stack.Push(target);
        });
    }

    public void OpenNotice(string notice, Transform parent = null)
    {
        string name = typeof(UI_NoticePopUp).Name;
        if (parent == null)
            parent = _baseTransform;

        Managers.Resc.Instantiate(name, parent, (op) => {
            UI_NoticePopUp np = Custom.GetOrAddComponent<UI_NoticePopUp>(op);
            np.Init();
            np.SetNoticeText(notice);
            _stack.Push(np);
        });
    }

    public void ClosePopUp(UI_PopUp popUp)
    {
        if (_stack.Count == 0)
            return;

        if(_stack.Peek() != popUp)
        {
            Debug.Log("There is another PopUp on top");
            return;
        }
        UnityEngine.Object.Destroy(_stack.Pop().gameObject);
    }

    public bool ClosePopUp()
    {
        if (_stack.Count == 0)
            return false;

        UnityEngine.Object.Destroy(_stack.Pop().gameObject);
        return true;
    }

    public void ClearStack()
    {
        _stack.Clear();
    }
}