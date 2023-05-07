using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum UIBtnType{
        COOK,
        COLLECT,
        DELIVER,
        INTERACT
    }
    public static UIManager Instance
    {
        get;
        private set;
    }
    [SerializeField] private Button cookBtn;
    [SerializeField] private Button collectBtn;
    [SerializeField] private Button deliverBtn;
    [SerializeField] private Button interactBtn;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        //ShowBtn(UIBtnType.INTERACT);
        //HideBtn(UIBtnType.COOK);
        //HideBtn(UIBtnType.COLLECT);
        //HideBtn(UIBtnType.DELIVER);
        //HideBtn(UIBtnType.INTERACT);

    }


    public void CheckSetUI()
    {
        ShowBtn(UIBtnType.INTERACT);
        
    }

    public void ShowBtn(UIBtnType uiBtnType)
    {
        switch (uiBtnType)
        {
            case UIBtnType.COOK:
                cookBtn.gameObject.SetActive(true);
                collectBtn.gameObject.SetActive(false);
                deliverBtn.gameObject.SetActive(false);
                break;
            case UIBtnType.COLLECT:
                cookBtn.gameObject.SetActive(false);
                collectBtn.gameObject.SetActive(true);
                deliverBtn.gameObject.SetActive(false);
                break;
            case UIBtnType.DELIVER:
                cookBtn.gameObject.SetActive(false);
                collectBtn.gameObject.SetActive(false);
                deliverBtn.gameObject.SetActive(true);
                break;
            case UIBtnType.INTERACT:
                cookBtn.gameObject.SetActive(false);
                collectBtn.gameObject.SetActive(false);
                deliverBtn.gameObject.SetActive(false);
                interactBtn.gameObject.SetActive(true);
                break;
            default:
                cookBtn.gameObject.SetActive(false);
                collectBtn.gameObject.SetActive(false);
                deliverBtn.gameObject.SetActive(false);
                interactBtn.gameObject.SetActive(false);
                break;
        }
    }
    public void HideBtn(UIBtnType uiBtnType)
    {
        switch(uiBtnType){
            case UIBtnType.COOK:
                cookBtn.gameObject.SetActive(false);
                break;
            case UIBtnType.COLLECT:
                collectBtn.gameObject.SetActive(false);
                break;
            case UIBtnType.DELIVER:
                deliverBtn.gameObject.SetActive(false);
                break;
            case UIBtnType.INTERACT:
                interactBtn.gameObject.SetActive(false);
                break;
            default:
                cookBtn.gameObject.SetActive(false);
                collectBtn.gameObject.SetActive(false);
                deliverBtn.gameObject.SetActive(false);
                break;
        }
    }
}
