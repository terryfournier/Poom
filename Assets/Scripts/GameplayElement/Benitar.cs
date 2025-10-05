using System.Collections;
using TMPro;
using UnityEngine;

public class Benitar : MonoBehaviour
{
    [SerializeField]
    private string LevelToLoadName;
    [SerializeField]
    private LoadLevelBenitar loadLevelB;

    [SerializeField] private Transform m_armSpawnPoint;

    [SerializeField] private GameObject m_armPrefab;
    private float m_waitingTime = 1.0f;

    private GameObject endLevelAnim;

    bool isLoading = false;
    private bool animEnded = false;
    [SerializeField]
    private bool isDesactivated;
    [SerializeField]
    Material activated;
    [SerializeField]
    Material desactivated;
    [SerializeField]
    SaveEndLevel saveEndLevel;

    private ControlsManager keysManager = null;

    private TMP_Text interract;


    private void Start()
    {
        keysManager = GameObject.Find("Game Keys Manager(Clone)").GetComponent<ControlsManager>();
        interract = GameObject.Find("Interract").GetComponent<TMP_Text>();
        endLevelAnim = GameObject.Find("AnimBenitarPrefab");

        loadLevelB.LevelToLoadName = LevelToLoadName;
        isDesactivated = !GameManager.instance.isLevelOpen[LevelToLoadName];
        if (isDesactivated)
        {
            //GetComponent<Renderer>().materials[1] = desactivated;
            //GetComponent<Renderer>().materials[0] = desactivated;
            //GetComponent<Renderer>().materials[2] = desactivated;
            GetComponent<MeshRenderer>().materials[1] = desactivated;
            GetComponent<MeshRenderer>().materials[0] = desactivated;
            GetComponent<MeshRenderer>().materials[2] = desactivated;
        }
        else
        {
            
            //GetComponent<Renderer>().materials[1] = activated;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && (
            ControlsManager.HasPressed_Keyboard(KeyboardActions.INTERACT, false)
            || ControlsManager.HasPressed_Gamepad(GamepadActions.INTERACT, false)
            )
            && !isLoading && !isDesactivated)
        {
            //endLevelAnim.GetComponent<LaunchBenitarAnim>().InteractBenitar(gameObject);

            StartCoroutine(animLevelLoader());

            //interract.enabled = false;
            //if (saveEndLevel != null)
            //{
            //    saveEndLevel.Save();
            //}
            //loadLevelB.LaunchCoroutine();
            isLoading = true;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            interract.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            interract.enabled = false;
        }
    }


    private IEnumerator HandleAnimBenitar()
    {
        while (!animEnded)
        {
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);

        if (saveEndLevel != null)
        {
            saveEndLevel.Save();
        }
        loadLevelB.LaunchCoroutine();
        isLoading = true;
    }

    public void EndAnimBenitar()
    {
        animEnded = true;
    }

    private IEnumerator animLevelLoader()
    {
        GameObject go = Instantiate(m_armPrefab, m_armSpawnPoint.position, m_armSpawnPoint.rotation);
        yield return new WaitForSeconds(go.GetComponent<ArmBenitar>().GetAnimLength() + m_waitingTime);
        LoadLevel();
    }

    public void LoadLevel()
    {
        interract.enabled = false;
        if (saveEndLevel != null)
        {
            saveEndLevel.Save();
        }
        loadLevelB.LaunchCoroutine();
        isLoading = true;
    }

}
