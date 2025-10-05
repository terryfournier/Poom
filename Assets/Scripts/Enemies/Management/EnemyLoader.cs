using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyLoader : MonoBehaviour
{
    struct dataReader
    {
        public EnemyType monsterType;
        public float lifePoints;
        public float damage;
        public float speed;
        public float attackSpeed;
        public float attackDistance;
        public DamageType damageType;
        public string assetName;
        public string Description;
    }

    string enemyDataPath = Application.streamingAssetsPath + "/Enemy/";

    //List of all the enemy in the game 
    private Dictionary<string, EnemyData> dicoEnemy = new Dictionary<string, EnemyData>();
    public Dictionary<string, EnemyData> DicoEnemy { get => dicoEnemy; }

    void Awake()
    {
        //Check if the folder exist 
        if (Directory.Exists(enemyDataPath))
        {
            //we get the info of the folder than in everyFile that will permit to open every file 
            DirectoryInfo enemyFolder = new DirectoryInfo(enemyDataPath);
            foreach (FileInfo file in enemyFolder.GetFiles())
            {
                if (file.Extension == ".json")
                {
                    LoadEnemy(file.FullName);
                }
            }
        }
    }

    private void LoadEnemy(string _pathEnemy)
    {
        // if( the file does exist we read every data in it and than we save them in an enemyData 
        if (File.Exists(_pathEnemy))
        {
            dataReader jSonEnemyData;
            using StreamReader strRead = new StreamReader(_pathEnemy);
            {
                jSonEnemyData = JsonConvert.DeserializeObject<dataReader>(strRead.ReadToEnd());
            }
            EnemyData enemyTry = new EnemyData(jSonEnemyData.monsterType, jSonEnemyData.lifePoints, jSonEnemyData.damage, jSonEnemyData.speed,
                jSonEnemyData.attackSpeed, jSonEnemyData.attackDistance, jSonEnemyData.assetName);

            // and finally we save the enemyData in the dictionnary for the generator
            string key = jSonEnemyData.monsterType.ToString() + jSonEnemyData.damageType.ToString();

            dicoEnemy.Add(key, enemyTry);
        }
    }

    public EnemyData GetData(string _key)
    {
        return dicoEnemy[_key];
    }
}
