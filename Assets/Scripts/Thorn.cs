using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : MonoBehaviour {

    float damage = 0;
    public void setdamage(float dam) {
        damage = dam;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Fruit")) {
            Fruit e = collision.collider.GetComponent<Fruit>();
            if(e.get_damage(damage)) { // 每次造成伤害，存活时间减半
                remaintime *= 0.5f;
            } else {
                remaintime = 0;
            }
        }
    }

    float remaintime = 0.4f;
    private void Update() {
        remaintime -= Time.deltaTime;
        if(remaintime < 0) {
            Destroy(gameObject);
            ParticleController.AssignParticle(transform.position, GetComponent<Rigidbody2D>().rotation, "ShieldBroken");
        }
    }

}
