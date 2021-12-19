using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static bool TryFindTransform(this Transform transform, string name, out Transform child)
    {
        child = transform.FindTransform(name);
        return child != null;
    }

    public static Transform FindTransform(this Transform transform, string name)
    {
        foreach (Transform child in transform)
        {
            if (child.name == name)
                return child;
            else
            {
                Transform found = child.FindTransform(name);
                if (found != null)
                    return found;
            }
        }

        return null;
    }

    public static T Find<T>(this Transform transform, string name) where T : Object
    {
        foreach (Transform child in transform)
        {
            if (child.name == name)
                return child.GetComponent<T>();
            else
            {
                T found = child.Find<T>(name);
                if (found != null)
                    return found;
            }
        }

        return null;
    }

    public static T[] FindAll<T>(this Transform transform, bool includeInactive = false) where T : Object
    {
        var queue = new Queue<Transform>();
        var result = new List<T>();
        Transform current;

        queue.Enqueue(transform);

        while (queue.Count > 0)
        {
            current = queue.Dequeue();

            foreach (Transform child in current)
                queue.Enqueue(child);

            T component = current.GetComponent<T>();

            if (component != null && (current.gameObject.activeInHierarchy || includeInactive)) 
                result.Add(component);          
        }

        return result.ToArray();
    }
}
