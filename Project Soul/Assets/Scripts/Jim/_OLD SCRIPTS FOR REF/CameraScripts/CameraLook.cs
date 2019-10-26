using UnityEngine;
using UnityEngine.AI;

public class CameraLook : MonoBehaviour
{
    //this script lets the user look around the screen
    //is like a sudo free cam
    //when the player has a path then the camera centers to the player for less dizzyness
    public float sensitivity = 0.025f;

    private float rotationX;
    private float rotationY;
    private const float adjustAmount = 0.5f;
    private GameObject player;
    private NavMeshAgent playerAgent;

    private Camera myCamera;
    private Vector3 vp;
    private Vector3 sp;
    private Vector3 v;
    private Quaternion startingCamPos;

    bool toResetView = false;
    void Start ()
    {
        myCamera = GetComponent<Camera>();
        startingCamPos = transform.localRotation;
        player = GameObject.FindGameObjectWithTag("Player");
        playerAgent = player.GetComponent<NavMeshAgent>();
        Cursor.lockState = CursorLockMode.Confined;
    }

    void LateUpdate ()
    {
        Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        //Detection for if the mouse pointer is inside game screen area
        Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
        if (playerAgent.hasPath)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
        else
        {
            toResetView = false;
        }

        //free cam
        if (!toResetView)
        {
            if (screenRect.Contains(Input.mousePosition))
            {
                rotationX = Input.mousePosition.x;
                rotationY += Input.GetAxis("Mouse Y");
                rotationY = Mathf.Clamp(rotationY, -15f, 30f);
                vp = myCamera.ScreenToViewportPoint(new Vector3(rotationX, rotationY, myCamera.nearClipPlane));

                vp.x -= adjustAmount;
                vp.y -= adjustAmount;
                vp.x *= sensitivity;
                vp.y *= sensitivity;
                vp.x += adjustAmount;
                vp.y += adjustAmount;

                transform.localRotation = Quaternion.Euler(-rotationY * vp.x + 10f, transform.localEulerAngles.y, transform.localEulerAngles.z);

                sp = myCamera.ViewportToScreenPoint(vp);

                v = myCamera.ScreenToWorldPoint(sp);
                transform.LookAt(v, Vector3.up);
            }
        }  
    }
}