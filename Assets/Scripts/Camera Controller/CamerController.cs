using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerController : MonoBehaviour
{
    public PlayerInput pi;               //获取玩家按键输入组件
    public float horizontalSpeed = 100.0f;//摄像机水平移动速度
    public float verticalSpeed = 80.0f;//摄像机垂直移动速度

    public GameObject playerHandle;      //获取玩家控制器  来控制摄像机的水平移动
    public GameObject cameraHandle;      //获取相机控制器  来控制摄像机的纵向移动
    public float tempEulerX;             //限定摄像机纵向移动的最大最小值


    private Vector3 startFingerPos;      //手指触摸的第一点
    private Vector3 nowFingerPos;        //现在手指所在位置的点
    private float xMoveDistance;         //移动的x轴移动的距离
    private float yMoveDistance;
    private int backValue = 0;
    public Joystick joystick;
    // Start is called before the first frame update
    void Awake()
    {
        cameraHandle = transform.parent.gameObject;          //获取父级物体
        //playerHandle = cameraHandle.transform.parent.gameObject;

        //Cursor.lockState = CursorLockMode.Locked;           //将鼠标锁死屏幕中央，并且不显示
    }

    // Update is called once per frame
    void Update()
    {
        //**************************************************************鼠标视角移动***********************************************************

#if UNITY_EDITOR
        if (Input.GetMouseButton(0) && Input.GetAxis("Mouse X") != 0)
        {
            cameraHandle.transform.RotateAround(playerHandle.transform.position, new Vector3(0,1,0),
                 -Input.GetAxis("Mouse X") * 100f * Time.deltaTime);
            //Debug.Log(1111111111);
        }
        else
        {
            if (Input.touchCount <= 0)
            {
                return;
            }
            if (joystick.Horizontal == 0 && joystick.Vertical == 0)
            {
                RotateCamera(0);
            }
            else
            {
                RotateCamera(1);
            }
        }



#elif UNITY_ANDROID
             if (Input.touchCount <= 0)
        {
            return;
        }
        if (joystick.Horizontal == 0 && joystick.Vertical == 0)
        {
            RotateCamera(0);
        }
        else
        {
            RotateCamera(1);
        }
#endif

        
    }
    public void RotateCamera(int i)
    {
        if (PlayerPrefs.HasKey("DontRotationCamera"))
        {
            return;
        }
        else
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began /*&&pi.Dvec!=Vector3.zero*/)
            {
                

                startFingerPos = Input.GetTouch(i).position;//获取第一根手指的位置
            }

            nowFingerPos = Input.GetTouch(i).position;
            //手指正在触摸屏幕但尚未移动||	从屏幕上抬起了手指。
            if ((Input.GetTouch(i).phase == TouchPhase.Stationary) || (Input.GetTouch(i).phase == TouchPhase.Ended))
            {
                startFingerPos = nowFingerPos;
                
                return;
            }

            
            if (startFingerPos == nowFingerPos)
            {
                return;
            }

            xMoveDistance = Mathf.Abs(nowFingerPos.x - startFingerPos.x);

            yMoveDistance = Mathf.Abs(nowFingerPos.y - startFingerPos.y);

            if (xMoveDistance > yMoveDistance)//向左右移动距离大于向上下移动距离
            {
                if (nowFingerPos.x - startFingerPos.x > 0)//手指从左往右
                {
                    //Debug.Log("=======沿着X轴负方向移动=====");

                    backValue = -1; //沿着X轴负方向移动

                }
                else
                {
                    //Debug.Log("=======沿着X轴正方向移动=====");

                    backValue = 1; //沿着X轴正方向移动
                }
            }
            else
            {
                if (nowFingerPos.y - startFingerPos.y > 0)//手指从下往上
                {
                    //Debug.Log("=======沿着Y轴正方向移动=====");

                    backValue = 2; //沿着Y轴正方向移动
                }
                else
                {
                    //Debug.Log("=======沿着Y轴负方向移动=====");

                    backValue = -2; //沿着Y轴负方向移动
                }
            }
            if (backValue == -1)
            {
                //Debug.Log(1111111111);
                cameraHandle.transform.RotateAround(playerHandle.transform.position, new Vector3(0, 1, 0),
                    -100 * Time.deltaTime);
            }
            else if (backValue == 1)
            {
                cameraHandle.transform.RotateAround(playerHandle.transform.position, new Vector3(0, 1, 0),
                    100 * Time.deltaTime);
            }
            else if (backValue == 2)
            {
            }
            else if (backValue == -2)
            {
            }
        }
    }
    
}
