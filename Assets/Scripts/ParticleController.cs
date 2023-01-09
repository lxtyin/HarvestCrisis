using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {

    public static ParticleController Instance { get; private set; }

    public List<ParticleSystem> Parti = new List<ParticleSystem>();

    static Dictionary<string, int> Dic = new Dictionary<string, int>();

    private void Awake() {
        Instance = this;
        for(int i = 0; i < Parti.Count; i++) {//自动为每个粒子取名
            Dic[Parti[i].name] = i;
        }
    }

    public static ParticleSystem AssignParticle(Transform t, string parname) {
        
        ParticleSystem par = Instantiate(Instance.Parti[Dic[parname]], t.position, t.rotation);
        par.transform.localScale = t.localScale;
        Instance.StartCoroutine(Following(par, t));
        return par;
    }//按目标分配粒子，粒子将跟随该目标直至其消亡

    public static ParticleSystem AssignParticle(Vector3 pos, float rotation, string parname) {
        ParticleSystem par = Instantiate(Instance.Parti[Dic[parname]], pos, Quaternion.Euler(new Vector3(0, 0, rotation)));
        return par;
    }//给定位置和方向，生成一次

    static IEnumerator Following(ParticleSystem par, Transform tar) {
        Transform tpar = par.transform;
        while(tar != null) {
            if(tpar == null) yield break;
            tpar.position = tar.position;
            tpar.rotation = tar.rotation;
            yield return new WaitForSeconds(0.02f);
        }
        var emi = par.emission;
        emi.rateOverTime = 0;
        Destroy(par.gameObject, 1f);
    }


}
