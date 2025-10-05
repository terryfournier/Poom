using UnityEditor;
using UnityEngine;
using Unity.AI.Navigation;
using System.Drawing;

public class NavMeshLinksCreator : EditorWindow
{
    private Vector2 scrollPosition;
    private SerializedObject serializedObject;
    private string goZoneName;
    
    [MenuItem("Window/Nav Mesh Links Creator")]
    public static void ShowWindow()
    {
        GetWindow<NavMeshLinksCreator>("Nav Mesh Links Creator");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Inspector Content Viewer", EditorStyles.boldLabel);

        if (Selection.activeObject != null)
        {
            serializedObject = new SerializedObject(Selection.activeObject);
            SerializedProperty property = serializedObject.FindProperty("m_OffMeshLinks");


            SerializedProperty propertyPosition = serializedObject.FindProperty("m_Position");

            if (property != null && property.isArray)
            {
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                EditorGUILayout.LabelField("m_OffMeshLinks Positions:", EditorStyles.boldLabel);
                for (int i = 0; i < property.arraySize; i++)
                {
                    SerializedProperty element = property.GetArrayElementAtIndex(i);
                    SerializedProperty startProperty = element.FindPropertyRelative("m_Start");
                    SerializedProperty endProperty = element.FindPropertyRelative("m_End");


                    if (startProperty != null && endProperty != null)
                    {
                        Vector3 startPosition = startProperty.vector3Value;
                        Vector3 endPosition = endProperty.vector3Value;

                        EditorGUILayout.LabelField($"Element {i}: Start {startPosition} -> End {endPosition}");
                    }
                }

                EditorGUILayout.EndScrollView();

                goZoneName = EditorGUILayout.TextField("ParentsObjectName", goZoneName);

                if (GUILayout.Button(" Create Links "))
                {
                    Creator();
                }

            }
            else
            {
                EditorGUILayout.LabelField("Property 'm_OffMeshLinks' not found or not an array.");
            }
        }
        else
        {
            EditorGUILayout.LabelField("No object selected.");
        }
    }

    private void Creator()
    {
        serializedObject = new SerializedObject(Selection.activeObject);
        SerializedProperty property = serializedObject.FindProperty("m_OffMeshLinks");


        SerializedProperty propertyPosition = serializedObject.FindProperty("m_Position");

        GameObject goZone = GameObject.Find(goZoneName);

        if(goZone != null)
        {
            if(GameObject.Find("GenerateNavMeshLink") == null)
            {
                GameObject goLink = new GameObject("GenerateNavMeshLink");
                goLink.transform.parent = goZone.transform;
                goLink.transform.position = Vector3.zero;

                for (int i = 0; i < property.arraySize; i++)
                {
                    SerializedProperty element = property.GetArrayElementAtIndex(i);
                    SerializedProperty startProperty = element.FindPropertyRelative("m_Start");
                    SerializedProperty endProperty = element.FindPropertyRelative("m_End");

                    if (startProperty != null && endProperty != null)
                    {
                        Vector3 startPosition = startProperty.vector3Value + propertyPosition.vector3Value;
                        Vector3 endPosition = endProperty.vector3Value + propertyPosition.vector3Value;

                        GameObject go = new GameObject("Links " + i);
                        go.transform.parent = goLink.transform;
                        NavMeshLink link = go.AddComponent<NavMeshLink>();

                        link.startPoint = startPosition;
                        link.endPoint = endPosition;
                        link.bidirectional = true;

                        go.AddComponent<LinkBehaviour>();
                    }
                }
            }
            else
            {
                Debug.Log(" already created ");
            }
        }
        else
        {
            Debug.Log("Not a good name");
        }
    }
}
