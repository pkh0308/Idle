using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    protected bool _initialized = false;

    void Start()
    {
        Init();
    }

    protected virtual bool Init()
    {
        if (_initialized)
            return false;

        // EventSystem이 없다면 생성
        _initialized = true;
        GameObject es = GameObject.Find(ConstValue.EventSystem);
        if (es == null)
        {
            Managers.Resc.Instantiate(ConstValue.EventSystem, null, (op) => {
                op.name = ConstValue.EventSystem;
            });
        }
        return true;
    }
}
