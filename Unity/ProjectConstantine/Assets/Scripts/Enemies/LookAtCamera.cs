using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        //var camPos = Camera.main.transform.position;
        //Vector3 lookPos = new Vector3(camPos.x, camPos.y, transform.position.z);
        //transform.LookAt(lookPos);
        //transform.LookAt(camPos.position - camPos.forward);
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
        //Quaternion lookRotation = Camera.main.transform.rotation;
        //transform.rotation = lookRotation;
    }
}
