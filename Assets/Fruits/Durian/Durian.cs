
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Durian : Fruit {

    public float damage;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            Vector2 v = collision.relativeVelocity;
            if (v.magnitude > 10) {
                var p = collision.collider.GetComponent<PlayerController>();
                p.modify_health(-damage);
                ParticleController.AssignParticle(collision.GetContact(0).point, Math.getRotation(-v), "Blood");
            }
        }
    }

    public override void be_destroy() {
        var par = ParticleController.AssignParticle(transform.position, 0, "DestroyRemain");
        par.transform.localScale *= size;
        var main = par.main;
        main.startColor = new Color(1, 0.7f, 0.4f);

        base.be_destroy();
    }

}
