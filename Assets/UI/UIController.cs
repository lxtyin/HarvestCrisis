using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public static UIController instance;
    private void Awake() {
        instance = this;
    }

    public Image health_bar, breath_bar;
    public Image health_late_bar, breath_late_bar;
    public Image harvest_hint;
    public Text total_txt, round_txt, required_txt;
    public GameObject escpage, deathpage, endpage;

    /// <summary>
    /// 显示harvest is coming
    /// </summary>
    public void show_harvest_hint() {
        StopCoroutine("harvest_anim");
        StartCoroutine("harvest_anim");
    }
    IEnumerator harvest_anim() {
        harvest_hint.color = new Color(1, 1, 1, 0);
        for(int i = 0;i < 50;i++) {
            harvest_hint.color += new Color(0, 0, 0, 0.02f);
            yield return new WaitForSeconds(0.02f);
        }
        harvest_hint.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 50; i++) {
            harvest_hint.color -= new Color(0, 0, 0, 0.02f);
            yield return new WaitForSeconds(0.02f);
        }
        harvest_hint.color = new Color(1, 1, 1, 0);
    }

    /// <summary>
    /// 更新右上角文字
    /// </summary>
    /// <param name="tot"></param>
    /// <param name="req"></param>
    public void update_counter_text(int tot, int round, int req) {
        total_txt.text = tot.ToString();
        round_txt.text = "第 <color=#ffff00>" + round.ToString() + "</color> 轮";
        required_txt.text = "距下次Harvest <color=#ffff00>" + req.ToString() + "</color>";
    }

    public Sprite[] levelimg = new Sprite[6];
    public GameObject Buffoption;
    public Transform Buffline;

    /// <summary>
    ///  调出buff选择页面
    /// </summary>
    public void show_choosebuff(List<BuffElement> ls) {
        Global.pause = true;
        int n = ls.Count;
        float st = -(n * 100 + (n - 1) * 50) / 2 + 50;

        for(int i = 0;i < n;i++) {
            var g = Instantiate(Buffoption);
            g.transform.SetParent(Buffline);
            g.GetComponent<Buffoption>().set(ls[i]);
            g.transform.localPosition = new Vector3(st + i * 150, 0, 0);
        }
    }
    public void close_choosebuff() {
        for (int i = 0; i < Buffline.childCount; i++) {
            Destroy(Buffline.GetChild(i).gameObject);
        }
        Invoke("invokedepause", 0.5f);
    }
    void invokedepause() { Global.pause = false; }


    public void show_death() {
        Global.pause = true;
        deathpage.transform.localScale = Vector3.one;
    }

    public void show_end() {
        Global.pause = true;
        endpage.transform.localScale = Vector3.one;
    }

    void Update() {
        health_bar.transform.localScale = new Vector3(Global.instance.player.get_health_percent(), 1, 1);
        breath_bar.transform.localScale = new Vector3(Global.instance.player.get_breath_percent(), 1, 1);

        var dt = health_bar.transform.localScale - health_late_bar.transform.localScale;
        health_late_bar.transform.localScale += dt * 0.1f;

        dt = breath_bar.transform.localScale - breath_late_bar.transform.localScale;
        breath_late_bar.transform.localScale += dt * 0.1f;
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(Time.timeScale < 0.1f) {
                start();
            } else {
                pause();
            }
        }
    }

    public void pause() {
        escpage.transform.localScale = Vector3.one;
        Time.timeScale = 0;
        Global.pause = true;
    }
    public void start() {
        escpage.transform.localScale = Vector3.zero;
        endpage.transform.localScale = Vector3.zero;
        deathpage.transform.localScale = Vector3.zero;
        Time.timeScale = 1;
        Global.pause = false;
    }
    public void remake() {
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1;
        Global.pause = false;
    }
    public void quit() {
        Application.Quit();
    }

}
