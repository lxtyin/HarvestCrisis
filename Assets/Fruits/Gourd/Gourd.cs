using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gourd : Fruit {

    public int choose_num;
    public override void be_destroy() {
        var par = ParticleController.AssignParticle(transform.position, 0, "Gourd");

        GameController.choose_buff(choose_num);

        base.be_destroy();
    }

}
