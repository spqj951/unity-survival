using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Craft
{
    public string craftName;//이름
    public GameObject go_Prefab;//실제 설치될 프리팹
    public GameObject go_Previewprefab;//미리보기 프리팹
}
public class CraftManual : MonoBehaviour
{
    private bool isActivated = false;
    private bool isPreviewActivated = false;
    [SerializeField]
    private GameObject go_BaseUI;

    [SerializeField]
    private Craft[] craft_fire;//모닥불용 탭

    private GameObject go_Preview;
    private GameObject go_Prefab;

    [SerializeField]
    private Transform tf_Player;//플레이어 위치

    //for raycast
    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float range;

    public void SlotClick(int _slotNumber)
    {
        //미리보기 생성
        go_Preview = Instantiate(craft_fire[_slotNumber].go_Previewprefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_fire[_slotNumber].go_Prefab;
        isPreviewActivated = true;
        go_BaseUI.SetActive(false);
        Debug.Log(tf_Player.position + tf_Player.forward + " "+ tf_Player.position + " "+ tf_Player.forward);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
        {
            Window();
        }
        if (isPreviewActivated)
        {
            PreviewPositionUpdate();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Build();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel();
        }
    }
    private void Build()
    {
        if (isPreviewActivated && go_Preview.GetComponent<PreviewObject>().IsBuildable())
        {
            Instantiate(go_Prefab, hitInfo.point, Quaternion.identity);
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;

            
        }
    }
    private void PreviewPositionUpdate()
    {
       
        if(Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            Debug.Log("hello");
            if(hitInfo.transform != null)
            {
                Debug.Log("hi");
                Vector3 _location = hitInfo.point;
                go_Preview.transform.position = _location;
            }
        }
    }

    private void Cancel()
    {
        if (isPreviewActivated)
        {

            Destroy(go_Preview);
        }

        isActivated = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;
        go_BaseUI.SetActive(false);
    }
    private void Window()
    {
        if (!isActivated)
        {
            OpenWindow();
        }
        else
        {
            CloseWindow();
        }
    }

    private void OpenWindow()
    {
        isActivated = true;
        go_BaseUI.SetActive(true);
    }

    private void CloseWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false);
    }
        
}
