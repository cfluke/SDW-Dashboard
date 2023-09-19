using UnityEngine;
using System;
using System.Collections.Generic;

public class MainThreadDispatcher : MonoBehaviour
{
    // singleton
    private static MainThreadDispatcher _instance;

    public static MainThreadDispatcher Instance
    {
        get
        {
            if (_instance != null) 
                return _instance;
            
            Debug.Log("No MainThreadDispatcher instance found!");
            return null;
        }
    }

    // queue of actions to perform
    private readonly Queue<Action> _actions = new();

    private void Start()
    {
        _instance = this;
    }

    private void Update()
    {
        lock (_actions)
        {
            while (_actions.Count > 0)
            {
                Action action = _actions.Dequeue();
                action.Invoke();
            }
        }
    }

    public void Enqueue(Action action)
    {
        lock (_actions)
        {
            _actions.Enqueue(action);
        }
    }
}