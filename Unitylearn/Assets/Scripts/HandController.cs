using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //현재 장착된 무기
    [SerializeField]
    private Hand currentHand;

    //공격여부
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo; //쏜 광선에 닿은 물체의 정보

   

    // Update is called once per frame
    void Update()
    {
        TryAttack();
    }

    private void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;//중복 실행 방지
        currentHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;

        //공격활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;//팔 접음

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
        isAttack = false;


    }

    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                //충돌했을 경우
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
              
            }

            yield return null;
         
        }
    }

    private bool CheckObject()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }

        return false;
    }
}
