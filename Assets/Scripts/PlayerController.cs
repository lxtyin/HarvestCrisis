using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float max_health;
    public float max_breath;
    public float max_power;
    [Header("ÿ��������")]
    public float accum_rate;
    [Header("ÿ�뼷ѹ����")]
    public float extrude_cost;
    [Header("��ɱ�ظ�")]
    public float recover_health;
    [Header("�ƻ���")]
    public float attack_rate;
    [Header("Breath�ָ�ʱ��")]
    public float breath_cd; // ��������������ѹ��ô��ʱ�䣬���ָ�ȫ��breath
    [Header("�׳��Ĵ�")]
    public GameObject thorn;

    public float health, breath;
    float power;
    float breath_time;
    float hit_attack;

    Rigidbody2D mybody;
    Animator myanimator;
    SpriteRenderer mysprite;
    bool is_crimp = false; // �Ƿ�����
    bool is_power = false; // �Ƿ�������
    
    private void Awake() {
        mybody = GetComponent<Rigidbody2D>();
        myanimator = GetComponent<Animator>();
        mysprite = GetComponent<SpriteRenderer>();
        health = max_health;
        breath = max_breath;
        StartCoroutine("extrude_update");
    }

    public void modify_health(float d) {
        health = Mathf.Clamp(health + d, 0, max_health);
        if(d <= -5) {
            red_fresh();
        }
        if (health <= 0) {
            Global.pause = true;
            UIController.instance.show_death();
        }
    }

    public void modify_breath(float d) {
        breath += d;
        if (breath > max_breath)
            breath = max_breath;
        if (breath < 0) {
            modify_health(breath);
            breath = 0;
        }
    }

    void red_fresh() {
        mysprite.color = Color.red;
        Invoke("whiteback", 0.2f);
    }
    void whiteback() { mysprite.color = Color.white;  }

    void dash(Vector2 dir, float p) {
        hit_attack = attack_rate * Mathf.Max(0.5f, p / 100); // ��ʱatk�����������
        mybody.velocity = dir.normalized * p;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (Global.pause)
            return;
        if(collision.collider.CompareTag("Fruit")) {
            Fruit e = collision.collider.GetComponent<Fruit>();
            float velocity_rate = Mathf.Log(1 + collision.relativeVelocity.magnitude, 2.7f); // �ٶȻ����������˺�����
            if (e.get_damage(velocity_rate * hit_attack)) {
                modify_health(recover_health);
            }
        }
        if(mybody.velocity.magnitude < 10 && !is_power) {
            myanimator.SetBool("crimp", false);
            hit_attack = 0;
            is_crimp = false;
        }
    }

    /// <summary>
    /// �ָ�ȫ��breath
    /// </summary>
    void recover_breath() {
        if(Buff.level["Hold breath"] <= 0) {
            breath = max_breath;
            ParticleController.AssignParticle(transform.position, 0, "Breath");
        }
    }

    /// <summary>
    /// �������update
    /// </summary>
    void accumulate_update() {
        if(Global.pause) {
            Arrow.hide();
            is_power = false;
            return;
        }
        Vector2 mouse_pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)) {
            power = max_power * 0.1f;
            mybody.rotation = 0;
/*            mybody.velocity *= 0.1f;*/
        }
        if (Input.GetMouseButtonUp(0)) {
            dash(mouse_pos - mybody.position, power);
            Arrow.hide();
        }

        is_power = Input.GetMouseButton(0);
        if (is_power) {
            myanimator.SetBool("crimp", true);
            is_crimp = true;
            if (power < max_power) {
                power += Time.deltaTime * accum_rate;
            }
            breath_time = breath_cd;
            Arrow.update_arrow(mybody.position, mouse_pos, power / max_power);
        }
        if(is_crimp) {
            if(mybody.velocity.x > 10) {
                mybody.rotation -= 1800 * Time.deltaTime;
            } else {
                mybody.rotation += 1800 * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// ����update
    /// </summary>
    void breath_update() {
        if (breath_time < 0) {
            if (breath < max_breath) {
                recover_breath();
            }
        } else {
            breath_time -= Time.deltaTime;
        }
    }

    float thorncd = 0;
    float repelcd = 0;
    float overbreathcd = 0;
    /// <summary>
    /// buff��ظ���
    /// </summary>
    void buff_update() {
        // ����ʱ���� lvӰ������ �˺�
        int lv = Buff.level["Thorn"];
        if (lv > 0 && is_crimp) {
            thorncd -= Time.deltaTime;
            if(thorncd < 0) {
                thorncd = 1;
                for(int i = 0;i < lv * 2; i++) {
                    float rot = Random.Range(0, 360);
                    var g = Instantiate(thorn, mybody.position, Quaternion.Euler(0, 0, rot + 90));
                    g.transform.localScale = transform.localScale;
                    g.GetComponent<Thorn>().setdamage(attack_rate * lv);
                    var b = g.GetComponent<Rigidbody2D>();
                    b.velocity = Math.getVector(rot) * 100;
                }
            }
        } else {
            thorncd = 1;
        }

        // ����ʱ���� lvӰ�췶Χ������
        lv = Buff.level["Repel"];
        if (lv > 0 && is_crimp) {
            repelcd -= Time.deltaTime;
            if(repelcd < 0) {
                repelcd = 1.5f;

                float radius = (3 + lv * 1.0f / 2) * transform.localScale.x;

                var par = ParticleController.AssignParticle(mybody.position, 0, "Exploation");
                // Exploation �����С�����3
                par.transform.localScale = Vector3.one * radius / 3;

                var rhits = Physics2D.CircleCastAll(mybody.position, radius, Vector2.one, 0, ~0);
                foreach (RaycastHit2D rhit in rhits) {
                    var f = rhit.collider.GetComponent<Fruit>();
                    if(f) {
                        f.addforce((f.transform.position - transform.position).normalized * 300 * lv);
                        f.get_damage(attack_rate * lv);
                    }
                }
            }
        } else {
            repelcd = 1.5f;
        }

        // ����breath��������
        lv = Buff.level["Over breath"];
        if (lv > 0 && is_power) {
            overbreathcd -= Time.deltaTime;
            if(overbreathcd < 0) {
                overbreathcd = 0.5f;
                if(power < max_power) {
                    float d = Mathf.Min(7 * lv, breath);
                    power = Mathf.Min(max_power, power + d * 2);
                    modify_breath(-d);
                }
            }
        } else {
            overbreathcd = 0.5f;
        }
    }

    void Update() {
        UIController.instance.update_hp_bp(health, max_health, breath, max_breath);
        accumulate_update();
        if (Global.pause) {
            return;
        }
        breath_update();
        buff_update();

        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("����: " + attack_rate.ToString());
            Debug.Log("����: " + mybody.mass.ToString());
            Debug.Log("�����ٶ�: " + accum_rate.ToString());
            Debug.Log("��������: " + max_power.ToString());
        }
    }


    /// <summary>
    /// ��ʱ��ѹ���
    /// </summary>
    /// <returns></returns>
    IEnumerator extrude_update() {
        RaycastHit2D[] raycastHits = new RaycastHit2D[16];
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(~LayerMask.GetMask("Fruit"));
        while (true) {
            yield return new WaitForSeconds(1);
            if (Global.pause)
                continue;
            int w = mybody.Cast(Vector2.up, contactFilter, raycastHits, 0.1f);
            int s = mybody.Cast(Vector2.down, contactFilter, raycastHits, 0.1f);
            int a = mybody.Cast(Vector2.left, contactFilter, raycastHits, 0.1f);
            int d = mybody.Cast(Vector2.right, contactFilter, raycastHits, 0.1f);
            if (w != 0 && s != 0 && a != 0 && d != 0) {
                breath_time = breath_cd;
                modify_breath(-extrude_cost);
            }
        }
    }
}
