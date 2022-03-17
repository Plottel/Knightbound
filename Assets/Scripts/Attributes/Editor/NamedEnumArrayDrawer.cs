using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(NamedEnumArrayAttribute))]
public class NamedEnumArrayDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        NamedEnumArrayAttribute enumNames = attribute as NamedEnumArrayAttribute;

        string path = property.propertyPath;
        Debug.Log(path);

        int index = System.Convert.ToInt32(property.propertyPath.Substring(property.propertyPath.IndexOf("[")).Replace("[", "").Replace("]", ""));

        label.text = enumNames.names[index];

        EditorGUI.PropertyField(position, property, label, false);
    }
}
