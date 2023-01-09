using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coconut : Fruit {

    public SpriteRenderer crack;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            Vector2 v = collision.relativeVelocity;
            if (v.magnitude > 20) {
                var par = ParticleController.AssignParticle(collision.GetContact(0).point, Math.getRotation(v), "ShieldBroken");
                var main = par.main;
                main.startColor = new Color(0.75f, 0.5f, 0.2f);
                par.transform.localScale *= size;
            }
        }
    }

    public override void be_destroy() {
        var par = ParticleController.AssignParticle(transform.position, 0, "Destroy");
        par.transform.localScale *= size;
        var main = par.main;
        main.startColor = new Color(0.75f, 0.5f, 0.2f);

        par = ParticleController.AssignParticle(transform.position, 0, "Stain");
        par.transform.localScale *= size / 3;

        base.be_destroy();
    }

    private void Update() {
        crack.color = new Color(1, 1, 1, 1 - health / max_health);
    }
}
