public class CameraController : MonoBehaviour
{
    public Transform spawnPoint; //this is where the rts camera starts at

    RTSController rtsController; //the rtsController script attached to the camera

    Transform lerpTarget; //the transform the camera is supposed to lerp to

    bool isLerping = false;

    public float transitionSpeed = 5f;

    public Vector3 rtsOffset; //distance the camera moves back and up when going back to rts camera   

    private void Start()
    {
        rtsController = this.GetComponent<RTSController>();
        this.transform.position = spawnPoint.transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (rtsController.GetTargetUnit())
            {
                lerpTarget = rtsController.GetTargetUnit().transform.Find("CameraHook").transform;

                isLerping = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            lerpTarget.position = this.transform.position + rtsOffset;
            isLerping = true;
        }
    }

    private void FixedUpdate()
    {
        if (isLerping)
            Lerp(lerpTarget);
    }

    void Lerp(Transform targetPosition)
    {
        this.transform.position = Vector3.Lerp(transform.position, targetPosition.transform.position, Time.deltaTime * transitionSpeed);       

        if (Vector3.SqrMagnitude(transform.position - lerpTarget.position) < 0.001)
        {
            isLerping = false;
        }
    }

    public bool GetIsLerping()
    {
        return isLerping;
    }
}
