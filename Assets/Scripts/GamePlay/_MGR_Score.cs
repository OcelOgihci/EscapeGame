using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _MGR_Score : MonoBehaviour
{
    private static _MGR_Score p_instance = null  ;              //Static instance of GameManager which allows it to be accessed by any other script.
    public static _MGR_Score Instance { get { return p_instance; } }

    public double score = 1000;
    public double getScore() { return score; }
    public void setScore(double _score) { score = _score; }
    
    public double decayPerSecond = 8;
    //Awake is always called before any Start functions
    void Awake()
    {
        // ===>> SingletonMAnager

        //Check if instance already exists
        if (p_instance == null)
            //if not, set instance to this
            p_instance = this;
        //If instance already exists and it's not this:
        else if (p_instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        //Sets this to not be destroyed when reloading scene
        // DontDestroyOnLoad(gameObject);   par nécessaire ici car déja fait  par script __DDOL sur l'objet _EGO_app qui recueille tous les mgr
    }

    void Update()
    {
        score -= decayPerSecond * Time.deltaTime;
    }
}