using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System.IO;
using Unity.VisualScripting;
using System.Linq;

class DataWeapon
{
    public string name { get; set; }
    public float shotPerSecond { get; set; }
    public float damagePerBullet { get; set; }
    public int bulletPerShot { get; set; }
    public int bulletPerMagasine { get; set; }
    public int bulletperStock { get; set; }

    public int dropRange { get; set; }
    public string assetName { get; set; }
    public string description { get; set; }
    public Rarity rarity { get; set; }
    public Type type { get; set; }
    public AmmoType damageType { get; set; }
}

public class WeaponLoader : MonoBehaviour
{
    [MenuItem("Tools/WeaponLoader")]
    static void LoadWeapon()
    {

        DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "/Files/BWeap");
        foreach (FileInfo item in dirInfo.GetFiles())
        {
            // do not read the .Meta
            if(item.Extension == ".json")
            {
                DataWeapon weapon = new DataWeapon();

                using (StreamReader sr = new StreamReader(item.FullName))
                {
                    weapon = JsonConvert.DeserializeObject<DataWeapon>(sr.ReadToEnd());

                    DirectoryInfo dirTest = new DirectoryInfo(Application.dataPath + "/Prefabs/Weapons/" + weapon.name + ".prefab");

                    if (!dirTest.Exists)
                    {
                        
                        string dirMesh = ("Assets/Model/Weapon/" + weapon.name + ".fbx");
                        Debug.Log(dirMesh);
                        var weapMesh = AssetDatabase.LoadAllAssetsAtPath(dirMesh);
                        Material mat = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/ou.mat");
                        Mesh mesh = (Mesh)weapMesh.First(weapMesh => weapMesh is Mesh);
                        

                        GameObject go = new GameObject();
                        go.name = weapon.name;
                        go.AddComponent<Weapon>();
                        Weapon weaponI = go.GetComponent<Weapon>();
                        WeaponStats weaponStats = new WeaponStats();
                        weaponStats.rarity = weapon.rarity;
                        weaponStats.type = weapon.type;
                        weaponStats.damageType = weapon.damageType;
                        weaponStats.damagePerBullet = (int)weapon.damagePerBullet;
                        weaponStats.bulletPerStock = weapon.bulletperStock;
                        weaponStats.bulletPerShot = weapon.bulletPerShot;
                        weaponStats.bulletPerMagazine = weapon.bulletPerMagasine;
                        weaponStats.shotsPerSecond = weapon.shotPerSecond;
                        weaponStats.name = weapon.name;
                        weaponStats.dropRange = weapon.dropRange;

                        //weaponI.weaponStats = weaponStats;

                        go.AddComponent<MeshRenderer>();
                        MeshRenderer mr = go.GetComponent<MeshRenderer>();
                        mr.material = mat;
                        go.AddComponent<MeshFilter>();
                        MeshFilter mf = go.GetComponent<MeshFilter>();
                        mf.mesh = mesh;
                        go.AddComponent<Rigidbody>();
                        go.AddComponent<BoxCollider>();

                        bool creationSucess;

                        PrefabUtility.SaveAsPrefabAssetAndConnect(go, dirTest.FullName, InteractionMode.UserAction, out creationSucess);

                        if (creationSucess)
                        {
                            Debug.Log(go.name + " have sucessfully get prefabed");
                        }
                        else
                        {
                            Debug.Log("!!!!!!!!!!!!!!!!!!!!" + go.name + " CRITICAL ERROR WHEN CREATING PREFAB");
                        }

                        DestroyImmediate(go);
                    }
                    else
                    {
                        Debug.Log(weapon.name + "Already Prefabed");
                    }
                   
                   
                }
            }
           
        }
    }
}
