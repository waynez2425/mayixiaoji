using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Geteggs : MonoBehaviour
{
    private GameObject GetEggsButtonPanel;
    private GameObject GetEggsButton;
    private int eggnumber;
    // Start is called before the first frame update
    void Start()
    {
        //获取面板和按钮
        GetEggsButtonPanel = GameObject.Find("Canvas/Ui Button Controller/Get Eggs");
        GetEggsButton = GameObject.Find("Canvas/Ui Button Controller/Get Eggs/Button");
        

    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log(eggnumber);
    }

    //收获鸡蛋的面板
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            //按钮监听事件
            GetEggsButton.GetComponent<Button>().onClick.AddListener(Getegg);
            GetEggsButtonPanel.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GetEggsButtonPanel.SetActive(false);
        }
    }
    //收获鸡蛋方法
    private void Getegg()
    {
        //eggnumber = PlayerPrefs.GetInt("eggnumber");
        //Debug.Log(eggnumber);
        //if (eggnumber == 0)
        //{
        //    PlayerPrefs.DeleteKey("layegg");
        //}

        GetEggsButtonPanel.SetActive(false);
        Destroy(this.gameObject);
        //
    }
    //鸡蛋数量删除
    private void OnDestroy()
    {
        //Debug.Log(111);
        eggnumber = PlayerPrefs.GetInt("eggnumber");
        eggnumber--;
        PlayerPrefs.SetInt("eggnumber", eggnumber);
    }
}
