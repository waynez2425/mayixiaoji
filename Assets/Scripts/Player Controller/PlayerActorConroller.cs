using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PlayerActorConroller : MonoBehaviour
{
    public PlayerInput pi;//获取玩家身上的PlayerInput组件

    public Animator animator;//获取玩家身上的动画组件
    public GameObject model;//获取玩家模型
    public GameObject CameraHandle;
    //public GameObject RoomControllerPanel;//灯光效果
    public NavMeshAgent agent;//玩家身上的寻路组件

    public GameObject GetEggPanel;//获取鸡蛋的面板

    private Vector3 movingVec;//定义移动速度
    public Rigidbody rigid;//玩家身上的刚体组件

    public float walkSpeed = 0.02f;//走路速度定义
    public float runMultiplier = 2.0f;//跑步速度定义

    public Vector3 point;//鼠标点击的位置
    private bool isNextMove = false;//是否移动到下一个点位的开关




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(PlayerPrefs.GetInt("eggnumber"));
        
        ////为了使走路动画到跑步动画之间的切换不那么突兀使用lerp函数取走路值和跑步值值的百分之50作为动画切换数值的增长值
        animator.SetFloat("Forward", pi.ForR * Mathf.Lerp(animator.GetFloat("Forward"), /*((pi.run) ? 2.0f :*/ 2.0f, 0.5f));
        //设置动画控制器上的数值（控制动画的条件名称，目标数值数值(缓慢增长的数值（当前值A，目标值B，A-B之间差量的百分之）)）

        //**************************************************************角色旋转*******************************************************************************
        if (pi.ForR > 0.1f)//当方向键有在输入值时才进行旋转                                
        {
            var enguler = Quaternion.LookRotation(pi.Dvec).eulerAngles; //把遥感的角度暂存
            enguler.y += CameraHandle.transform.eulerAngles.y;          //摄像机的角度y值
            model.transform.eulerAngles = enguler;                      //遥感的角度给模型角度
           
        }
        else
        {
            animator.SetFloat("Forward", 0);
        }
        //设置跑步速度
        if (animator.GetFloat("Forward") >= 1.5f)
        {
            walkSpeed = 0.05f;
        }
        else
        {
            walkSpeed = 0.02f;
        }
        movingVec = pi.ForR * model.transform.forward * walkSpeed *  1.0f;//设置速度为速度方向向量*模型前方*速度*判断是否开始跑步
        
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
    void FixedUpdate()//每秒模拟50次   Time.fixedDeltaTime
    {
        //**************************************************************角色位移****************************************************************************
        agent.Move(new Vector3(movingVec.x, rigid.velocity.y, movingVec.z));
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;//限制碰撞


    }
    //鼠标点击移动
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

