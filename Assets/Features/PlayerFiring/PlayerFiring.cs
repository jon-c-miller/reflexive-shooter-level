using UnityEngine;

/// <summary> Handles player firing input and execution. Uses camera's transform (passed in via IOnNotify) for fire direction. </summary>
public class PlayerFiring : MonoBehaviour, IListener
{
    [SerializeField] PlayerFiringV view;
    [SerializeField] PlayerFiringM model = new();

    public void IOnNotify(Notifies notifyID, params object[] data)
    {
        switch (notifyID)
        {
            case Notifies.PlayerFiringSetActiveStatus:
                bool isActive = (bool)data[0];
                model.IsActive = isActive;
                // If enabling, reset the reticle position for smooth transition into combat
                if (isActive && view.Cam != null)
                {
                    ResetReticle();
                }
                view.gameObject.SetActive(isActive);
                break;

            case Notifies.PlayerCamAnnounceSelfTransform:
                view.Cam = (Transform)data[0];
                break;
        }
    }

    public void CalculateLaunchAndHitPoints()
    {
        // Get camera position for launch point and apply desired fire offsets
        Vector3 updatedLaunchPoint = view.Cam.localPosition;
        updatedLaunchPoint += view.Cam.right * model.CameraOffset.x;
        updatedLaunchPoint += view.Cam.up * model.CameraOffset.y;
        updatedLaunchPoint += view.Cam.forward * model.CameraOffset.z;
        model.LaunchPoint = updatedLaunchPoint;

        // Create a projected end point for launch direction by extending along camera forward
        model.ProjectedEndPoint = view.Cam.forward * model.FireDirectionEndDistance;

        // Move the fire point every frame if not detached
        if (!model.DetachedFirePoint)
        {
            view.UpdateLaunchPointPosition(updatedLaunchPoint, model.FirePointCatchupRate);
        }
    }

    public void AdjustReticleSizeBasedOnAim()
    {
        Vector3 reticlePosition = Vector3.zero;
        float distanceBasedSize = 0;
        bool isValidHit = false;

        // Raycast from the launch point to the projected end point
        if (Physics.Raycast(model.LaunchPoint, model.ProjectedEndPoint - model.LaunchPoint, out RaycastHit hit))
        {
            // Check the valid tags array and ensure the hit is valid
            for (int i = 0; i < model.ValidHitTags.Count; i++)
            {
                if (hit.collider.CompareTag(model.ValidHitTags[i]))
                {
                    isValidHit = true;
                    reticlePosition = hit.point;

                    if (model.ShowDebugRayToHit)
                    {
                        Debug.DrawRay(model.LaunchPoint, reticlePosition - model.LaunchPoint, Color.red);
                    }

                    if (model.LogHitObject)
                    {
                        Debug.Log($"Raycast hit {hit.collider.name}.");
                    }
                    break;
                }
            }
        }

        if (isValidHit)
        {
            /// Update the scale of the reticle in proportion to its distance from the camera:

            // Get sqr magnitude of dist from cam to hit (cheaper to calculate) and convert it back to actual distance
            float sqrDistanceFromFireToHit = (reticlePosition - view.Cam.localPosition).sqrMagnitude;
            float distanceFromFireToHit = Mathf.Sqrt(sqrDistanceFromFireToHit);

            // Get reticle scale based on distance and size modifier
            distanceBasedSize = distanceFromFireToHit * model.ReticleHitSizeModifier;

            // Apply new position and scale to the reticle, also passing in a rate to lerp it to the new position
            view.UpdateReticlePositionAndScale(reticlePosition, distanceBasedSize, model.FirePointCatchupRate);
        }
        else
        {
            ResetReticle();
        }
    }

    void ResetReticle()
    {
        // Get reticle scale based on modifiers, and get position slightly forward of camera (hide reticle if needed)
        Vector3 reticlePosition = view.Cam.localPosition + view.Cam.forward.normalized * model.ReticleHiddenDistance;

        // Apply new position near camera to reticle for a smooth transition when it lerps back to hit point
        view.UpdateReticlePositionAndScale(reticlePosition, 0);
    }
    
    void Update()
    {
        if (model.IsActive)
        {
            view.GetInput(model);

            if (view.Cam != null)
            {
                CalculateLaunchAndHitPoints();
                AdjustReticleSizeBasedOnAim();
            }
        }
    }
}