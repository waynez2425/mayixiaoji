using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class AnimalController : MonoBehaviour
{
    private bool isidel,isrunorwalk,isdance,isrun,isrotation;//�Ƿ���״̬
    private float changetime;//ÿ�α仯״̬�ļ��ʱ��
    private float looktime;//�۲�ʱ��
    //private float eggtime;
    private float statetype;//����״̬
    private float rotationx, rotationz;//��תֵ
    private float eggx, eggz;//������xzֵ
    private int eggnumber;//����������ֵ

    //private float isrun;//�ж��Ƿ�Ҫ�ܲ�
    private Animator xiaojianimator;//��ȡģ�����ϵĶ������
    private NavMeshAgent xiaojiNavMeshAgent;//��ȡģ�����ϵ��Զ�Ѱ·���
    private Rigidbody rigid;//��ȡģ�����ϵĸ������
    private GameObject target;//Ŀ���
    private GameObject Player;//��ҿ�����
    private GameObject FeedButton;//ιʳ��ť
    private GameObject FeedButtonText;//ιʳ��ť���ı�
    private Text text_feed_food;//ιʳ��ʳ����
    private Transform targetposition;//Ŀ���λ��
    public bool IsLayeggs=false;//�ж��Ƿ�����
    public GameObject egg;//��ȡ����Ԥ����
    public GameObject FeedParticleSystem; //ιʳ������Ч��
    public int foods;   //ʳ������
    private SignIn signIn; //��ȡ����
    private int SignInfoods;
    private bool getfood;

    // Start is called before the first frame update
    private void Awake()
    {
        //PlayerPrefs.DeleteKey("signNum");
        //PlayerPrefs.DeleteKey("signData");
        //PlayerPrefs.DeleteKey("foodnumber");
        //PlayerPrefs.DeleteKey("foodnum");
        //��ȡ���
        xiaojianimator = this.transform.GetChild(1).gameObject.GetComponent<Animator>();
        //xiaojianimator = this.transform.Find("xiaoji").GetComponent<Animator>();
        xiaojiNavMeshAgent = this.GetComponent<NavMeshAgent>();
        rigid = this.GetComponent<Rigidbody>();
        target = GameObject.Find("Animal Controller/target");
        Player = GameObject.Find("Player Controller");
        FeedButton = GameObject.Find("Canvas/Ui Button Controller/Feed Panel/Button");
        FeedButtonText = GameObject.Find("Canvas/Ui Button Controller/Feed Panel/Button/Text (TMP)");
        signIn = GameObject.Find("Canvas/Get Food Panel/Scroll View Panel/Scroll View/Viewport/Content/Sign In Panel").GetComponent<SignIn>();
        text_feed_food=FeedButtonText.GetComponent<Text>();//��ȡ��ʳ�ı�
        FeedButton.GetComponent<Button>().onClick.AddListener(Feed);//����ιʳ��ť

        
    }
    void Start()
    {
        PlayerPrefs.DeleteKey("targetposition");
        isidel = true;
        getfood = true;
        foods = GetFoodNum();
        text_feed_food.text = "��" + foods + "g";//��ʾ��ʳ����'
        PlayerPrefs.SetInt("foodnum", foods);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        foods = GetFoodNum();//ʵʱ������ʳ����
        


        changetime += Time.deltaTime;//״̬�ı�����


        rigid.velocity = Vector3.zero;//������ײ

        //ÿ��10����л�һ��״̬
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
            //ͨ��������л�״̬
            statetype = Random.Range(0,3);
            
            
            //�л��ɳԶ���
            if (statetype == 0)
            {
                //�ı�״̬
                isidel = true;
                isrotation = false;
                changetime = 0;
                xiaojianimator.SetBool("IsWalk", false);
                xiaojianimator.SetBool("IsEat", true);
                
            }

            
            //�л�����·ģʽ
            if (statetype == 1 )
            {
                isidel = true;
                changetime = 0;

                isrunorwalk = true;
                isrotation = false;
                targetposition = target.gameObject.transform;//��ȡĿ��λ��
                                                             //Debug.Log(targetposition.position);
                PlayerPrefs.SetFloat("targetposition", targetposition.position.z);//�洢Ŀ��λ�õ�z��
                xiaojiNavMeshAgent.SetDestination(target.gameObject.transform.position);//�ƶ���Ŀ��λ��

                //���ö���
                xiaojianimator.SetBool("IsWalk", true);
                xiaojianimator.SetBool("IsEat", false);

                
            }
            //�л�Ϊ��ת״̬
            if (statetype == 2)
            {

                isrotation = true;
                isidel = true;
                isrunorwalk = false;
                changetime = 0;

                //��ȡ��תֵ
                rotationx = Random.Range(-1f, 1f);
                rotationz = Random.Range(-1f, 1f);

                xiaojianimator.SetBool("IsWalk", false);
                xiaojianimator.SetBool("IsEat", false);
            }
           
        }
        
        //�ж��Ƿ�ر���·����
        if (targetposition!=null)
        {
            
            if (xiaojiNavMeshAgent.remainingDistance< xiaojiNavMeshAgent.stoppingDistance)
            {
                
                xiaojianimator.SetBool("IsWalk", false);
                
            }
        }
        //�ж��Ƿ�Ҫ������ת
        if (isrotation)
        {
            this.transform.forward = Vector3.Slerp(this.transform.forward, new Vector3(rotationx, 0, rotationz), 0.03f); //����ģ�͵��ƶ����򣻳ɽ���㻺������(ģ�ͷ���Ŀ��ֵ��������)
        }

        //����
        if (xiaojianimator.gameObject.name == "xiaoji")
        {
            if (IsLayeggs)
            {
                
                xiaojianimator.SetBool("IsLayeggs", true);
                Invoke("Layeggs", 2f);//��������
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
    //�������ж��Ƿ����������Ұ��Χ��
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            //�ж϶��ŵ�ʱ��
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
    //ײǽת��
    private void Hitwall()
    {
       
        this.transform.forward = new Vector3(0, 0, -1);
    }
    //����
    private void Layeggs()
    {
        //RaycastHit hit;
        eggx = Random.Range(this.transform.position.x - 0.5f, this.transform.position.x + 0.5f);
        eggz = Random.Range(this.transform.position.z - 0.5f, this.transform.position.z + 0.5f);
        //�ж����ɼ�����λ�ò���դ����
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
    //ֹͣ����
    private void StopLayeggs()
    {
        xiaojianimator.SetBool("IsLayeggs", false);
    }
    //ιʳ
    private void Feed()
    {
        if (foods >= 180)
        {
            Instantiate(FeedParticleSystem, new Vector3(this.transform.position.x,0.5f,this.transform.position.z) ,
                FeedParticleSystem.transform.rotation);
            
            xiaojianimator.SetBool("IsEat", true);
            Invoke("StopEat", 2f);
            foods=GetFoodNum();//��ȡ��ʳ����
            foods -= 180;
            PlayerPrefs.SetInt("foodnum", foods);//��ιʳ�����ʳֵ��������
            text_feed_food.text = "��" + foods + "g";//��ʾ��ʳ����
        }
    }
    //ֹͣ��ʳ����
    private void StopEat()
    {
        xiaojianimator.SetBool("IsEat", false);
    }


    //��ȡ��ʣ���������
    public int GetFoodNum()
    {
        if (PlayerPrefs.HasKey("foodnum"))
            return PlayerPrefs.GetInt("foodnum");
        return 300;
    }
}
