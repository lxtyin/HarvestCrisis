using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour {

    public float max_health = 100;
    [Header("�͹۴�С")]
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
    /// �ܵ��˺�
    /// </summary>
    /// <param name="dam">�˺�</param>
    /// <returns>�Ƿ�����</returns>
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
    /// ���ݻ�
    /// </summary>
    public virtual void be_destroy() {
        GameController.add_destroy();
        Destroy(gameObject);
    }

}
