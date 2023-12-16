using UnityEngine;

public class AIProjectile : MonoBehaviour
{
    Collider collision;
    Rigidbody rigidBody;

    float timeToDisable;
    int hitAmount;
    bool hasHit;

	public static event System.Action<AIProjectile> OnReturnAIProjectile;
    
    public void Launch(Vector3 direction, Vector3 launchPosition, float velocity, int hitAmount)
    {
        timeToDisable = 5;
        this.hitAmount = hitAmount;
        Quaternion launchDirection = Quaternion.LookRotation(direction);
        transform.SetPositionAndRotation(launchPosition, launchDirection);
        gameObject.SetActive(true);
        hasHit = false;
        collision.enabled = true;
        rigidBody.useGravity = false;
        rigidBody.AddForce(direction * velocity, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other)
    {
        // Prevent projectiles that bounce off of the environment from colliding with player
        hasHit = true;
        rigidBody.useGravity = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasHit)
        {
            return;
        }

        // Convey hitAmount to ICanBeHit interface (player) if not null
        ICanBeHit hit = other.GetComponentInParent<ICanBeHit>();
        hit?.IHit(hitAmount);
        hasHit = true;
        ReturnProjectile();
        // Debug.Log($"Hit {other.name}.");
    }

    void ReturnProjectile()
    {
        OnReturnAIProjectile?.Invoke(this);
        rigidBody.velocity = Vector3.zero;
        rigidBody.rotation = Quaternion.identity;
    }

    public void SetActiveStatus(bool isActive) => gameObject.SetActive(isActive);

    void Awake()
    {
        TryGetComponent(out collision);
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