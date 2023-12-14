using UnityEngine;

public class PlayerMovementV : MonoBehaviour
{
    public Transform PlayerTransform;

    [SerializeField] Rigidbody playerRigidbody;
    [Space]
    [SerializeField] Transform startPosition;

    [HideInInspector] public Transform Cam;

    const string HORIZONTALINPUT = "Horizontal";
    const string VERTICALINPUT = "Vertical";

    float previousCamY;

    public void InitializePosition()
    {
        playerRigidbody.position = startPosition.localPosition;
    }

    public void GetInput(PlayerMovementM model)
    {
        // Get movement input based on rate
        float rightInput = Input.GetAxisRaw(HORIZONTALINPUT) * model.StrafeRate * Time.deltaTime;
        float forwardInput = Input.GetAxisRaw(VERTICALINPUT) * model.ForwardMoveRate * Time.deltaTime;

        // Get jump input
        if (Input.GetKeyDown(model.JumpKey))
        {
            // If infinite jump isn't intended, only jump when near the ground and velocity indicates nearly stationary state
            if (!model.AllowInfiniteJump && PlayerTransform.localPosition.y < 0.6f && Mathf.Abs(playerRigidbody.velocity.y) < 0.01f)
            {
                playerRigidbody.AddForce(Vector3.up * model.JumpForce, ForceMode.Impulse);
            }
        }

        // Compensate for moving diagonally
        if (forwardInput != 0 && rightInput != 0)
        {
            forwardInput *= 0.75f;
            rightInput *= 0.75f;
        }

        // Get forward and right based on camera facing
        Vector3 forward = Cam != null ? Cam.forward : Vector3.forward;
        Vector3 right = Cam != null ? Cam.right : Vector3.right;

        // Remove camera height from calculation
        forward.y = 0f;
        right.y = 0f;

        // Get target movement based on normalized camera vectors and input
        model.TargetMovement = forward.normalized * forwardInput + right.normalized * rightInput;

        // Dampen movement whilst in midair (aboveground and velocity sufficiently greater than 0)
        if (PlayerTransform.localPosition.y > 0.6f && Mathf.Abs(playerRigidbody.velocity.y) > 0.1f)
        {
            model.TargetMovement *= model.MidJumpMovementDamping;
        }
    }

    public void Move(PlayerMovementM model)
    {
        // Apply any movement, and swiftly halt the velocity while there is no input and not in the air
        if (model.TargetMovement != Vector3.zero)
        {
            playerRigidbody.AddForce(model.TargetMovement, ForceMode.VelocityChange);
        }
    }

    public void FaceCamera()
    {
        if (Cam != null)
        {
            // Face the same y direction as camera
            float camY = Cam.transform.localEulerAngles.y;
            if (Mathf.Abs(previousCamY - camY) > 0.01f)
            {
                playerRigidbody.MoveRotation(Quaternion.Euler(new Vector3(0, camY, 0)));
                previousCamY = camY;
            }
        }
    }
}