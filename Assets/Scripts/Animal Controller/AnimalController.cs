using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class AnimalController : MonoBehaviour
{
    private bool isidel,isrunorwalk,isdance,isrun,isrotation;//是否在状态
    private float changetime;//每次变化状态的间隔时间
    private float looktime;//观察时间
    //private float eggtime;
    private float statetype;//定义状态
    private float rotationx, rotationz;//旋转值
    private float eggx, eggz;//鸡蛋的xz值
    private int eggnumber;//鸡蛋的数量值

    //private float isrun;//判断是否要跑步
    private Animator xiaojianimator;//获取模型身上的动画组件
    private NavMeshAgent xiaojiNavMeshAgent;//获取模型身上的自动寻路组件
    private Rigidbody rigid;//获取模型身上的刚体组件
    private GameObject target;//目标点
    private GameObject Player;//玩家控制器
    private GameObject FeedButton;//喂食按钮
    private GameObject FeedButtonText;//喂食按钮的文本
    private Text text_feed_food;//喂食粮食数量
    private Transform targetposition;//目标的位置
    public bool IsLayeggs=false;//判断是否生蛋
    public GameObject egg;//获取蛋的预制体
    public GameObject FeedParticleSystem; //喂食的粒子效果
    public int foods;   //食物总量
    private SignIn signIn; //获取代码
    private int SignInfoods;
    private bool getfood;

    // Start is called before the first frame update
    private void Awake()
    {
        //PlayerPrefs.DeleteKey("signNum");
        //PlayerPrefs.DeleteKey("signData");
        //PlayerPrefs.DeleteKey("foodnumber");
        //PlayerPrefs.DeleteKey("foodnum");
        //获取组件
        xiaojianimator = this.transform.GetChild(1).gameObject.GetComponent<Animator>();
        //xiaojianimator = this.transform.Find("xiaoji").GetComponent<Animator>();
        xiaojiNavMeshAgent = this.GetComponent<NavMeshAgent>();
        rigid = this.GetComponent<Rigidbody>();
        target = GameObject.Find("Animal Controller/target");
        Player = GameObject.Find("Player Controller");
        FeedButton = GameObject.Find("Canvas/Ui Button Controller/Feed Panel/Button");
        FeedButtonText = GameObject.Find("Canvas/Ui Button Controller/Feed Panel/Button/Text (TMP)");
        signIn = GameObject.Find("Canvas/Get Food Panel/Scroll View Panel/Scroll View/Viewport/Content/Sign In Panel").GetComponent<SignIn>();
        text_feed_food=FeedButtonText.GetComponent<Text>();//获取粮食文本
        FeedButton.GetComponent<Button>().onClick.AddListener(Feed);//监听喂食按钮

        
    }
    void Start()
    {
        PlayerPrefs.DeleteKey("targetposition");
        isidel = true;
        getfood = true;
        foods = GetFoodNum();
        text_feed_food.text = "粮" + foods + "g";//显示粮食数量'
        PlayerPrefs.SetInt("foodnum", foods);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        foods = GetFoodNum();//实时更新粮食数量
        


        changetime += Time.deltaTime;//状态改变自增


        rigid.velocity = Vector3.zero;//限制碰撞

        //每隔10秒后切换一次状态
        if (changetime > 10f)
        {
            isidel = false;
           
        }
        else
        {
            isidel = true;
        }
        if (isidel==false)
        {
            //通过随机数切换状态
            statetype = Random.Range(0,3);
            
            
            //切换成吃动画
            if (statetype == 0)
            {
                //改变状态
                isidel = true;
                isrotation = false;
                changetime = 0;
                xiaojianimator.SetBool("IsWalk", false);
                xiaojianimator.SetBool("IsEat", true);
                
            }

            
            //切换成走路模式
            if (statetype == 1 )
            {
                isidel = true;
                changetime = 0;

                isrunorwalk = true;
                isrotation = false;
                targetposition = target.gameObject.transform;//获取目标位置
                                                             //Debug.Log(targetposition.position);
                PlayerPrefs.SetFloat("targetposition", targetposition.position.z);//存储目标位置的z轴
                xiaojiNavMeshAgent.SetDestination(target.gameObject.transform.position);//移动到目标位置

                //设置动画
                xiaojianimator.SetBool("IsWalk", true);
                xiaojianimator.SetBool("IsEat", false);

                
            }
            //切换为旋转状态
            if (statetype == 2)
            {

                isrotation = true;
                isidel = true;
                isrunorwalk = false;
                changetime = 0;

                //获取旋转值
                rotationx = Random.Range(-1f, 1f);
                rotationz = Random.Range(-1f, 1f);

                xiaojianimator.SetBool("IsWalk", false);
                xiaojianimator.SetBool("IsEat", false);
            }
           
        }
        
        //判断是否关闭走路动画
        if (targetposition!=null)
        {
            
            if (xiaojiNavMeshAgent.remainingDistance< xiaojiNavMeshAgent.stoppingDistance)
            {
                
                xiaojianimator.SetBool("IsWalk", false);
                
            }
        }
        //判断是否要进行旋转
        if (isrotation)
        {
            this.transform.forward = Vector3.Slerp(this.transform.forward, new Vector3(rotationx, 0, rotationz), 0.03f); //控制模型的移动方向；成渐变般缓慢增长(模型方向，目标值，增长数)
        }

        //生蛋
        if (xiaojianimator.gameObject.name == "xiaoji")
        {
            if (IsLayeggs)
            {
                
                xiaojianimator.SetBool("IsLayeggs", true);
                Invoke("Layeggs", 2f);//生蛋方法
                IsLayeggs = false;
                
                Invoke("StopLayeggs", 2f);
                changetime = 0;
                PlayerPrefs.SetInt("layegg",1);
                if (PlayerPrefs.HasKey("eggnumber"))
                {
                    eggnumber= PlayerPrefs.GetInt("eggnumber");
                    eggnumber++;
                }
                else
                {
                    eggnumber = 0;
                    eggnumber++;
                }
                
                PlayerPrefs.SetInt("eggnumber", eggnumber);
                
            }
            else
            {
                
            }
        }
        
    }
    //触发器判断是否有玩家在视野范围内
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            //判断盯着的时间
            looktime += Time.deltaTime;
            if (looktime <= 20f)
            {
                this.transform.LookAt(Player.transform.position);
                xiaojianimator.SetBool("IsEat", false);
                changetime = 0;
            }
            else
            {
                this.transform.LookAt(-Player.transform.position);
            }
            isrotation = false;
            isrunorwalk = false;
            isdance = false;
            //Debug.Log(Player);
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            looktime = 0;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
       
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
           
            Invoke("Hitwall", 2f);
            
        }
    }
    //撞墙转弯
    private void Hitwall()
    {
       
        this.transform.forward = new Vector3(0, 0, -1);
    }
    //生蛋
    private void Layeggs()
    {
        //RaycastHit hit;
        eggx = Random.Range(this.transform.position.x - 0.5f, this.transform.position.x + 0.5f);
        eggz = Random.Range(this.transform.position.z - 0.5f, this.transform.position.z + 0.5f);
        //判断生成鸡蛋的位置不在栅栏内
        while (eggx > 5.3 || eggx < -0.6 )
        {
            eggx = Random.Range(this.transform.position.x - 0.5f, this.transform.position.x + 0.5f);
        }
        while (eggz > 11.8 || eggz < 5 )
        {
            eggz = Random.Range(this.transform.position.z - 0.5f, this.transform.position.z + 0.5f);
        }
        

        
        Instantiate(egg, new Vector3(eggx, -0.045f, eggz), egg.transform.rotation);
       
        
    }
    //停止生蛋
    private void StopLayeggs()
    {
        xiaojianimator.SetBool("IsLayeggs", false);
    }
    //喂食
    private void Feed()
    {
        if (foods >= 180)
        {
            Instantiate(FeedParticleSystem, new Vector3(this.transform.position.x,0.5f,this.transform.position.z) ,
                FeedParticleSystem.transform.rotation);
            
            xiaojianimator.SetBool("IsEat", true);
            Invoke("StopEat", 2f);
            foods=GetFoodNum();//获取粮食数量
            foods -= 180;
            PlayerPrefs.SetInt("foodnum", foods);//将喂食后的粮食值储存起来
            text_feed_food.text = "粮" + foods + "g";//显示粮食数量
        }
    }
    //停止吃食动画
    private void StopEat()
    {
        xiaojianimator.SetBool("IsEat", false);
    }


    //获取还剩余的饲料量
    public int GetFoodNum()
    {
        if (PlayerPrefs.HasKey("foodnum"))
            return PlayerPrefs.GetInt("foodnum");
        return 300;
    }
}
