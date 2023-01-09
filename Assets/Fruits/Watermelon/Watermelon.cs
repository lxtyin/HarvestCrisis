using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watermelon : Fruit {

    public GameObject seed;
    public int seed_num;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            Vector2 v = collision.relativeVelocity;
            if (v.magnitude > 10) {
                var par = ParticleController.AssignParticle(collision.GetContact(0).point, Math.getRotation(v), "ShieldBroken");
                var main = par.main;
                main.startColor = Color.green;
                par.transform.localScale *= size;
            }
        }
    }

    public override void be_destroy() {
        var par = ParticleController.AssignParticle(transform.position, 0, "DestroyRemain");
        par.transform.localScale *= size;
        var main = par.main;
        main.startColor = new Color(1, 0.4f, 0.3f);


        for(int i = 0;i < seed_num; i++) {
            var g = Instantiate(seed, mybody.position, Quaternion.identity);
            var body = g.GetComponent<Rigidbody2D>();
            body.velocity = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));
        }

        base.be_destroy();
    }

}
