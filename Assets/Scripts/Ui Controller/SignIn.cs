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
   //private bool isRewardTake = false;
    public Text text_GetFood;
    //public int GetFoodNumber=0;
    //public int Number = 5;
    private void Start()
    {
        //Number = 5;
        today = DateTime.Now;
        signNum = GetSignNum();
        signData = DateTime.Parse(GetSignData());//���ϴλ�ȡ��ʱ��תΪDateTime����ʽ
        //if (PlayerPrefs.HasKey("foodnumber"))
        //{
        //    Number = PlayerPrefs.GetInt("foodnumber");
        //}
        //else
        //{
        //    PlayerPrefs.SetInt("foodnumber", 5);
        //}
        //Debug.Log(signNum);
        text_GetFood.text = signNum * 5 + "g";
        if (IsOneDay(signData, today))
        {
            signbutton.interactable = false;
            //text_GetFood.text = signNum * 5 + "g";
            return;
        }

        ////test
        //if(!IsOneDay(signData, today)&& signNum > 0&&NeedClean()==false)
        //{
        //    Number += 5;
        //    PlayerPrefs.SetInt("foodnumber", Number);
        //    //Debug.Log(11111);
        //}
        //else
        //{
        //    Number = PlayerPrefs.GetInt("foodnumber");
        //}
        //Number = PlayerPrefs.GetInt("foodnumber");
        //Debug.Log(PlayerPrefs.GetInt("foodnumber"));
        //text_GetFood.text = Number.ToString() + "g";
        //  Debug.Log(string.Format("lastSign==={0},today===={1}", signData, today));
        //�µ�ǩ�����ڣ���Ҫ���ǩ���浵(���ǩ����������һ��ǩ������)
        if (NeedClean())
        {
            PlayerPrefs.DeleteKey("signNum");
            PlayerPrefs.DeleteKey("signData");
            //PlayerPrefs.DeleteKey("foodnumber");
        }
        signNum = GetSignNum();
        


        //OnBtnGetRewordClick();
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


        //Debug.Log(PlayerPrefs.GetString("signData"));
        //���û�����ʳ
        //GetFoodNumber = FoodNumber(signNum);

        //���û��ӽ��
        //DataManager.instance.SetCoin(DataManager.instance.GetCoin() + signNum * 3);
        //text_Getcoin.transform.GetChild(0).GetComponent<Text>().text = "Get" + " " + signNum * 3 + " coins";
        //text_Getcoin.gameObject.SetActive(true);
        //text_Getcoin.transform.GetChild(0).transform.DOScale(1.2f, 2.5f).onComplete = delegate
        //{
        //    text_Getcoin.transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1);
        //    text_Getcoin.gameObject.SetActive(false);
        //PlayerPrefs.DeleteKey("foodnumber");
        //PlayerPrefs.DeleteKey("signNum");
        //PlayerPrefs.DeleteKey("signData");
        //};
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
        if (signNum >= 7 || tsDur.Days > 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

