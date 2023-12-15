using UnityEngine;

public class PlayerCamV : MonoBehaviour
{
    [SerializeField] Camera cam;
    public Camera Cam => cam;

    const string XINPUT = "Mouse X";
    const string YINPUT = "Mouse Y";

    public void InitializeRotation(PlayerCamM model)
    {
        model.VerticalInput = 0;
        model.HorizontalInput = 0;
        model.TargetRotation = Quaternion.identity;
        cam.transform.rotation = Quaternion.identity;
    }

    public void GetMouseInput(PlayerCamM model)
    {
        // Get rotation input for x axis (based on cam y input) and y axis (based on cam x input)
        model.VerticalInput += Input.GetAxis(YINPUT) * model.VerticalSensitivity;
        model.HorizontalInput += Input.GetAxis(XINPUT) * model.HorizontalSensitivity;

        if (model.ClampVerticalRotation)
        {
            model.VerticalInput = Mathf.Clamp(model.VerticalInput, model.VerticalRotationMin, model.VerticalRotationMax);
        }

        // Combine horizontal and vertical inputs as rotations based on input (horizontal must preceed in order to avoid z rotation issues)
        model.TargetRotation = Quaternion.AngleAxis(model.HorizontalInput, Vector3.up) * Quaternion.AngleAxis(model.VerticalInput, Vector3.left);
    }

    public void UpdateCameraDirection(PlayerCamM model)
    {
        if(model.SmoothInput)
        {
            Cam.transform.localRotation = Quaternion.Slerp(Cam.transform.localRotation, model.TargetRotation, model.SmoothingStrength * Time.deltaTime);
        }
        else
        {
            Cam.transform.localRotation = model.TargetRotation;
        }
    }

    public void KeepCameraWithPlayer(PlayerCamM model)
    {
        if (model.Player != null)
        {
            Vector3 playerPosition = model.Player.ICurrentPosition;
            playerPosition.y += model.ViewHeight;
            cam.transform.localPosition = playerPosition;
        }
    }
}