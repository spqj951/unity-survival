using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    //스피드 조정 변수
    [SerializeField]
    private float walkSpeed; //데이터 직렬화 및 이진화 : 데이터 저장시 유용
    [SerializeField]
    private float runSpeed;
    [SerializeField] float crouchSpeed;//앉았을 때의 속도

    private float applySpeed;

    //상태 변수
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    //앉았을 때 얼만큼의 높이로 앉을지 결정하는 변수;
    [SerializeField]
    private float crouchPosY;
    private float originPosY;//처음 높이
    private float applyCrouchPosY;

    private Rigidbody myRigid; //collider에 물리학을 입힘

    [SerializeField] private float lookSensitivity;//카메라 민감도

    [SerializeField] private float jumpForce;

    //땅 착지 여부
    private CapsuleCollider capsuleCollider;

    //카메라 한계
    [SerializeField] 
    private float cameraRotationLimit;

    private float currentCameraRotationX = 0;
  
    private Camera theCamera;
    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        theCamera = GetComponentInChildren<Camera>();
        applySpeed = walkSpeed;
        capsuleCollider = GetComponent<CapsuleCollider>();
        originPosY = theCamera.transform.localPosition.y;//내가 속해 있는 오브젝트에 대한 상대 위치
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        Move();
        CameraRotation();
        CharacterRotation();
        TryCrouch();
    }

    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCorouthine());
    }

    IEnumerator CrouchCorouthine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY,0.3f);//1번째 인자에서 2번째 인자까지 3번째 인자의 비율로
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if(count > 15)//LERP함수는 0~1사이라 끝나지가 않음 그러므로 횟수를 제한해놓고 끝나면 1로 돌리도록 만들어줌
            {
                break;
            }
            yield return null;//한프레임마다 대기 -> 부드러운 움직임
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);

    }

    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;// (1,0,0) * (1 || -1)
        Vector3 moveVertical = transform.forward * moveDirZ;//(0,0,1) * (1 || -1)

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed;//속력

        myRigid.MovePosition(transform.position + velocity * Time.deltaTime);


    }

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);//캡슐콜라이더 y 이 1/2
    }
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)//keydown 은 한번 getkey 는 누른 상태
        {
            Jump();
        }
    }

    private void Jump()
    {
        myRigid.velocity = transform.up * jumpForce;
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    private void Running()
    {
        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }
    private void CharacterRotation()
    {
        //좌우 캐릭터 회전
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(characterRotationY));//rotation 은 quaterion


    }

    private void CameraRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");

        float cameraRotationX = xRotation * lookSensitivity;

        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);//범위를 지정함

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);

       
    }
}
