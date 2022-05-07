using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    private float _currentMusicTime = 0.0f;
    private AudioSource _audioSource;
    void Start()
    {
        //DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();
#if false
        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();

        if(!_audioSource.isPlaying)
        {
            //_audioSource.Play();
        }
#endif
    }

    void Update()
    {
        _currentMusicTime = _audioSource.time;
    }
}
