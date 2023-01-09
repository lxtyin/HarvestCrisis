using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Fruit {

    Vector2 bomb_pos;
    public override void be_destroy() {
        bomb_pos = mybody.position;
        base.be_destroy(); // 先把自己删了再炸 防乱递归
        bomb();
        Invoke("bomb", Random.Range(0.1f, 0.5f));
    }

    void bomb() {
        var par = ParticleController.AssignParticle(bomb_pos, 0, "Exploation");
        par.transform.localScale *= 5;
        var main = par.main;
        main.startColor = new Color(1, 0.7f, 0.3f);

        var rhits = Physics2D.CircleCastAll(bomb_pos, 12, Vector2.one, 0, ~0);
        foreach (RaycastHit2D rhit in rhits) {
            var g = rhit.collider;
            var r = g.GetComponent<Rigidbody2D>();
            if (r) {
                r.AddForce((r.position - bomb_pos).normalized * 2000); // 推力
                if ((r.position - bomb_pos).magnitude < 10) { // 内环伤害
                    var f = g.GetComponent<Fruit>();
                    if (f) {
                        f.get_damage(1000);
                    }
                    var p = g.GetComponent<PlayerController>();
                    if (p) {
                        p.modify_health(-30);
                    }
                }
            }
        }
    }

}
