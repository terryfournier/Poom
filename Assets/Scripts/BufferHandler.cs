using UnityEngine;
//using UnityEngine.ProBuilder;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BufferHandler : MonoBehaviour
{
    private GameObject bloodBuffer;
    private GameObject bulletBuffer;
    private GameObject stickersBuffer;

    private GameObject[] bloodParticles;
    private GameObject[] bulletParticles;


    private int bloodIndex = 0;
    private int bulletIndex = 0;

    [SerializeField] private Sprite[] bloodSprites;
    private int bloodSpriteIndex = 0;

    //Limits of spaces available in each buffer
    private const int bloodBufferLimit = 50;
    private const int bulletBufferLimit = 50;


    private void Start()
    {
        bloodBuffer = GameObject.Find("BloodBuffer");
        bulletBuffer = GameObject.Find("BulletBuffer");
        stickersBuffer = GameObject.Find("StickersBuffer");
        LoadSprites();
        InitiateBuffers();
    }

    private void InitiateBuffers()
    {
        if (bloodBuffer != null)
        {
            bloodParticles = new GameObject[bloodBufferLimit];
            for (int i = 0; i < bloodBufferLimit; i++)
            {
                GameObject newOne = new GameObject("BloodBuffer" + i);
                newOne.transform.SetParent(bloodBuffer.transform);
                newOne.transform.position = new Vector3(0, -100, 0);
                newOne.transform.rotation = Quaternion.identity;

                bloodParticles[i] = newOne;
                bloodParticles[i].AddComponent<SpriteRenderer>();
            }
        }
    }

    private void LoadSprites()
    {
        bloodSprites = new Sprite[5];
        bloodSprites[0] = Resources.Load<Sprite>("BloodSprites/Blood_00");
        bloodSprites[1] = Resources.Load<Sprite>("BloodSprites/Blood_01");
        bloodSprites[2] = Resources.Load<Sprite>("BloodSprites/Blood_02");
        bloodSprites[3] = Resources.Load<Sprite>("BloodSprites/Blood_03");
        bloodSprites[4] = Resources.Load<Sprite>("BloodSprites/Blood_04");

    }

    public void AddBloodParticle(Vector3 _position, Vector3 _normal)
    {
        SpriteRenderer sr = bloodParticles[bloodIndex].GetComponent<SpriteRenderer>();
        if (bloodParticles[bloodIndex] == null || sr == null)
        {
            return;
        }
            sr.sprite = bloodSprites[Random.Range(0, bloodSprites.Length)];
        bloodParticles[bloodIndex].transform.position = _position + new Vector3(0, 0.001f, 0);
        bloodParticles[bloodIndex].transform.rotation = Quaternion.FromToRotation(Vector3.forward, _normal);
        bloodParticles[bloodIndex].transform.Rotate(0, 0, Random.Range(0, 360));
            bloodIndex = (bloodIndex + 1) % bloodBufferLimit;
    }

}

