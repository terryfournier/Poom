using UnityEngine;
using UnityEngine.UI;

public class Trophy : MonoBehaviour
{
    int id { get; set; }
    string nameTrophy { get; set; }

   string description { get; set; }

    bool isAchieved { get; set; }

    Image trophyImage { get; set; }
}
