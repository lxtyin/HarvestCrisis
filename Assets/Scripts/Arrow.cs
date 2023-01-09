using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject arrow;
    public GameObject arrow_back;
    SpriteRenderer back_sprite;

    public static Arrow instance;
    private void Awake() {
        instance = this;
        back_sprite = arrow_back.GetComponent<SpriteRenderer>();
        hide();
    }

    /// <summary>
    /// 显示并更新arrow的位置和显示比例
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="percent"></param>
    public static void update_arrow(Vector2 from, Vector2 to, float percent) {
        var dir = to - from;
        float rotz = Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x);
        instance.arrow.transform.eulerAngles = new Vector3(0, 0, rotz);
        instance.arrow.transform.localScale = new Vector3(dir.magnitude, 2, 1);
        instance.arrow.transform.position = from;
        instance.arrow_back.transform.localScale = new Vector3(percent, 1, 1);
        if(percent > 0.99f) {
            instance.back_sprite.color = new Color(255, 0, 0);
        } else {
            instance.back_sprite.color = new Color(255, 132, 0);
        }
    }

    public static void hide() {
        instance.arrow.transform.localScale = new Vector3(0, 0, 0);
    }

}
