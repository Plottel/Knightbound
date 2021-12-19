using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumArrayAttribute : PropertyAttribute
{
    public string[] names;

    public EnumArrayAttribute(Type enumType)
    {
        names = Enum.GetNames(enumType);
    }
}
