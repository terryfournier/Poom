using UnityEngine;
using UnityEngine.VFX;

public class TextureSelector : MonoBehaviour
{
    [SerializeField] private Texture2D[] textureRand;
    VisualEffect vfx;

    private bool canReset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vfx = GetComponent<VisualEffect>();
        Texture2D temp = textureRand[Random.Range(0, textureRand.Length)];
        vfx.SetTexture("BloodTex", temp);

        canReset = true;
    }


    //private void Update()
    //{
    //    if(vfx.HasAnySystemAwake() && canReset)
    //    {
    //        Texture2D temp = textureRand[Random.Range(0, textureRand.Length)];
    //        vfx.SetTexture("BloodTex", temp);
    //        canReset = false;
    //    }
    //    else
    //    {
    //        canReset = true;
    //    }
    //}
}
