using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _MGR_SoundManager : MonoBehaviour {

    private static _MGR_SoundManager p_instance = null;
    public static _MGR_SoundManager Instance { get { return p_instance; } }

    [System.Serializable]
    public class Son
    {
        public string nom;
        public AudioClip son;
        
    }

    public Son[] sons;

    
    public List<AudioSource> p_listaudioSources;



    public Dictionary<string, AudioClip> p_sons;
    

    // Use this for initialization
    private void Awake()
    {
       if(p_instance == null)
            //if not, set instance to this
            p_instance = this;
        //If instance already exists and it's not this:
        else if (p_instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);


        p_sons = new Dictionary<string, AudioClip>();
        

        foreach(Son _Son in sons)
        {
            p_sons.Add(_Son.nom, _Son.son);
        }

        foreach(AudioSource a in p_listaudioSources)
        {
            p_listaudioSources.Add(_MGR_Ressources.Instance.GetComponent<AudioSource>());
        }
    }
    


    //public void PlaySound(string _nom, Vector3 pos_son)
    //{
        
    //}
}
