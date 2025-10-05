using System.Collections;
using UnityEngine;

public class LaunchBenitarAnim : MonoBehaviour
{
    private GameObject child; // The child object bearer of animation
    private Animator animator;
    private Transform anchor;
    private GameObject benitar;
    private Benitar beniScript;



    private void Awake()
    {
        // Find the child object with the name "Benitar" and assign it to the child variable
        child = transform.Find("AnimModel").gameObject;
        animator = child.GetComponent<Animator>();
    }


    public void InteractBenitar(GameObject benitar)
    {
        //get main camera attach to self
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // Set the position of the child object to the position of the main camera
            child.transform.position = mainCamera.transform.position;
            mainCamera.transform.parent = this.transform.gameObject.transform.Find("AnimModel").transform;

        }
        anchor = benitar.transform;
        this.benitar = benitar;
        Linker();

        //Launch animation
        animator.SetTrigger("PlayAnim");

        StartCoroutine(WaitEndAnim());



    }


    private void Linker()
    {
        this.transform.position = anchor.position;
        this.transform.rotation = anchor.rotation;
    }


    private IEnumerator WaitEndAnim()
    {
        yield return new WaitForSeconds(2.0f);

        BenitarLoadLevel();

        yield return null;
    }

    private void BenitarLoadLevel()
    {
        benitar.GetComponent<Benitar>().LoadLevel();
    }




}
