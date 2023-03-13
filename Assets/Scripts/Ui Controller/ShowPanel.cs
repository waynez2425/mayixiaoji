using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPanel : MonoBehaviour
{
    private Animator thisanimator;//自己的动画
    private GameObject InterShowPanelButton;
    private GameObject QuitShowPanelButton;
    // Start is called before the first frame update
    void Start()
    {
        thisanimator = this.GetComponent<Animator>();
        //InterShowPanelButton = GameObject.Find("Canvas/Ui Button Controller/Below Panel/Get Food/Button");
        //QuitShowPanelButton = GameObject.Find("Canvas/Get Food Panel/Panel/Button");
    }

    // Update is called once per frame
    void Update()
    {
        //InterShowPanelButton.GetComponent<Button>().onClick.AddListener(InterShowPanel);
        //QuitShowPanelButton.GetComponent<Button>().onClick.AddListener(QuitShowPanel);
    }
    public void InterShowPanel()
    {
        PlayerPrefs.SetInt("DontRotationCamera",1 );
        thisanimator.SetBool("InterPanel", true);
        //PlayerPrefs.DeleteKey("DontRotationCamera");
        
    }
    public void QuitShowPanel()
    {
        PlayerPrefs.DeleteKey("DontRotationCamera");
        thisanimator.SetBool("InterPanel", false);
    }
}
