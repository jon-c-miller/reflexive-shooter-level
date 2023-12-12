using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    Rigidbody rigidBody;

    float timeToDisable;

	public static event System.Action<PlayerProjectile> OnReturnPlayerProjectile;
    
    public void Launch(Vector3 direction, Vector3 launchPosition, float velocity)
    {
        timeToDisable = 6;
        transform.localPosition = launchPosition;
        gameObject.SetActive(true);
        rigidBody.AddForce(direction * velocity, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log($"Hit {other.collider.name}.");
        ReturnProjectile();
    }

    void ReturnProjectile()
    {
        OnReturnPlayerProjectile?.Invoke(this);
        rigidBody.velocity = Vector3.zero;
        rigidBody.rotation = Quaternion.identity;
    }

    public void SetActiveStatus(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    void Awake()
    {
        TryGetComponent(out rigidBody);
    }

    void Update()
    {
        // Automatically disable if nothing hit
        if (timeToDisable < 0)
        {
            ReturnProjectile();
            timeToDisable = 6;
        }
        timeToDisable -= Time.deltaTime;
    }
}