using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAgent : MonoBehaviour
{
    public float DistanceToTalk = 2.0f;
    public Constants.Enums.Characters CharacterName;

    private bool _hasDialogue;
    private GameObject _player;
    private GameObject _dialogueBox;
    private EventManager _eventManager;

    private void Awake()
    {
        //Get currently available dialogue    
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Constants.Tags.Player);
        _dialogueBox = GameObject.FindGameObjectWithTag(Constants.Tags.DialogueBox);

        _eventManager = EventManager.GetInstance();
        _eventManager.onAdvanceScenePressed += OnDisplayDialogue;
    }

    private void Update()
    {
        
    }

    private void OnDisplayDialogue(bool value)
    {
        if(!value ||
            !_hasDialogue ||
            Vector3.Distance(gameObject.transform.position, _player.transform.position) > DistanceToTalk)
        {
            return;
        }

        //Display current dialogue
    }
}
