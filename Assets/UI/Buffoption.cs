using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buffoption : MonoBehaviour {
    public Image main_img;
    public Text introduce;
    public Image lv_img;

    string buffname;

    public void set(BuffElement b) {
        introduce.text = b.introduce;
        buffname = b.name;
        main_img.sprite = b.img;
        lv_img.sprite = UIController.instance.levelimg[ Mathf.Min(6, Buff.level[buffname] + 1) ]; // 显示下一级
    }

    private float scale = 1;
    private void Update() {
        float dt = scale - transform.localScale.x;
        transform.localScale += new Vector3(dt, dt, 0) * 0.2f;
    }

    public void event_mouse_enter() {
        scale = 1.2f;
    }

    public void event_mouse_exit() {
        scale = 1f;
    }

    public void event_mouse_click() {
        Buff.add_buff(buffname);

        var par = ParticleController.AssignParticle(transform.position, 0, "Breath");
        par.transform.SetParent(Global.instance.canvas.transform);
        var main = par.main;
        main.startColor = new Color(0.2f, 1, 0.6f);
        par.transform.localScale = new Vector3(1.7f, 3, 1);
        par.transform.position = transform.position;


        UIController.instance.close_choosebuff();
    }

}
