using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tool
{
    static public T Pop<T>(this List<T> list)
    {
        T result = default(T);
        int index = list.Count - 1;
        if (index >= 0)
        {
            result = list[index];
            list.RemoveAt(index);
            return result;
        }
        return result;
    }
}