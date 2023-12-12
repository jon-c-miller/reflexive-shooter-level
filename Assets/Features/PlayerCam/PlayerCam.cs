using UnityEngine;

/// <summary> Handles player look direction input and execution; announces self transform for other features that rely on it. </summary>
public class PlayerCam : MonoBehaviour, IListener
{
    [SerializeField] PlayerCamV view;
    [SerializeField] PlayerCamM model = new();

    public void IOnNotify(Notifies notifyID, params object[] data)
    {
        switch (notifyID)
        {
            case Notifies.PlayerCamSetActiveStatus:
                bool isActive = (bool)data[0];
                model.IsActive = isActive;
                view.Cam.enabled = isActive;
                break;

            case Notifies.PlayerMovementAnnounceSelf:
                model.Player = (ICanBeTargeted)data[0];
                break;
        }
    }

    void Start()
    {
        NotifyHandler.N.QueueNotify(Notifies.PlayerCamAnnounceSelfTransform, view.Cam.transform);
        model.TargetRotation = view.Cam.transform.localRotation;
    }

    void Update()
    {
        if (model.IsActive)
        {
            view.GetMouseInput(model);
            view.UpdateCameraDirection(model);
            view.KeepCameraWithPlayer(model);
        }
    }
}