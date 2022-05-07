using System.Collections;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float EasyShockRecharge = 2.0f;
    public float MediumShockRecharge = 1.0f;
    public float HardShockRecharge = 0.2f;

    public GameObject[] Rings;

    private bool _gameStarted;
    private float _ringShockRecharge;
    private float _rollingRingShockRecharge = 0.5f;

	void Start()
	{
	    _ringShockRecharge = EasyShockRecharge;
    }
	
	// Update is called once per frame
	void Update()
	{
	    if(_gameStarted)
	    {
	        return;
	    }

	    if(Input.GetMouseButtonDown(0) || Input.touchCount >= 1)
	    {
	        _gameStarted = true;
            StartCoroutine(_DebugShockGenerator());
	    }
    }

    IEnumerator _DebugShockGenerator()
    {
        IEnumerator coroutine = null;

        Debug.Log("Single Random");
        coroutine = _RandomRingShocker(1);
        StartCoroutine(coroutine);

        yield return new WaitForSeconds(15);

        while (true)
        {
            Debug.Log("Multi Random");

            StopCoroutine(coroutine);
            coroutine = _RandomRingShocker(2);
            StartCoroutine(coroutine);

            yield return new WaitForSeconds(15);

            Debug.Log("Rolling - Random: " + Random.Range(0, 1));

            StopCoroutine(coroutine);
            coroutine = _RollingRingShocker(Random.Range(0, 1) == 1, Random.Range(0, 1) == 1);
            StartCoroutine(coroutine);
        }
    }

    IEnumerator _RandomRingShocker(uint ringsToShock)
    {
        while(true)
        {
            for(var ring = 0; ring < ringsToShock; ring++)
            {
                var ringToShock = Random.Range(0, Rings.Length - 1);
                Rings[ringToShock].GetComponent<RingShocker>().Charge();
            }
            

            yield return new WaitForSeconds(_ringShockRecharge);
        }
    }

    IEnumerator _RollingRingShocker(bool rollUp, bool rollFromPlayerPosition)
    {
        while(true)
        {
            foreach(var ring in (rollUp ? Rings : Rings.Reverse()))
            {
                ring.GetComponent<RingShocker>().Charge();
                yield return new WaitForSeconds(_rollingRingShockRecharge);
            }


            yield return new WaitForSeconds(_ringShockRecharge);
        }
    }
}
