using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamedEnumArrayAttribute : PropertyAttribute
{
    public string[] names;

    public NamedEnumArrayAttribute(Type enumType)
    {
        names = Enum.GetNames(enumType);
    }
}
