using UnityEngine;

public class PlayerHealth : MonoBehaviourBase
{
	[Header("Health")]
	public int MaxHealth = 10;
	public int CurrentHealth = 10;
	public float BlobletVerticalVelocity = 6.5f;
	public float BlobletHorizontalVelocity = 6.5f;
	public Transform BlobletSpawn;
	public GameObject BlobletPrefab;

	private GameManager _gameManager;

	private void Awake()
	{
		LogDebug("Player Health Initilized");
		_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Bloblet"))
        {
			CurrentHealth++;
			Destroy(col.gameObject);
			//LogDebug("Destroying bloblett prefab");
		}
		else if(col.gameObject.CompareTag("Hazard"))
		{
			//LogDebug("Player hit hazard");
			TakeDamage(col.gameObject.GetComponent<Hazard>().DamagePerHit);
		}
	}

	public void GainHealth(int healthGained)
    {
		CurrentHealth += healthGained;
		if(CurrentHealth >= MaxHealth)
        {
			CurrentHealth = MaxHealth;
        }
    }
	public void TakeDamage(int damageAmount, bool createBloblet = true)
	{
		CurrentHealth -= damageAmount;

		if(CurrentHealth <= 0)
		{
			//Player Dies - Load from last save
			_gameManager.LoadGame();
		}
		else if(CurrentHealth > 0 && createBloblet)
		{
			var bloblet = Instantiate(BlobletPrefab);
			bloblet.transform.position = BlobletSpawn.position;

			var blobletRigidBody = bloblet.GetComponent<Rigidbody2D>();
			blobletRigidBody.velocity = new Vector2(
				Random.Range(-1.0f, 1.0f) * BlobletHorizontalVelocity, BlobletVerticalVelocity);

			var attractor = bloblet.GetComponent<Attractor>();
			attractor.PlayerTransform = transform;
		}
	}
}
