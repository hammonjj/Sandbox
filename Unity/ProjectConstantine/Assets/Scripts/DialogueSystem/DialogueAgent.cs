using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAgent : MonoBehaviourBase
{
    public float DistanceToTalk = 2.0f;
    public Constants.Enums.Characters CharacterName;

    private bool _hasDialogue;
    private bool _isDialogueActive;
    private GameObject _player;
    private GameObject _dialogueBox;
    private EventManager _eventManager;

    private void Awake()
    {
        var dialog = DialogManager.GetInstance().GetDialog(CharacterName);
        LogDebug($"DialogueAgent - Character: {CharacterName}");
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Constants.Tags.Player);
        //_dialogueBox = GameObject.FindGameObjectWithTag(Constants.Tags.DialogueBox);

        _eventManager = EventManager.GetInstance();
        _eventManager.onAdvanceScenePressed += OnDisplayDialog;
    }

    private void OnDisplayDialog(bool value)
    {
        if(!value ||
            !_hasDialogue ||
            Vector3.Distance(gameObject.transform.position, _player.transform.position) > DistanceToTalk)
        {
            return;
        }
        else if(_isDialogueActive)
        {
            //Speed up dialog or skip
            return;
        }

        LogDebug("Starting Dialog");
        _isDialogueActive = true;
        //Display current dialogue
    }
}
