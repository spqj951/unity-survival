using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed; //������ ����ȭ �� ����ȭ : ������ ����� ����

    private Rigidbody myRigid; //collider�� �������� ����

    [SerializeField] private float lookSensitivity;//ī�޶� �ΰ���

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

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * walkSpeed;//�ӷ�

        myRigid.MovePosition(transform.position + velocity * Time.deltaTime);


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
