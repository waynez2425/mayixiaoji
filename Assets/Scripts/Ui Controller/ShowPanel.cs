using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPanel : MonoBehaviour
{
    private Animator thisanimator;//自己的动画

    // Start is called before the first frame update
    void Start()
    {
        thisanimator = this.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void InterShowPanel()
    {
        PlayerPrefs.DeleteKey("eggnumber");
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
