using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed; //데이터 직렬화 및 이진화 : 데이터 저장시 유용

    private Rigidbody myRigid; //collider에 물리학을 입힘

    [SerializeField] private float lookSensitivity;//카메라 민감도

    [SerializeField] 
    private float cameraRotationLimit;

    private float currentCameraRotationX = 0;
  
    private Camera theCamera;
    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        theCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CameraRotation();
        CharacterRotation();
    }

    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;// (1,0,0) * (1 || -1)
        Vector3 moveVertical = transform.forward * moveDirZ;//(0,0,1) * (1 || -1)

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * walkSpeed;//속력

        myRigid.MovePosition(transform.position + velocity * Time.deltaTime);


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
