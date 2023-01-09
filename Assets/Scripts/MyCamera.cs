using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{

    public Transform target;

    float target_size = 12;

    private void Update() {

        float dr = -Input.GetAxis("Mouse ScrollWheel");
        target_size = Mathf.Clamp(target_size + dr * 10, 10, 50);

        Camera.main.orthographicSize += (target_size - Camera.main.orthographicSize) * 0.03f;

        var t = target.position;
        transform.position = new Vector3(t.x, t.y, -10);

        if(target.position.y > 350) { //²Êµ°
            target_size = 50;
        }
    }
}
