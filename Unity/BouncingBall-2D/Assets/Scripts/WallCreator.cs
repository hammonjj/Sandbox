using System;
using UnityEngine;

public class WallCreator : MonoBehaviour
{
    public GameObject PlayerWall;
    
    private Vector2 _startingPosition;
    private Vector2 _endingPosition;

    private Vector2 _startingTouchPosition;
    private Vector2 _endingEndingPosition;
    private GameController _gameController;

    private void Start()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    private void Update()
    {
        if(!_gameController.GameStarted)
        {
            return;
        }

#if true
        if(Input.touchCount != 0)
        {
            var touch = Input.GetTouch(0);
            var touchPhase = Input.GetTouch(0).phase;

            //Touch Begin
            if(touchPhase == TouchPhase.Began)
            {
                _startingTouchPosition = touch.position;
            }
            //Touch End
            else if(touchPhase == TouchPhase.Ended &&
                    (Math.Abs(touch.position.x - _startingTouchPosition.x) > 0.1 && Math.Abs(_startingTouchPosition.y - touch.position.y) > 0.1))
            {
                _endingEndingPosition = touch.position;

                var v2 = _endingEndingPosition - _startingTouchPosition;
                var wallAngle = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
                if(wallAngle < 0)
                {
                    wallAngle += 360;
                }

                wallAngle -= 360;

                var playerWall = Instantiate(
                    PlayerWall,
                    _startingPosition,
                    Quaternion.identity);

                playerWall.transform.eulerAngles = new Vector3(0, 0, wallAngle + 90);
            }
        }
#endif
#if true
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButtonDown(0))
        {
            _startingPosition = mousePosition;
        }
        else if(Input.GetMouseButtonUp(0) && 
            (Math.Abs(mousePosition.x - _startingPosition.x) > 0.1 && Math.Abs(mousePosition.y - _startingPosition.y) > 0.1))
        {
            _endingPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var v2 = _endingPosition - _startingPosition;
            var wallAngle = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
            if(wallAngle < 0)
            {
                wallAngle += 360;
            }

            wallAngle -= 360;

            var playerWall = Instantiate(
                PlayerWall,
                _startingPosition,
                Quaternion.identity);
            
            playerWall.transform.eulerAngles = new Vector3(0, 0, wallAngle + 90);
        }
#endif
    }
}
