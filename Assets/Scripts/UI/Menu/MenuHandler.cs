using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{


    [SerializeField] private KeyCode optionsSelectorTriggerButton = KeyCode.Escape;
    [SerializeField] private GameObject optionsSelectorGameObject;

    private const string APPEAR_ANIM_NAME = "appear";
    private bool currentOptionSelectorState;
    private Animator optionsSelectorAnimator;

    private void Start()
    {
        optionsSelectorAnimator = optionsSelectorGameObject.GetComponent<Animator>();
    }

    public void HandleOptionsSelectorVisibility()
    {
        currentOptionSelectorState = !currentOptionSelectorState;
        optionsSelectorAnimator.SetBool(APPEAR_ANIM_NAME, currentOptionSelectorState);
    }

    public void OpenOptionsButton()
    {
        optionsSelectorGameObject.SetActive(true);
    }

    public void CloseOptionsButton()
    {
        optionsSelectorGameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(optionsSelectorTriggerButton)) 
        {
            currentOptionSelectorState = !currentOptionSelectorState;
            optionsSelectorGameObject.SetActive(currentOptionSelectorState);
        }
    }
}
