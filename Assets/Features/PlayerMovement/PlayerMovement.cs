using UnityEngine;

/// <summary> Handles player movement input and execution. Uses and updates camera's transform (passed in via IOnNotify) for move direction. </summary>
public class PlayerMovement : MonoBehaviour, IListener, ICanBeTargeted
{
    [SerializeField] PlayerMovementV view;
    [SerializeField] PlayerMovementM model = new();

    public Vector3 ICurrentPosition => view.PlayerTransform.localPosition;

    public void IOnNotify(Notifies notifyID, params object[] data)
    {
        switch (notifyID)
        {
            case Notifies.PlayerMovementSetActiveStatus:
                bool isActive = (bool)data[0];
                model.IsActive = isActive;
                break;

            case Notifies.PlayerMovementInitialize:
                view.InitializePosition();
                break;

            case Notifies.PlayerCamAnnounceSelfTransform:
                view.Cam = (Transform)data[0];
                break;
        }
    }

    void Start() => NotifyHandler.N.QueueNotify(Notifies.PlayerMovementAnnounceSelf, this);

    void Update()
    {
        if (model.IsActive)
        {
            view.GetInput(model);
            view.FaceCamera();
        }
    }

    void FixedUpdate()
    {
        if (model.IsActive)
        {
            view.Move(model);
        }
    }
}