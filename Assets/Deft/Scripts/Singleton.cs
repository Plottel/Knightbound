using UnityEngine;

namespace Deft
{
    public abstract class Singleton : MonoBehaviour
    {
        public virtual void OnAwake() { }
        public virtual void OnStart() { }
        public virtual void OnUpdate() { }
        public virtual void OnLateUpdate() { }
    }
}
