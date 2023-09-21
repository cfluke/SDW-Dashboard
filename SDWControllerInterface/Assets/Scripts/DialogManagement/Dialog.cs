using System;
using UnityEngine;

namespace DialogManagement
{
    public abstract class Dialog<T, TParams> : MonoBehaviour where TParams : class
    {
        public Action<T> OnConfirm;

        public abstract void Init(TParams parameters);
        public abstract void Confirm();
        public abstract void Cancel();
    }
} 