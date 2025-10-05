using UnityEngine;
using UnityEditor;
using UnityEditor.TerrainTools;

[CustomEditor(typeof(Trophy))]
public class TrophyCustomEditor : Editor
{
    SerializedProperty propertyItem;

    private void OnEnable()
    {
        propertyItem = serializedObject.FindProperty("Trophy");
    }

    public override void OnInspectorGUI()
    {
        
    }

    void AddTrophy(Trophy trophy)
    {
        serializedObject.Update();

        int arraySize = propertyItem.arraySize;
        propertyItem.InsertArrayElementAtIndex(arraySize);
        propertyItem.GetArrayElementAtIndex(arraySize).objectReferenceValue = trophy;

        serializedObject.ApplyModifiedProperties();
    }
}
