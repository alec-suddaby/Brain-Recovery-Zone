using System;
using UnityEngine;

namespace Elixr.MenuSystem
{

    [System.Serializable]
    public abstract class Node : ICloneable
    {
        public enum State
        {
            Running,
            Failure,
            Success
        }

        [HideInInspector] public State state = State.Running;
        [HideInInspector] public bool started = false;
        [HideInInspector] public string guid = System.Guid.NewGuid().ToString();
        [HideInInspector] public Vector2 position;
        [HideInInspector] public Context context;
        [HideInInspector] public Blackboard blackboard;
        public string title;
        [TextArea] public string description;
        [HideInInspector] public bool drawGizmos = false;

        [SerializeField] protected virtual bool backOnSceneReload => true;
        public bool BackOnSceneReload => backOnSceneReload;

        [field: SerializeField]
        public virtual bool isEnabled { get; set; } = true;

        public State Update()
        {

            if (!started)
            {
                OnStart();
                started = true;
            }

            state = OnUpdate();

            if (state != State.Running)
            {
                OnStop();
                started = false;
            }

            return state;
        }

        public void Abort()
        {
            BehaviourTree.Traverse(this, (node) =>
            {
                node.started = false;
                node.state = State.Running;
                node.OnStop();
            });
        }

        public virtual void OnDrawGizmos() { }

        protected virtual void OnStart()
        {
            blackboard.ActiveNode = this;
        }
        protected abstract void OnStop();
        protected abstract State OnUpdate();

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}