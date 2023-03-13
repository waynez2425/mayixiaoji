using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Geteggs : MonoBehaviour
{
    private GameObject GetEggsButtonPanel;
    private GameObject GetEggsButton;
    // Start is called before the first frame update
    void Start()
    {
        //��ȡ���Ͱ�ť
        GetEggsButtonPanel = GameObject.Find("Canvas/Ui Button Controller/Get Eggs");
        GetEggsButton = GameObject.Find("Canvas/Ui Button Controller/Get Eggs/Button");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�ջ񼦵������
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            //��ť�����¼�
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
    //�ջ񼦵�����
    private void Getegg()
    {
        PlayerPrefs.DeleteKey("layegg");
        GetEggsButtonPanel.SetActive(false);
        DestroyImmediate(this.gameObject);
    }
}
