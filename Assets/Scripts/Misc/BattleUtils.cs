using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUtils : MonoBehaviour
{

    private static BattleUtils instance = null;
    public static BattleUtils Instance
    {
        get { return instance; }
    }



    [SerializeField]
    private Transform battleCenter;
    public Transform BattleCenter
    {
        get { return battleCenter; }
    }

    [SerializeField]
    private GameObject borderSprites;
    public GameObject BorderSprites
    {
        get { return borderSprites; }
    }

    [SerializeField]
    private ParticleSystem explorationParticle;
    public ParticleSystem ExplorationParticle
    {
        get { return explorationParticle; }
    }
    [SerializeField]
    private ParticleSystem battleParticle;
    public ParticleSystem BattleParticle
    {
        get { return battleParticle; }
    }

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
}
