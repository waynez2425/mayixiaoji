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
    private bool isGetEggPanel;



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
            //model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);//����ģ�͵��ƶ����򣻳ɽ���㻺������(ģ�ͷ���Ŀ��ֵ��������)
        }
        else
        {
            animator.SetFloat("Forward", 0);
        }
        if (animator.GetFloat("Forward") >= 1.5f)
        {
            walkSpeed = 0.05f;
        }
        else
        {
            walkSpeed = 0.02f;
        }
        movingVec = pi.ForR * model.transform.forward * walkSpeed * /*((pi.run) ? runMultiplier :*/ 1.0f;//�����ٶ�Ϊ�ٶȷ�������*ģ��ǰ��*�ٶ�*�ж��Ƿ�ʼ�ܲ�
        //Debug.Log("���﷽��"+model.transform.forward);
        //Debug.Log("����ͷ"+text11.transform.forward);
        //if (animator.GetFloat("Forward") != 0)
        //{
        //    animator.SetBool("IsDance", false);
        //}
        if (Input.GetMouseButtonDown(0) && /*PlayerPrefs.HasKey("layegg")*/PlayerPrefs.GetInt("eggnumber")>0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Plane")
                {
                    //Debug.Log(22222);
                    //Debug.DrawLine(ray.origin, hit.point, Color.black,10f);
                    //Debug.Log(hit.point);
                    //agent.SetDestination(hit.point);

                    //animator.SetBool("IsRun",true);
                    //Invoke("StopRun", 1.5f);
                    isNextMove = true;
                    point = hit.point;
                }
                if (hit.collider.gameObject.tag == "Egg")
                {
                    //Debug.Log(111);
                    //Debug.Log(hit.point);
                    isNextMove = true;
                    point = hit.collider.gameObject.transform.position;
                    //Debug.Log(point);
                }
            }
        }
        //if (GetEggPanel.activeSelf)
        //{
        //    isGetEggPanel = true;
        //}
        //else
        //{
        //    isGetEggPanel = false;
        //}
        if (isNextMove /*&& isGetEggPanel == false*/)
        {
            Move(point);
        }
        //if (isGetEggPanel)
        //{
        //    animator.SetBool("IsRun", false);
        //}
    }
    void FixedUpdate()//ÿ��ģ��50��   Time.fixedDeltaTime
    {
        //**************************************************************��ɫλ��****************************************************************************
        agent.Move(new Vector3(movingVec.x, rigid.velocity.y, movingVec.z));
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;//������ײ

        //������ƶ�
        
       
        //Debug.Log(movingVec);
        //rigid.position += movingVec * Time.fixedDeltaTime;  �����λ��+=�ƶ��ٶ�*ʱ��  ��������һ���ƶ�����
        //rigid.velocity = new Vector3(movingVec.x, rigid.velocity.y, movingVec.z);//������ٶ�=�¶�����ٶȵ�x��zֵ���Լ����屾���yֵ����Ϊ������ٶ�ֵ��yֵ��û�ж����
    }
    public void Move(Vector3 pos)
    {

        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * 2f);
        //agent.SetDestination(pos);//�ƶ���Ŀ��λ��

        Debug.Log(pos);
        //model.transform.forward = pos - model.transform.position;
        //model.transform.forward = Quaternion.Euler(pos.x, pos.y, pos.z);
        model.transform.LookAt(pos);
        animator.SetBool("IsRun", true);
        if (transform.position == pos)
        {
            isNextMove = false;
            animator.SetBool("IsRun", false);
        }
    }
}

