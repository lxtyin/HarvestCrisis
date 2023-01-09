using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour {

    public static Global instance;
    public static bool pause;

    private void Awake() {
        instance = this;
    }


    public PlayerController player;
    public Canvas canvas;


}
