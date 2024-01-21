using System;
using System.Runtime.InteropServices.WindowsRuntime;


#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

#endif

public class DynamicRangeAttribute : PropertyAttribute
{
    public string MinVar { get; }
	public string MaxVar { get; }

	public DynamicRangeAttribute(string minVar, string maxVar)
	{
		MinVar = minVar;
		MaxVar = maxVar;
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(DynamicRangeAttribute))]
public class DynamicRangeAttributePropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		DynamicRangeAttribute range = attribute as DynamicRangeAttribute;

		EditorGUI.BeginProperty(position, label, property);

		SerializedProperty minProp = property.serializedObject.FindProperty(range.MinVar);
		SerializedProperty maxProp = property.serializedObject.FindProperty(range.MaxVar);
		if (minProp == null || maxProp == null)
		{
			EditorGUI.LabelField(position, label.text, "DynamicRange could not find Min or Max var.");
		}
		else if (!PropertyTypesMatch(property, minProp, maxProp))
		{
			EditorGUI.LabelField(position, label.text, "DynamicRange min and max props must match type of base property.");
		}
		else
		{
			if (property.propertyType == SerializedPropertyType.Float)
				EditorGUI.Slider(position, property, minProp.floatValue, maxProp.floatValue);
			else if (property.propertyType == SerializedPropertyType.Integer)
				EditorGUI.IntSlider(position, property, minProp.intValue, maxProp.intValue);
			else
				EditorGUI.LabelField(position, label.text, "Use DynamicRange with float or int.");
		}

		EditorGUI.EndProperty();

		static bool PropertyTypesMatch(SerializedProperty baseProp, SerializedProperty minProp, SerializedProperty maxProp)
		{
			bool allInt = baseProp.propertyType == SerializedPropertyType.Integer
				&& minProp.propertyType == SerializedPropertyType.Integer
				&& maxProp.propertyType == SerializedPropertyType.Integer;
			bool allFloat = baseProp.propertyType == SerializedPropertyType.Float
				&& minProp.propertyType == SerializedPropertyType.Float
				&& maxProp.propertyType == SerializedPropertyType.Float;

			return allInt || allFloat;
		}
	}
}
#endif
