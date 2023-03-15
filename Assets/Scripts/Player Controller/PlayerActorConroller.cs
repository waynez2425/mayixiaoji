using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerActorConroller : MonoBehaviour
{
    public PlayerInput pi;//��ȡ������ϵ�PlayerInput���

    public Animator animator;//��ȡ������ϵĶ������
    public GameObject model;//��ȡ���ģ��
    public GameObject CameraHandle;
    //public GameObject RoomControllerPanel;//�ƹ�Ч��
    public NavMeshAgent agent;//������ϵ�Ѱ·���

    public GameObject GetEggPanel;//��ȡ���������

    private Vector3 movingVec;//�����ƶ��ٶ�
    public Rigidbody rigid;//������ϵĸ������

    public float walkSpeed = 0.02f;//��·�ٶȶ���
    public float runMultiplier = 2.0f;//�ܲ��ٶȶ���

    public Vector3 point;//�������λ��
    private bool isNextMove = false;//�Ƿ��ƶ�����һ����λ�Ŀ���




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(PlayerPrefs.GetInt("eggnumber"));
        
        ////Ϊ��ʹ��·�������ܲ�����֮����л�����ôͻأʹ��lerp����ȡ��·ֵ���ܲ�ֵֵ�İٷ�֮50��Ϊ�����л���ֵ������ֵ
        animator.SetFloat("Forward", pi.ForR * Mathf.Lerp(animator.GetFloat("Forward"), /*((pi.run) ? 2.0f :*/ 2.0f, 0.5f));
        //���ö����������ϵ���ֵ�����ƶ������������ƣ�Ŀ����ֵ��ֵ(������������ֵ����ǰֵA��Ŀ��ֵB��A-B֮������İٷ�֮��)��

        //**************************************************************��ɫ��ת*******************************************************************************
        if (pi.ForR > 0.1f)//���������������ֵʱ�Ž�����ת                                
        {
            var enguler = Quaternion.LookRotation(pi.Dvec).eulerAngles; //��ң�еĽǶ��ݴ�
            enguler.y += CameraHandle.transform.eulerAngles.y;          //������ĽǶ�yֵ
            model.transform.eulerAngles = enguler;                      //ң�еĽǶȸ�ģ�ͽǶ�
           
        }
        else
        {
            animator.SetFloat("Forward", 0);
        }
        //�����ܲ��ٶ�
        if (animator.GetFloat("Forward") >= 1.5f)
        {
            walkSpeed = 0.05f;
        }
        else
        {
            walkSpeed = 0.02f;
        }
        movingVec = pi.ForR * model.transform.forward * walkSpeed *  1.0f;//�����ٶ�Ϊ�ٶȷ�������*ģ��ǰ��*�ٶ�*�ж��Ƿ�ʼ�ܲ�
        
        if (Input.GetMouseButtonDown(0) && PlayerPrefs.GetInt("eggnumber")>0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Plane")
                {
                    
                    isNextMove = true;
                    point = hit.point;
                }
                if (hit.collider.gameObject.tag == "Egg")
                {
                    
                    isNextMove = true;
                    point = hit.collider.gameObject.transform.position;
                   
                }
            }
        }

        if (isNextMove )
        {
            Move(point);
        }

    }
    void FixedUpdate()//ÿ��ģ��50��   Time.fixedDeltaTime
    {
        //**************************************************************��ɫλ��****************************************************************************
        agent.Move(new Vector3(movingVec.x, rigid.velocity.y, movingVec.z));
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;//������ײ


    }
    //������ƶ�
    public void Move(Vector3 pos)
    {

        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * 2f);

        model.transform.LookAt(pos);
        animator.SetBool("IsRun", true);
        if (transform.position == pos)
        {
            isNextMove = false;
            animator.SetBool("IsRun", false);
        }
    }
}

