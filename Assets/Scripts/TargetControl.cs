using NeatDiggers.GameServer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetControl : MonoBehaviour
{
    private Action<Vector> callback;

    public void StartListen(Action<Vector> callback) => this.callback = callback;
    public void StopListen() => callback = null;

    void Update()
    {
        int x, y;
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 touchPos = Input.GetTouch(0).position;
                Ray ray = Camera.main.ScreenPointToRay(touchPos);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    x = (int)hit.transform.position.x;
                    y = (int)hit.transform.position.z;
                    transform.position = new Vector3(x, 0, y);
                    callback?.Invoke(new Vector { X = x, Y = y });
                }
            }
        }
    }
}
