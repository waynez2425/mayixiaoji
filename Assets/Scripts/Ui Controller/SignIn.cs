
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
//using DG.Tweening;
public class SignIn : MonoBehaviour
{
    /// <summary>
    /// 获取签到次数
    /// </summary>
    /// <returns>The sign number.</returns>
    public int GetSignNum()
    {
        if (PlayerPrefs.HasKey("signNum"))
            return PlayerPrefs.GetInt("signNum");
        return 1;
    }
    /// <summary>
    /// 设置签到次数
    /// </summary>
    /// <param name="num">Number.</param>
    public void SetSignNum(int num)
    {
        PlayerPrefs.SetInt("signNum", num);
    }
    /// <summary>
    /// 获取上次签到日期
    /// </summary>
    /// <returns>The sign data.</returns>
    public string GetSignData()
    {
        if (PlayerPrefs.HasKey("signData"))
            return PlayerPrefs.GetString("signData");
        return DateTime.MinValue.ToString();
    }
    /// <summary>
    /// 设置上次签到日期
    /// </summary>
    public void SetSignData(DateTime data)
    {
        PlayerPrefs.SetString("signData", data.ToString());
    }


    int signNum;//签到次数
    DateTime today;//今日日期
    DateTime signData;//上次签到日期
    public Button signbutton;//签到按钮
   
    public Text text_GetFood;//每日签到可得饲料的显示文本
    private Text text_feed_food;//喂食粮食数量
    private int foods;

    private void Awake()
    {
        text_feed_food = GameObject.Find("Canvas/Ui Button Controller/Feed Panel/Button/Text (TMP)").GetComponent<Text>();
    }
    private void Start()
    {
        
        today = DateTime.Now;
        signNum = GetSignNum();
        signData = DateTime.Parse(GetSignData());//将上次获取的时间转为DateTime的形式
       


        
        if (IsOneDay(signData, today))
        {
            //当天数没有变化，是当天的时候显示饲料量
            text_GetFood.text = (signNum-1) * 5 + "g";
            signbutton.interactable = false;
            //text_GetFood.text = signNum * 5 + "g";
            return;
        }

        
        //  Debug.Log(string.Format("lastSign==={0},today===={1}", signData, today));
        //新的签到周期，需要清除签到存档(清楚签到次数和上一次签到日期)
        if (NeedClean())
        {
            PlayerPrefs.DeleteKey("signNum");
            PlayerPrefs.DeleteKey("signData");
            //PlayerPrefs.DeleteKey("foodnumber");
        }
        signNum = GetSignNum();
        //在天数变化后重新生成饲料量
        text_GetFood.text = signNum * 5 + "g";


        //OnBtnGetRewordClick();
        //如果不是同一天激活按钮
        signbutton.interactable = true;
    }
    //签到按钮点击
    public void OnBtnGetRewordClick()
    {
        
        signNum++;//签到次数增加
        signData = today;//签到的时间为现在的时间
        //更新存档
        SetSignData(signData);
        SetSignNum(signNum);

        
        //喂食按钮下的text文本值更改
        foods = PlayerPrefs.GetInt("foodnum");
        foods += (GetSignNum() - 1) * 5;
        PlayerPrefs.SetInt("foodnum",foods);
        text_feed_food.text = "粮" + foods + "g";
        //按钮触发完后关闭按钮
        signbutton.interactable = false;
    }
    //判断是否是同一天
    bool IsOneDay(DateTime t1, DateTime t2)
    {
        return (t1.Year == t2.Year &&
         t1.Month == t2.Month &&
          t1.Day == t2.Day);
    }
    //需要清除数据(当签到天数大于等于7天或者签到间隔大约一天，则重置数据)
    bool NeedClean()
    {
        //转换为TimeSpan新实例化的指数
        TimeSpan tsNow = new TimeSpan(today.Ticks);
        TimeSpan tsSign = new TimeSpan(signData.Ticks);
        TimeSpan tsDur = tsNow.Subtract(tsSign).Duration();//现在的时间减去过去的时间
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

