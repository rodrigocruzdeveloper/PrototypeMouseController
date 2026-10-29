using System.Collections;
using UnityEngine;

public class PlayerMouseController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float timeAttack;

    [Header("Swipe Settings")]
    [SerializeField] private float minSwipeDistance = 50.0f;
    [SerializeField] private Vector2[] lanes;


    private int currentLane = 2;


    private Vector2 touchStart;
    private Vector2 touchEnd;
    private Vector2 swipe;

    private bool walking;
    private bool attacking;

    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        Move();
        AnimationManager();
        LaneDistance();

        if (Input.GetButtonDown("Fire1"))
        {
            touchStart = Input.mousePosition;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            touchEnd = Input.mousePosition;
            
            swipe = touchEnd - touchStart;
                      
            if(swipe.magnitude >= minSwipeDistance)
            {
                if(swipe.x > 0)
                {
                    SwipeRight();
                }
                else
                {
                    SwipeLeft();
                }
            }
            else
            {
                StartCoroutine(Attack(timeAttack));
            }
        }
    }


    void SwipeLeft()
    {
        transform.eulerAngles = new Vector2(0, 180);

        if(currentLane > 0)
        {
            currentLane--;
        }        
    }


    void SwipeRight()
    {
        transform.eulerAngles = new Vector2(0, 0);

        if (currentLane < lanes.Length - 1)
        {
            currentLane++;
        }        
    }


    void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, lanes[currentLane], speed * Time.deltaTime);
    }


    IEnumerator Attack(float t)
    {
        if(walking == false && attacking == false)
        {            
            attacking = true;
            yield return new WaitForSeconds(t);
            attacking = false;
        }
    }


    bool LaneDistance()
    {
        walking = Vector2.Distance(transform.position, lanes[currentLane]) > 0.01f;
        return walking;
    }


    void AnimationManager()
    {
        animator.SetBool("pWalk", LaneDistance());
        animator.SetBool("pUpAttack", attacking);
    }
}
