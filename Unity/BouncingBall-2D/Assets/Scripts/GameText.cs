using System.Collections;
using UnityEngine;

public class GameText : MonoBehaviour
{
    public GameObject SwipeInstructionText;
    public GameObject PortalInstructionText;

    void Start()
    {
        StartCoroutine(_showInstructions());
    }

    private IEnumerator _showInstructions()
    {
        yield return new WaitForSeconds(3);

        SwipeInstructionText.SetActive(false);
        PortalInstructionText.SetActive(false);
    }
}
