using System.Collections.Generic;
using UnityEngine;

public class UIManager 
{
    Stack<UI_PopUp> _stack = new Stack<UI_PopUp>();

    public void Init()
    {
        
    }

    public void OpenPopUp<T>(string name = null, Transform parent = null) where T : UI_PopUp
    {
        if (name == null)
            name = typeof(T).Name;
        
        Managers.Resc.Instantiate(name, parent, (op) => {
            op.transform.SetParent(parent);
            _stack.Push(Custom.GetOrAddComponent<T>(op));
        });
    }

    public void OpenNotice(string notice, Transform parent = null)
    {
        string name = typeof(UI_NoticePopUp).Name;

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
        Object.Destroy(_stack.Pop().gameObject);
    }
}
