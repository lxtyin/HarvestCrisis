using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Math : MonoBehaviour//掌管一些啰嗦的数学计算
{
    //坐标系：右x正，上y正
    //rotation为x正方向为底逆时针转角度数

    public static float getRotation(Vector2 dir) {
        float ang = Vector2.Angle(Vector2.right, dir);
        if (dir.y < 0)
            ang = -ang;
        return ang;
    }

    public static Vector2 getVector(float rot) {
        return new Vector2(Mathf.Cos(rot * Mathf.Deg2Rad), Mathf.Sin(rot * Mathf.Deg2Rad)).normalized;
    }




}
