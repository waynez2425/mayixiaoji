using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Joystick joystick;
    public float Jup;
    public float Jright;
    public float UorD;//判断输入的是前进还是后退
    public float RorL;//判断输入的是向右还是向左
    public float Dup;//前进数值的缓慢增长值，使得从静态到动态的转换不会显得过于唐突。
    public float Dright;//向右移动的数值的缓慢增长值，使得从静态到动态的转换不会显得过于唐突。
    public Vector3 Dvec;//角色移动的方向值
    public float VeloctiyUp;
    public float VeloctiyRight;
    public float ForR;//判断输入的方向是向前还是向右，还是斜向方向


    public PlayerActorConroller pac;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        
        //****************************************************************角色移动***********************************************************
        UorD = joystick.Vertical;
        //用三目运算判断是按下前进还是后退，当按下W时，UorD的值为1，否则为0
        RorL = joystick.Horizontal;
        //用三目运算判断是按下向右还是向左，当按下D时，RorL的值为1，否则为0
        Dup = Mathf.SmoothDamp(Dup, UorD, ref VeloctiyUp, 0.1f);
        //用SmoothDamp函数是的数值变化不会从0直接到1显得动作切换过于突然
        //SmoothDamp(起始值，目标值，变化速率（默认是0就可以），变化时间)
        Dright = Mathf.SmoothDamp(Dright, RorL, ref VeloctiyRight, 0.1f);
        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));//调用从平面坐标的斜向值转为球体斜向值的转换法方

        
        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;
        //float Dright2 = joystick.Horizontal;
        //float Dup2 = joystick.Vertical;

        ForR = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
        //利用数学思想，在坐标轴上，y轴坐标平方加上x轴坐标的平方再开根号等于斜向方向，所以ForR就是存储斜向方向的值
        //****************************************************************角色跑步***********************************************************
        //run = Input.GetKey(keyA);
        ////****************************************************************角色翻滚***********************************************************
        //roll = Input.GetKeyDown(keyB);

        //****************************************************************角色旋转***********************************************************
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;//方向存量乘余当前模型的方向向量   向右的向量加上向上的向量等于斜向右上的向量
        //Debug.Log(Dvec);
        //Debug.Log(joystick.Vertical);
       
    }
    //角色移动中的方法
    private Vector2 SquareToCircle(Vector2 input)//定义一个方法，使平面上的xy值变成球体上的xy值，使角色斜面上的速度增量不会变大
    {
        Vector2 output = Vector2.zero; //定义一个二维值，x，y坐标先付为（0，0）

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);//将转换完的x值付给新定义的二维值的x                 转换公式：旧的x*开平方的1减二分之y平方
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }

}
