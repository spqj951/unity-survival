using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    //���ǵ� ���� ����
    [SerializeField]
    private float walkSpeed; //������ ����ȭ �� ����ȭ : ������ ����� ����
    [SerializeField]
    private float runSpeed;
    [SerializeField] float crouchSpeed;//�ɾ��� ���� �ӵ�

    private float applySpeed;

    //���� ����
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    //�ɾ��� �� ��ŭ�� ���̷� ������ �����ϴ� ����;
    [SerializeField]
    private float crouchPosY;
    private float originPosY;//ó�� ����
    private float applyCrouchPosY;

    private Rigidbody myRigid; //collider�� �������� ����

    [SerializeField] private float lookSensitivity;//ī�޶� �ΰ���

    [SerializeField] private float jumpForce;

    //�� ���� ����
    private CapsuleCollider capsuleCollider;

    //ī�޶� �Ѱ�
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
        originPosY = theCamera.transform.localPosition.y;//���� ���� �ִ� ������Ʈ�� ���� ��� ��ġ
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
            _posY = Mathf.Lerp(_posY, applyCrouchPosY,0.3f);//1��° ���ڿ��� 2��° ���ڱ��� 3��° ������ ������
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if(count > 15)//LERP�Լ��� 0~1���̶� �������� ���� �׷��Ƿ� Ƚ���� �����س��� ������ 1�� �������� �������
            {
                break;
            }
            yield return null;//�������Ӹ��� ��� -> �ε巯�� ������
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);

    }

    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;// (1,0,0) * (1 || -1)
        Vector3 moveVertical = transform.forward * moveDirZ;//(0,0,1) * (1 || -1)

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed;//�ӷ�

        myRigid.MovePosition(transform.position + velocity * Time.deltaTime);


    }

    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);//ĸ���ݶ��̴� y �� 1/2
    }
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)//keydown �� �ѹ� getkey �� ���� ����
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
        //�¿� ĳ���� ȸ��
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(characterRotationY));//rotation �� quaterion


    }

    private void CameraRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");

        float cameraRotationX = xRotation * lookSensitivity;

        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);//������ ������

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);

       
    }
}
