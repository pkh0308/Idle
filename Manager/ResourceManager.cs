using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Object = UnityEngine.Object;

public class ResourceManager
{
    Dictionary<string, Object> _resources = new Dictionary<string, Object>();
    Dictionary<string, AsyncOperationHandle> _handles = new Dictionary<string, AsyncOperationHandle>();
    public int HandleCount { get; private set; }

    public void Init() { }

    #region 리소스 로드
    public void Load<T>(string key, Action<T> callback) where T : Object
    {
        if (_resources.TryGetValue(key, out Object resource))
        {
            callback?.Invoke(resource as T);
            return;
        }

        if (_handles.TryGetValue(key, out AsyncOperationHandle handle))
        {
            handle.Completed += (op) =>
            {
                callback?.Invoke(op.Result as T);
            };
            return;
        }

        _handles.Add(key, Addressables.LoadAssetAsync<T>(key));
        HandleCount++;
        _handles[key].Completed += (op) =>
        {
            callback?.Invoke(op.Result as T);
        };
    }

    public void LoadByIdx<T>(string key, int idx, Action<T, int> callback) where T : Object
    {
        if (_resources.TryGetValue(key, out Object resource))
        {
            callback?.Invoke(resource as T, idx);
            return;
        }

        if (_handles.TryGetValue(key, out AsyncOperationHandle handle))
        {
            handle.Completed += (op) =>
            {
                callback?.Invoke(op.Result as T, idx);
            };
            return;
        }

        _handles.Add(key, Addressables.LoadAssetAsync<T>(key));
        HandleCount++;
        _handles[key].Completed += (op) =>
        {
            callback?.Invoke(op.Result as T, idx);
        };
    }
    #endregion

    #region 프리팹 로드
    public void Instantiate(string key, Transform parent = null, Action<GameObject> callback = null)
    {
        Addressables.InstantiateAsync(key, parent).Completed += (op) =>
        {
            callback?.Invoke(op.Result);
        };
    }

    public void InstantiateByIdx(string key, int idx, Transform parent = null, Action<GameObject, int> callback = null)
    {
        Addressables.InstantiateAsync(key, parent).Completed += (op) =>
        {
            callback?.Invoke(op.Result, idx);
        };
    }
    #endregion

    #region 해제
    public void Release(string key)
    {
        if (_resources.ContainsKey(key) == false)
            return;
        _resources.Remove(key);

        if (_handles.ContainsKey(key) == false)
            return;
        _handles.Remove(key);
    }

    public void Destroy(GameObject go)
    {
        Object.Destroy(go);
        Addressables.ReleaseInstance(go);
    }

    public void Clear()
    {
        _resources.Clear();

        foreach (AsyncOperationHandle handle in _handles.Values)
            Addressables.Release(handle);
        _handles.Clear();
    }
    #endregion
}