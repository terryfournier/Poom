using UnityEngine;

public class KeyHolder : MonoBehaviour
{
    [SerializeField] EnemyType m_enemyType;

    public string Key { get; private set; } = "CLASSIC" + "CLASSIC";
    public EnemyType Type
    {
        get => m_enemyType;
    }

    void Awake()
    {
        Key = m_enemyType + "CLASSIC";
    }
}
