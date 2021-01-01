using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerScript : SingletonScript<InputManagerScript>
{
    //First 2 variable is changeable.
    private float _thresholdPos;  //Minimum distance to swipe. User must pass this value while swapping.
    private float _thresholdTime = 0.5F; //Maximum time to swipe. For example user must swipe 200 pixel in 0.5 secs. Ratio is 200 pixel for 0.5 secs.
    private bool[] _inputs = {false,false,false,false,false}; //touched, swiped right, swiped left, swiped up, swiped down
    private TouchPhase _lastPhase;
    private Vector2 _beginTouchPos;
    private float _beginTime;
    private bool _canSwipe = true;

    private void Start()
    {
        _thresholdPos = Screen.width / 10;
    }

    // Update is called once per frame
    void Update()
    {
        DetectInputs();
    }

    private void ResetInputs()
    {
        for (int i = 0; i<_inputs.Length; i++)
        {
            _inputs[i] = false;
        }
    }

    private void DetectInputs()
    {
        ResetInputs();
        
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Ended)
            {
                //Debug.Log((Time.time - _beginTime) + " " + (Mathf.Abs(touch.position.x) - Mathf.Abs(_beginTouchPos.x)));
                if (_lastPhase == TouchPhase.Began || _lastPhase == TouchPhase.Stationary)
                {
                    _inputs[0] = true;
                    //Debug.Log("Touched");
                }
                _lastPhase = TouchPhase.Ended;
                _canSwipe = true;
            }
            else if (touch.phase == TouchPhase.Moved && _canSwipe)
            {
                Vector2 pos = touch.position;
                _lastPhase = TouchPhase.Moved;
                float deltaX = pos.x - _beginTouchPos.x;
                float deltaXAbs = Mathf.Abs(pos.x - _beginTouchPos.x);
                float deltaY = pos.y - _beginTouchPos.y;
                float deltaYAbs = Mathf.Abs(pos.y - _beginTouchPos.y);
                if (deltaXAbs > deltaYAbs)
                {
                    if (deltaXAbs > _thresholdPos && Time.time - _beginTime < deltaXAbs/_thresholdPos * _thresholdTime)
                    {
                        if (deltaX > 0)
                        {
                            _inputs[1] = true;
                            //Debug.Log("Swiped Right");
                        }
                        else
                        {
                            _inputs[2] = true;
                            //Debug.Log("Swiped Left");
                        }

                        _canSwipe = false;
                    }
                }
                else
                {
                    if (deltaYAbs > _thresholdPos && Time.time - _beginTime < deltaYAbs/_thresholdPos * _thresholdTime)
                    {
                        if (deltaY > 0)
                        {
                            _inputs[3] = true;
                            //Debug.Log("Swiped Up");
                        }
                        else
                        {
                            _inputs[4] = true;
                            //Debug.Log("Swiped Down");
                        }
                        _canSwipe = false;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Stationary)
            {
                _lastPhase = TouchPhase.Stationary;
            }
            else if (touch.phase == TouchPhase.Began)
            {
                _beginTouchPos = touch.position;
                _beginTime = Time.time;
                _lastPhase = TouchPhase.Began;
            }
        }
    }

    public bool[] GetInputs()
    {
        return _inputs;
    }
}
