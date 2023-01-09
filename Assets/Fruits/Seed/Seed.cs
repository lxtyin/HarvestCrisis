using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : Fruit {
    public override void be_destroy() {
        var par = ParticleController.AssignParticle(transform.position, 0, "Destroy");
        par.transform.localScale *= size;
        var main = par.main;
        main.startColor = Color.black;
        base.be_destroy();
    }
}
