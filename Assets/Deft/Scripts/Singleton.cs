using UnityEngine;

namespace Deft
{
    public abstract class Singleton : MonoBehaviour
    {
        public abstract void EnsureInstance();

        public virtual void OnAwake() { }
        public virtual void OnStart() { }
        public virtual void OnUpdate() { }
        public virtual void OnLateUpdate() { }
    }
}
