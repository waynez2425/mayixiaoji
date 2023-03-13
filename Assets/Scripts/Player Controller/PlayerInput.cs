using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Joystick joystick;
    public float Jup;
    public float Jright;
    public float UorD;//�ж��������ǰ�����Ǻ���
    public float RorL;//�ж�����������һ�������
    public float Dup;//ǰ����ֵ�Ļ�������ֵ��ʹ�ôӾ�̬����̬��ת�������Եù�����ͻ��
    public float Dright;//�����ƶ�����ֵ�Ļ�������ֵ��ʹ�ôӾ�̬����̬��ת�������Եù�����ͻ��
    public Vector3 Dvec;//��ɫ�ƶ��ķ���ֵ
    public float VeloctiyUp;
    public float VeloctiyRight;
    public float ForR;//�ж�����ķ�������ǰ�������ң�����б����


    public PlayerActorConroller pac;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        
        //****************************************************************��ɫ�ƶ�***********************************************************
        UorD = joystick.Vertical;
        //����Ŀ�����ж��ǰ���ǰ�����Ǻ��ˣ�������Wʱ��UorD��ֵΪ1������Ϊ0
        RorL = joystick.Horizontal;
        //����Ŀ�����ж��ǰ������һ������󣬵�����Dʱ��RorL��ֵΪ1������Ϊ0
        Dup = Mathf.SmoothDamp(Dup, UorD, ref VeloctiyUp, 0.1f);
        //��SmoothDamp�����ǵ���ֵ�仯�����0ֱ�ӵ�1�Եö����л�����ͻȻ
        //SmoothDamp(��ʼֵ��Ŀ��ֵ���仯���ʣ�Ĭ����0�Ϳ��ԣ����仯ʱ��)
        Dright = Mathf.SmoothDamp(Dright, RorL, ref VeloctiyRight, 0.1f);
        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));//���ô�ƽ�������б��ֵתΪ����б��ֵ��ת������

        
        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;
        //float Dright2 = joystick.Horizontal;
        //float Dup2 = joystick.Vertical;

        ForR = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
        //������ѧ˼�룬���������ϣ�y������ƽ������x�������ƽ���ٿ����ŵ���б��������ForR���Ǵ洢б�����ֵ
        //****************************************************************��ɫ�ܲ�***********************************************************
        //run = Input.GetKey(keyA);
        ////****************************************************************��ɫ����***********************************************************
        //roll = Input.GetKeyDown(keyB);

        //****************************************************************��ɫ��ת***********************************************************
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;//����������൱ǰģ�͵ķ�������   ���ҵ������������ϵ���������б�����ϵ�����
        //Debug.Log(Dvec);
        //Debug.Log(joystick.Vertical);
       
    }
    //��ɫ�ƶ��еķ���
    private Vector2 SquareToCircle(Vector2 input)//����һ��������ʹƽ���ϵ�xyֵ��������ϵ�xyֵ��ʹ��ɫб���ϵ��ٶ�����������
    {
        Vector2 output = Vector2.zero; //����һ����άֵ��x��y�����ȸ�Ϊ��0��0��

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);//��ת�����xֵ�����¶���Ķ�άֵ��x                 ת����ʽ���ɵ�x*��ƽ����1������֮yƽ��
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }

}
