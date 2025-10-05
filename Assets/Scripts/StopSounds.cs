using FMODUnity;
using UnityEngine;

public class StopSounds : MonoBehaviour
{
    private LoadLevelBenitar[] llB;

    private StudioEventEmitter emitter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        llB = FindObjectsByType<LoadLevelBenitar>(FindObjectsSortMode.None);

        emitter = GetComponent<StudioEventEmitter>();

        foreach(var benitar in llB)
        {
            benitar.desactivateSounds += Desactivate;
        }
    }

   private void Desactivate()
    {
        if(gameObject != null)
        {
            emitter.enabled = false;
        }
       
    }

    private void OnDestroy()
    {
        foreach (var benitar in llB)
        {
            benitar.desactivateSounds -= Desactivate;
        }
    }
}
