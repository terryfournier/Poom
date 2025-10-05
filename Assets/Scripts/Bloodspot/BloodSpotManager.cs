using UnityEngine;

public class BloodSpotManager : MonoBehaviour
{
    [SerializeField] BloodSpot[] bloodSpots;

    public void ActivateBloodSpot()
    {
        int bloodSpotToActivate = Random.Range(0, bloodSpots.Length);

        if (bloodSpots[bloodSpotToActivate].gameObject.activeInHierarchy == false)
        {
            bloodSpots[bloodSpotToActivate].gameObject.SetActive(true);
            bloodSpots[bloodSpotToActivate].currentDurationOnScreen = 0;
        }
    }
}
