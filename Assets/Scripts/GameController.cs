using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HarvestElement {
    public GameObject g;
    public int v;
}

[System.Serializable]
public class Harvest {
    public int next_lim; // 进入下一阶段所需击杀数
    public List<HarvestElement> ls;
}

public class GameController : MonoBehaviour {

    // setting --- 
    public float map_l, map_r;
    public float map_top;

    public Transform fruitbox;
    public List<Harvest> stages = new List<Harvest>();
    // ------- 

    public static GameController instance;

    public static int level;
    public static int required_num; // 下阶段所需击杀数
    public static int total_num;    // 总击杀数

    static bool is_looping = false;
    private void Awake() {
        instance = this;
        level = 0;
        required_num = 1;
        total_num = 0;
        is_looping = true;
        UIController.instance.update_counter_text(total_num, 0, required_num);
    }

    public static void add_destroy() {
        total_num++;
        if(is_looping) {
            required_num--;
            if (required_num == 0) {
                next_level();
            }
        }
        UIController.instance.update_counter_text(total_num, level, required_num);
    }

    static void next_level() {
        UIController.instance.show_harvest_hint();

        level++;
        if(level == instance.stages.Count) {
            UIController.instance.show_end();
            is_looping = false;
            return;
        }

        required_num = instance.stages[level].next_lim;
        foreach (var p in instance.stages[level].ls) {
            for (int i = 0; i < p.v; i++) {
                var g = Instantiate(p.g, new Vector2(Random.Range(instance.map_l, instance.map_r),
                    instance.map_top + Random.Range(-10, 10)),
                    Quaternion.identity);
                g.transform.SetParent(instance.fruitbox);
            }
        }
    }

    /// <summary>
    /// 进入buff选择
    /// </summary>
    /// <param name="num"></param>
    public static void choose_buff(int num) {
        var ls = Buff.select_buff(num);
        UIController.instance.show_choosebuff(ls);
    }

}
