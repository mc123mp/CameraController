using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 rtsOffset;
    public float rtsDownwardRotation;
    Transform lerpTarget;
    Vector3 targetAngle;
    bool IsRTS;

    Transform currentView; //current view
    public Transform spawnPoint;
    RTSController rtsController;
    FPSController fpsController;
    
    public float transitionSpeed = 5f;
    bool IsLerping = false;

    // Start is called before the first frame update
    void Start()
    {
        rtsController = this.GetComponent<RTSController>();
        //fpsController = this.GetComponent<FPSController>(); //implement later
        transform.position = spawnPoint.transform.position + rtsOffset;
        targetAngle.x = rtsDownwardRotation;
        IsRTS = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (rtsController.GetTargetUnit() && IsRTS)
            {
                lerpTarget = rtsController.GetTargetUnit().transform.Find("CameraHook").transform;
                //print("Target: " + lerpTarget);
                IsLerping = true;
                IsRTS = false;
                rtsController.enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!IsRTS)
            {
                rtsController.enabled = true;
                lerpTarget.position = transform.position + rtsOffset;
                lerpTarget.transform.localEulerAngles = targetAngle;
                //print("Target pos: " + lerpTarget.position + ", target angle x: " + lerpTarget.transform.localEulerAngles.x);
                IsLerping = true;
                IsRTS = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsLerping)
            Lerp(lerpTarget);
    }    

    void Lerp(Transform targetPosition)
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition.transform.position, Time.deltaTime * transitionSpeed);

        Vector3 currentAngle = new Vector3(
            Mathf.LerpAngle(transform.rotation.eulerAngles.x, targetPosition.transform.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
            Mathf.LerpAngle(transform.rotation.eulerAngles.y, targetPosition.transform.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
            Mathf.LerpAngle(transform.rotation.eulerAngles.z, targetPosition.transform.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));

        transform.eulerAngles = currentAngle;

        if (Vector3.SqrMagnitude(transform.position - lerpTarget.position) < 0.001)
        {
            IsLerping = false;
            print("Final pos: " + lerpTarget.position);
        }
    }

    public bool GetIsLerping()
    {
        return IsLerping;
    }
}
