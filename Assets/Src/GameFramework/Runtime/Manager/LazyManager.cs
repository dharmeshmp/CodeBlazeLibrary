﻿using System.Collections;

using CodeBlaze.GameFramework.Behaviour;

using UnityEngine;

namespace CodeBlaze.GameFramework.Manager {

    public class LazyManager<T> where T : LazyManager<T>, new() {

        private static T _current;

        public static T Current {
            get => _current ?? (_current = new T());
            set => _current = value;
        }

        private InternalBehaviour Behaviour { get; }

        protected LazyManager() {
            Behaviour = InternalBehaviour.Create(typeof(T).Name);
            Behaviour.OnUpdateAction = OnUpdate;
            Behaviour.OnLateUpdateAction = OnLateUpdate;
            Behaviour.OnDrawGizmosAction = OnDrawGizmos;
            Behaviour.OnDestroyAction = () => {
                OnDestroy();
                Current = default;
            };
            OnInitialize(); // Could lead to problems
        }
        
        protected virtual void OnInitialize() {}
        
        protected virtual void OnDestroy() {}
        
        protected virtual void OnUpdate() {}
        
        protected virtual void OnLateUpdate() {}
        
        protected virtual void OnDrawGizmos() {}
        
        protected TChild InstantiateChild<TChild>(TChild original) where TChild : UnityEngine.Object =>
            UnityEngine.Object.Instantiate<TChild>(original, Behaviour.transform);

        protected Coroutine StartCoroutine(IEnumerator routine) => Behaviour.StartCoroutine(routine);

        protected void StopCoroutine(IEnumerator routine) => Behaviour.StopCoroutine(routine);

        protected void StopCoroutine(Coroutine routine) => Behaviour.StopCoroutine(routine);
        
    }

}