using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffElement {
    public string name;
    public Sprite img;
    [Multiline]
    public string introduce;
    public int max_level = 3;
}

public class Buff : MonoBehaviour {

    public static Buff instance;

    public List<BuffElement> bufflist;

    public static Dictionary<string, int> level = new Dictionary<string, int>();
    
    private void Awake() {

        instance = this;
        level.Clear();
        foreach (BuffElement e in bufflist) {
            level.Add(e.name, 0);
        }

/*        level.Add("Faster gather", 0);
        level.Add("Faster breath", 0);
        level.Add("Repel", 0);
        level.Add("Bounce", 0);
        level.Add("Ejection", 0);

        level.Add("Deep breath", 0);
        level.Add("Hold breath", 0);
        level.Add("Blooding", 0);
        level.Add("Big body", 0);
        level.Add("Small body", 0);
        level.Add("Small body", 0);*/
    }

    /// <summary>
    /// 随机抽取若干个未满级的buff
    /// </summary>
    /// <param name="num"></param>
    public static List<BuffElement> select_buff(int num) {
        List<BuffElement> ls = new List<BuffElement>();
        foreach (BuffElement e in instance.bufflist) {
            if (level[e.name] < e.max_level) {
                ls.Add(e);
            }
        }

        num = Mathf.Min(num, ls.Count);
        for (int i = 0; i < ls.Count; i++) {
            int x = Random.Range(0, ls.Count);
            int y = Random.Range(0, ls.Count);
            var tmp = ls[x];
            ls[x] = ls[y];
            ls[y] = tmp;
        }
        return ls.GetRange(0, num);
    }

    /// <summary>
    /// 添加一个buff
    /// </summary>
    /// <param name="name"></param>
    public static void add_buff(string name) {
        level[name]++;
        if(name == "Faster gather") {
            Global.instance.player.accum_rate *= 1.3f;
        }
        if (name == "Faster breath") {
            Global.instance.player.breath_cd *= 0.8f;
        }
        if (name == "Sharp") {
            Global.instance.player.attack_rate += 25;
        }
        if (name == "Heavy") {
            Global.instance.player.GetComponent<Rigidbody2D>().mass += 0.3f;
        }
        if (name == "Deep breath") {
            Global.instance.player.max_breath += 30;
            Global.instance.player.max_power += 20;
        }

        if (name == "Hold breath") {
            Global.instance.player.recover_health += 2;
            Global.instance.player.attack_rate += 70;
        }
        if (name == "Blood") {
            Global.instance.player.extrude_cost += 2;
            Global.instance.player.attack_rate += 50;
        }
        if (name == "Big body") {
            Global.instance.player.transform.localScale *= 1.5f;
        }
        if (name == "Small body") {
            Global.instance.player.transform.localScale *= 0.7f;
        }
        if (name == "Over breath") {
            Global.instance.player.max_power += 40;
        }
    }

}
