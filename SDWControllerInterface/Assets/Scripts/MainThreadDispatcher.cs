using UnityEngine;
using System;
using System.Collections.Generic;

public class MainThreadDispatcher : MonoBehaviour
{
    // singleton
    private static MainThreadDispatcher _instance;

    private readonly Queue<Action> actions = new Queue<Action>();

    private static bool _applicationIsQuitting = false;

    public static MainThreadDispatcher Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning("MainThreadDispatcher.Instance is already destroyed and cannot be accessed.");
                return null;
            }

            if (_instance == null)
            {
                // TODO: change this to cache singleton instance at runtime instead - needs to instantiate itself in the main thread
                GameObject go = new GameObject("MainThreadDispatcher");
                _instance = go.AddComponent<MainThreadDispatcher>();
                DontDestroyOnLoad(go);
            }

            return _instance;
        }
    }

    private void Update()
    {
        lock (actions)
        {
            while (actions.Count > 0)
            {
                Action action = actions.Dequeue();
                action.Invoke();
            }
        }
    }

    public void Enqueue(Action action)
    {
        lock (actions)
        {
            actions.Enqueue(action);
        }
    }

    private void OnDestroy()
    {
        _applicationIsQuitting = true;
    }
}