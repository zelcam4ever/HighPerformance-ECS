using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    public GameObject  gTarget;
    public float       gAttackSpeed = 10f;
    public float       gWaitTimeBetweenEachAttack = 10f;
    float              gAttackTimeCounter;
    public GameObject  gArcher;
    public GameObject  gArrow;
    float              gStep = 100f;
    public float       gArrowSize = 1f;
    public float       gArrowSpeed = 1000f;
    public GameObject  gBowPoint;
    public Vector3     gBowPointDefatultPosition = new Vector3(-0.0019f, 0f, 0f);
    public Vector3     gBowPointStretchedPosition = new Vector3(-0.0065f, 0f, 0f);
    public bool        gAttack = false;

    void Start()
    {

    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (gTarget != null)
        {
            Animator rAnimator = gArcher.transform.GetChild(0).GetComponent<Animator>();

            if (gWaitTimeBetweenEachAttack < gAttackTimeCounter 
                    && (rAnimator.GetCurrentAnimatorStateInfo(0).IsName("Archer1") == false
                        || rAnimator.GetCurrentAnimatorStateInfo(0).length <= rAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime))
            {

                gAttackTimeCounter = 0f;

                ArcherRotate();

                StartAnim();                

            }

            gAttackTimeCounter += 1f * Time.timeScale;
            
        }
    }

    void ArcherRotate()
    {
        Vector3 rTargetDirection = gTarget.transform.position - gArcher.transform.position;

        float rStep = gStep * Time.deltaTime;

 
        Vector3 rLastDirection = Vector3.RotateTowards(gArcher.transform.forward, rTargetDirection, rStep, 0.0f);

        gArcher.transform.rotation = Quaternion.LookRotation(rLastDirection);

    }

    void StartAnim()
    {
        gArrow.SetActive(true);

        gArcher.transform.GetChild(0).GetComponent<Animator>().Play("Archer1", 0, 0f);

        gArcher.transform.GetChild(0).GetComponent<Animator>().speed = gAttackSpeed * Time.deltaTime;


    }
    


    public void gArcherAttack()
    {        

        GameObject rArrow = Instantiate(gArrow, gArrow.transform.position, gArrow.transform.rotation);

        rArrow.transform.localScale = new Vector3(gArrowSize, gArrowSize, gArrowSize);

        rArrow.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

        rArrow.transform.GetComponent<BoxCollider>().enabled = true;

        Vector3 rTargetDirection = (gTarget.transform.position - rArrow.transform.position).normalized;

        rArrow.SetActive(true);

        rArrow.GetComponent<Rigidbody>().AddForce(rTargetDirection * gArrowSpeed);

        rArrow.AddComponent<DestroyAll>();

        float rStep = gStep; // * Time.deltaTime;

        Vector3 rLastDirection = Vector3.RotateTowards(rArrow.transform.forward, rTargetDirection, rStep, 0.0f);

        rArrow.transform.rotation = Quaternion.LookRotation(rLastDirection);

    }

    public void gAttackTimeCounterReset()
    {
        gAttackTimeCounter = 0f;
    }

}
