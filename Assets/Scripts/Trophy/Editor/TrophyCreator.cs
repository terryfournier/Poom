using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TrophyCreator : EditorWindow
{
    List<Trophy> listTrophy;

    TrophyDatabase database;
    public static TrophyCreator OpenTrophyCreator()
    {
        return GetWindow<TrophyCreator>("Item Picker");
    }

    private void OnEnable()
    {
        database = Resources.Load<TrophyDatabase>("Trophy Database");
    }


    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("ID", GUILayout.Width(200));
        GUILayout.Label("Name", GUILayout.Width(200));
        GUILayout.Label("Description", GUILayout.Width(200));
        GUILayout.EndHorizontal();
    }

    void UpdateTrophyList()
    {
        listTrophy = database.trophies;
    }
}
