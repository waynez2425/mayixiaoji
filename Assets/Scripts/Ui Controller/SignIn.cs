
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
//using DG.Tweening;
public class SignIn : MonoBehaviour
{
    /// <summary>
    /// ��ȡǩ������
    /// </summary>
    /// <returns>The sign number.</returns>
    public int GetSignNum()
    {
        if (PlayerPrefs.HasKey("signNum"))
            return PlayerPrefs.GetInt("signNum");
        return 1;
    }
    /// <summary>
    /// ����ǩ������
    /// </summary>
    /// <param name="num">Number.</param>
    public void SetSignNum(int num)
    {
        PlayerPrefs.SetInt("signNum", num);
    }
    /// <summary>
    /// ��ȡ�ϴ�ǩ������
    /// </summary>
    /// <returns>The sign data.</returns>
    public string GetSignData()
    {
        if (PlayerPrefs.HasKey("signData"))
            return PlayerPrefs.GetString("signData");
        return DateTime.MinValue.ToString();
    }
    /// <summary>
    /// �����ϴ�ǩ������
    /// </summary>
    public void SetSignData(DateTime data)
    {
        PlayerPrefs.SetString("signData", data.ToString());
    }


    int signNum;//ǩ������
    DateTime today;//��������
    DateTime signData;//�ϴ�ǩ������
    public Button signbutton;//ǩ����ť
   
    public Text text_GetFood;//ÿ��ǩ���ɵ����ϵ���ʾ�ı�
    private Text text_feed_food;//ιʳ��ʳ����
    private int foods;

    private void Awake()
    {
        text_feed_food = GameObject.Find("Canvas/Ui Button Controller/Feed Panel/Button/Text (TMP)").GetComponent<Text>();
    }
    private void Start()
    {
        
        today = DateTime.Now;
        signNum = GetSignNum();
        signData = DateTime.Parse(GetSignData());//���ϴλ�ȡ��ʱ��תΪDateTime����ʽ
       


        
        if (IsOneDay(signData, today))
        {
            //������û�б仯���ǵ����ʱ����ʾ������
            text_GetFood.text = (signNum-1) * 5 + "g";
            signbutton.interactable = false;
            //text_GetFood.text = signNum * 5 + "g";
            return;
        }

        
        //  Debug.Log(string.Format("lastSign==={0},today===={1}", signData, today));
        //�µ�ǩ�����ڣ���Ҫ���ǩ���浵(���ǩ����������һ��ǩ������)
        if (NeedClean())
        {
            PlayerPrefs.DeleteKey("signNum");
            PlayerPrefs.DeleteKey("signData");
            //PlayerPrefs.DeleteKey("foodnumber");
        }
        signNum = GetSignNum();
        //�������仯����������������
        text_GetFood.text = signNum * 5 + "g";


        //OnBtnGetRewordClick();
        //�������ͬһ�켤�ť
        signbutton.interactable = true;
    }
    //ǩ����ť���
    public void OnBtnGetRewordClick()
    {
        
        signNum++;//ǩ����������
        signData = today;//ǩ����ʱ��Ϊ���ڵ�ʱ��
        //���´浵
        SetSignData(signData);
        SetSignNum(signNum);

        
        //ιʳ��ť�µ�text�ı�ֵ����
        foods = PlayerPrefs.GetInt("foodnum");
        foods += (GetSignNum() - 1) * 5;
        PlayerPrefs.SetInt("foodnum",foods);
        text_feed_food.text = "��" + foods + "g";
        //��ť�������رհ�ť
        signbutton.interactable = false;
    }
    //�ж��Ƿ���ͬһ��
    bool IsOneDay(DateTime t1, DateTime t2)
    {
        return (t1.Year == t2.Year &&
         t1.Month == t2.Month &&
          t1.Day == t2.Day);
    }
    //��Ҫ�������(��ǩ���������ڵ���7�����ǩ�������Լһ�죬����������)
    bool NeedClean()
    {
        //ת��ΪTimeSpan��ʵ������ָ��
        TimeSpan tsNow = new TimeSpan(today.Ticks);
        TimeSpan tsSign = new TimeSpan(signData.Ticks);
        TimeSpan tsDur = tsNow.Subtract(tsSign).Duration();//���ڵ�ʱ���ȥ��ȥ��ʱ��
        // Debug.Log(string.Format("days====={0},hours======{1},minutes====={2}", tsDur.Days, tsDur.Hours, tsDur.Minutes));
        signNum = GetSignNum();
        if (signNum > 7 || tsDur.Days > 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

