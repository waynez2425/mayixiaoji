using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerController : MonoBehaviour
{
    public PlayerInput pi;               //��ȡ��Ұ����������
    public float horizontalSpeed = 100.0f;//�����ˮƽ�ƶ��ٶ�
    public float verticalSpeed = 80.0f;//�������ֱ�ƶ��ٶ�

    public GameObject playerHandle;      //��ȡ��ҿ�����  �������������ˮƽ�ƶ�
    public GameObject cameraHandle;      //��ȡ���������  ������������������ƶ�
    public float tempEulerX;             //�޶�����������ƶ��������Сֵ


    private Vector3 startFingerPos;      //��ָ�����ĵ�һ��
    private Vector3 nowFingerPos;        //������ָ����λ�õĵ�
    private float xMoveDistance;         //�ƶ���x���ƶ��ľ���
    private float yMoveDistance;
    private int backValue = 0;
    public Joystick joystick;
    // Start is called before the first frame update
    void Awake()
    {
        cameraHandle = transform.parent.gameObject;          //��ȡ��������
        //playerHandle = cameraHandle.transform.parent.gameObject;

        //Cursor.lockState = CursorLockMode.Locked;           //�����������Ļ���룬���Ҳ���ʾ
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(joystick.Horizontal);
        //**************************************************************����ӽ��ƶ�***********************************************************

#if UNITY_EDITOR
        if (Input.GetMouseButton(0) && Input.GetAxis("Mouse X") != 0)
        {
            cameraHandle.transform.RotateAround(playerHandle.transform.position, new Vector3(0,1,0)/*transform.up*/,
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

        //û�д���
        //if (Input.touchCount <= 0)
        //{
        //    return;
        //}
        //if (joystick.Horizontal == 0 && joystick.Vertical == 0)
        //{
        //    RotateCamera(0);
        //}
        //else
        //{
        //    RotateCamera(1);
        //}
        //Jup = backValue;
        ////������������ƶ�
        //Jright = backValue;
        ////�����������ƶ�
        //playerHandle.transform.Rotate(Vector3.up, pi.Jright * horizontalSpeed * Time.deltaTime);
        //����playerHandle����תֵ�������������ˮƽ�ƶ�

        //��������������ƶ�
        //tempEulerX -= pi.Jup * verticalSpeed * Time.deltaTime;//��ȡ��������ƶ�ֵ
        //tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);//�����ƶ��Ƕ�
        //cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);//��ֵ����������ĸ���
    }
    public void RotateCamera(int i)
    {
        if (PlayerPrefs.HasKey("DontRotationCamera"))
        {

        }
        else
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began /*&&pi.Dvec!=Vector3.zero*/)
            {
                //Debug.Log("======��ʼ����=====");

                startFingerPos = Input.GetTouch(i).position;//��ȡ��һ����ָ��λ��
            }

            nowFingerPos = Input.GetTouch(i).position;
            //��ָ���ڴ�����Ļ����δ�ƶ�||	����Ļ��̧������ָ��
            if ((Input.GetTouch(i).phase == TouchPhase.Stationary) || (Input.GetTouch(i).phase == TouchPhase.Ended))
            {
                startFingerPos = nowFingerPos;
                //Debug.Log("======�ͷŴ���=====");
                return;
            }

            //			if (Input.GetTouch(0).phase == TouchPhase.Ended) {
            //				
            //			}
            if (startFingerPos == nowFingerPos)
            {
                return;
            }

            xMoveDistance = Mathf.Abs(nowFingerPos.x - startFingerPos.x);

            yMoveDistance = Mathf.Abs(nowFingerPos.y - startFingerPos.y);

            if (xMoveDistance > yMoveDistance)//�������ƶ���������������ƶ�����
            {
                if (nowFingerPos.x - startFingerPos.x > 0)//��ָ��������
                {
                    //Debug.Log("=======����X�Ḻ�����ƶ�=====");

                    backValue = -1; //����X�Ḻ�����ƶ�

                }
                else
                {
                    //Debug.Log("=======����X���������ƶ�=====");

                    backValue = 1; //����X���������ƶ�
                }
            }
            else
            {
                if (nowFingerPos.y - startFingerPos.y > 0)//��ָ��������
                {
                    //Debug.Log("=======����Y���������ƶ�=====");

                    backValue = 2; //����Y���������ƶ�
                }
                else
                {
                    //Debug.Log("=======����Y�Ḻ�����ƶ�=====");

                    backValue = -2; //����Y�Ḻ�����ƶ�
                }
            }
            if (backValue == -1)
            {
                //Debug.Log(1111111111);
                cameraHandle.transform.RotateAround(/*cameraHandle.transform.parent.GetChild(1)*/playerHandle.transform.position, new Vector3(0, 1, 0)/*transform.up*/,
                    -100 * Time.deltaTime);
            }
            else if (backValue == 1)
            {
                cameraHandle.transform.RotateAround(/*cameraHandle.transform.parent.GetChild(1)*/playerHandle.transform.position, new Vector3(0, 1, 0)/*transform.up*/,
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
