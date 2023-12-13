using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    Rigidbody rigidBody;

    float timeToDisable;
    int hitAmount;

	public static event System.Action<PlayerProjectile> OnReturnPlayerProjectile;
    
    public void Launch(Vector3 direction, Vector3 launchPosition, float velocity, int hitAmount)
    {
        timeToDisable = 6;
        this.hitAmount = hitAmount;
        transform.localPosition = launchPosition;
        gameObject.SetActive(true);
        rigidBody.AddForce(direction * velocity, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other)
    {
        // Convey hitAmount to ICanBeHit interface (enemy) if not null
        ICanBeHit hit = other.collider.GetComponentInParent<ICanBeHit>();
        hit?.IHit(hitAmount);
        ReturnProjectile();
        Debug.Log($"Hit {other.collider.name}.");
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