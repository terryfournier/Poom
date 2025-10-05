using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;

public class ActionInputField_Keyboard : MonoBehaviour
{
    //[SerializeField] private KeyboardActions currentAction = KeyboardActions.NONE;

    //private TMP_InputField inputField = null;
    //private GameKeysManager keysManager = null;
    //private string actionString = "";

    //// Awake is called once before all other methods, when the GameObject is created
    //private void Awake()
    //{
    //    Debug.Log(currentAction);
    //    Debug.Log(currentAction.ToString());

    //    inputField = GetComponent<TMP_InputField>();
    //    inputField.text = currentAction.ToString();
    //}

    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //private void Start()
    //{
    //    keysManager = GameObject.Find("Game Keys Manager(Clone)").GetComponent<GameKeysManager>();
    //}

    //// Update is called once per frame
    //private void Update()
    //{
    //    actionString = currentAction.ToString();

    //    if (inputField.text != actionString)
    //    {
    //        //KeyControl keyToChange = null;
    //        //string newKeyAsString = "";

    //        //for (int i = 0; i < Keyboard.current.allKeys.Count; i++)
    //        //{
    //        //    newKeyAsString = "key:/keyboard/" + inputField.text;

    //        //    if (actionString == newKeyAsString)
    //        //    {
    //        //        keyToChange = 
    //        //    }
    //        //}

    //        //ControlsManager.Remap_Keyboard((int)(currentAction), )

    //        inputField.text = actionString;
    //    }
    //}
}
