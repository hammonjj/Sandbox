using UnityEngine;
using UnityEngine.SceneManagement;

public class RingShocker : MonoBehaviour
{
    public float ChargeDuration = 1.0f;

    private bool _charging;
    private float _currentCharge;
    private SpriteRenderer _spriteRenderer;

	private void Start()
	{
	    _spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	private void Update()
    {
        if(_charging)
        {
            _spriteRenderer.color = Color.Lerp(Color.white, Color.red, _currentCharge / ChargeDuration);
            _currentCharge += 0.03f;

            if(_currentCharge > 1.0f)
            {
                _charging = false;
                _currentCharge = 0.0f;

                //Check if player is currently in the ring
                var containsPlayer = GetComponent<RingCollider>().ContainsPlayer();
                //Debug.Log("Rings: " + gameObject.name + " - Contains Player: " + containsPlayer);

                if(containsPlayer)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }

                _spriteRenderer.color = Color.white;
            }
        }	
	}

    public void Charge()
    {
        _charging = true;
    }
}
