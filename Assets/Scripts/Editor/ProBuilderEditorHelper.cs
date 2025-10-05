using System;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEditor.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;
using UObject = UnityEngine.Object;

static class ProBuilderEditorHelper
{
    /// <summary>
    /// Menu interface for manually re-generating all ProBuilder geometry in scene.
    /// </summary>
    [MenuItem("Tools/ProBuilder/Helper/Rebuild Empty ProBuilderMesh Components", false)]
    public static void MenuForceSceneRefresh()
    {
        StringBuilder sb = new StringBuilder();
        ProBuilderMesh[] all = UObject.FindObjectsByType<ProBuilderMesh>(FindObjectsSortMode.None);

        for (int i = 0, l = all.Length; i < l; i++)
        {
            UnityEditor.EditorUtility.DisplayProgressBar(
                "Rebuild ProBuilder Objects",
                $"Rebuilding ProBuilderMesh {all[i].name}.",
                (float)i / l);

            var mf = all[i].GetComponent<MeshFilter>();
            var sm = mf == null ? null : mf.sharedMesh;

            try
            {
                if (sm != null && sm.vertexCount > 0 && all[i].vertexCount < 1)
                {
                    var mesh = all[i];

                    // FIX: Create MeshImporter from GameObject, then call Import() directly
                    MeshImporter importer = new MeshImporter(mesh.gameObject);
                    importer.Import();

                    mesh.ToMesh();
                    mesh.Refresh();
                    mesh.Optimize();
                }
            }
            catch (Exception e)
            {
                sb.AppendLine($"Failed rebuilding: {all[i]}\n\t{e}");
            }
        }

        if (sb.Length > 0)
            Debug.LogError(sb.ToString());

        UnityEditor.EditorUtility.ClearProgressBar();
        UnityEditor.EditorUtility.DisplayDialog(
            "Refresh ProBuilder Objects",
            "Successfully refreshed all ProBuilder objects in scene.",
            "Okay");
    }

    /// <summary>
    /// Bake the mesh of all ProBuilder GameObject in Selection
    /// => Convert it into Unity mesh
    /// </summary>
    [MenuItem("Tools/ProBuilder/Helper/Bake Selection Mesh", false)]
    public static void BakeSelectionMesh()
    {
        if (!Selection.activeGameObject) { return; }

        Transform current = null;
        Transform current2 = null;
        ProBuilderMesh proBuilderMesh = null;
        int childCount = 0;
        int childCount2 = 0;

        for (int i = 0; i < Selection.count; i++)
        {
            current = Selection.gameObjects[i].transform;
            proBuilderMesh = current.GetComponent<ProBuilderMesh>();
            childCount = current.childCount;

            if (proBuilderMesh) { proBuilderMesh.ToMesh(); }

            if (childCount > 0)
            {
                for (int j = 0; j < childCount; j++)
                {
                    proBuilderMesh = current.GetChild(j).GetComponent<ProBuilderMesh>();

                    if (proBuilderMesh) { proBuilderMesh.ToMesh(); }

                    current2 = current.GetChild(j);
                    childCount2 = current2.transform.childCount;

                    if (childCount2 > 0)
                    {
                        for (int k = 0; k < childCount2; k++)
                        {
                            proBuilderMesh = current2.GetChild(k).GetComponent<ProBuilderMesh>();

                            if (proBuilderMesh) { proBuilderMesh.ToMesh(); }
                        }
                    }
                }
            }
        }
    }
}
