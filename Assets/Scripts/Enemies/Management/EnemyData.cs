public class EnemyData
{
    //Constructor of EnemyData with every values that way we can protect all of them 
    public EnemyData(EnemyType _typeEn, float _lifePoints, float _damage, float _speed, float _cooldown, float _distanceAttack, string _assetName)
    {
        TypeEn = _typeEn;
        LifePoints = _lifePoints; 
        Damage = _damage;
        Speed = _speed;
        Cooldown = _cooldown;
        DistanceAttack = _distanceAttack;
        AssetName = _assetName;
    }
    public EnemyType TypeEn { get; private set; } = 0;

    public float LifePoints;
    public float Damage { get; private set; } = 0.0f;
    public float Speed { get; private set; } = 0.0f;
    public float Cooldown { get; private set; } = 0.0f;
    public float DistanceAttack { get; private set; } = 0.0f;
    public string AssetName { get; private set; } = "";
}
