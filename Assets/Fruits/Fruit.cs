using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour {

    public float max_health = 100;
    [Header("客观大小")]
    public float size = 1;

    protected float health;
    protected Rigidbody2D mybody;

    private void Awake() {
        mybody = GetComponent<Rigidbody2D>();
        health = max_health;
    }

    public void addforce(Vector2 v) {
        mybody.AddForce(v);
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="dam">伤害</param>
    /// <returns>是否死亡</returns>
    public virtual bool get_damage(float dam) {
        if(dam > 1000) {
            Debug.Log(dam);
        }
        health -= dam;
        if (health < 0) {
            be_destroy();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 被摧毁
    /// </summary>
    public virtual void be_destroy() {
        GameController.add_destroy();
        Destroy(gameObject);
    }

}
