using UnityEngine;




public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public enum Type
{
    Mellee,
    Shotgun,
    Gun,
    Rifle,
    HuntingRifle,
    Mystical

}


public enum AmmoType
{
    Classic,
    Poison,
    Bleeding,
    Fire
}

[System.Serializable]
public class WeaponStats
{

    public string name;
    public float shotsPerSecond;
    public int damagePerBullet;
    public int bulletPerShot;
    public int bulletPerMagazine;
    public int bulletPerStock;
    public int dropRange;
    public string assetName;
    public string description;
    public Rarity rarity;
    public Type type;
    public AmmoType damageType;
    public bool canShoot = true;


    public WeaponStats()
    {
    }


    public void LoadFromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }




}
