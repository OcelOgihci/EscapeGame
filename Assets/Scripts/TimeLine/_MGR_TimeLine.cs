using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _MGR_TimeLine : MonoBehaviour
{

    private static _MGR_TimeLine p_instance = null; //Static instance of GameManager which allows it to be accessed by any other script.
    public static _MGR_TimeLine Instance { get { return p_instance; } } // READ ONLY

    public const float DURE_MAX_PAR_DEFAUT = 60; // temps maximum d'une partie (sans pause) par defaut = 1 minute
    public float dureeMax { get; set; } // temps maximum d'une partie (sans pause)
    private float p_debutApp;
    public float chrono { get; private set; } // chrono partie
    public float dureeJeu { get; private set; } // chrono partie
    public float dureeApp { get { return Time.time - p_debutApp; } } // temps exécution application 

    public bool ChronoDemarre { get; private set; }
    public bool ChronoEnPause { get; private set; }

    private uint p_nb_TL_Events;
    List<Interface_TL_Events> p_Liste_TL_Events;

    // à faire : 
    // stocker tous les évenement dans une timeline d'évements à déclencher ou arrêter .... 
    // prétraitement plus difficile mais ensuite plus rapide ;-)
    // == construire une liste d'évenements à declencher à partir de l'analyse de p_Liste_TL_Events
    // == ensuite à chaque frame teste s'il est temps de réaliser une opération sur l'évenement suivant
    // OU
    // test à chaque frame des évenements à déclencher ou arrêter ... : plus simple et plus lent !! 
    // == parcourir p_Liste_TL_Events pour savoir si l'evenement doit être déclenché ou arrêté 


    void Awake()
    { // ===>> Singleton Manager

        //Check if instance already exists
        if (p_instance == null)
            //if not, set instance to this
            p_instance = this;
        //If instance already exists and it's not this:
        else if (p_instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        //Sets this to not be destroyed when reloading scene
        // DontDestroyOnLoad(gameObject); par nécessaire ici car déja fait par script __DDOL sur l'objet _EGO_app qui recueille tous les mgr


        p_debutApp = Time.time;
        dureeMax = DURE_MAX_PAR_DEFAUT;
        p_Liste_TL_Events = null;
        p_nb_TL_Events = 0;
        ChronoDemarre = false;
    }


    /* à partir des objets passés en paramètres, rescencer tous les scripts représentants des évenements de la TL
    il s'agit des scripts (ou instances de classes) réalisant l'interface Interface_TL_Events , quelque soit la classe
    pour cela le code s'appuie essentiellement sur le méthode GetComponents<Interface_TL_Events>()
    */
    public void Configurer(Event_TL[] __Events_TL)
    {
        // ====> récupérer et vérifier les évenements passés en paramètres 
        if (p_Liste_TL_Events != null) p_Liste_TL_Events.Clear(); // vider la liste (si 2ème ou suivantes exécutions....
        else p_Liste_TL_Events = new List<Interface_TL_Events>(); // 1ere exécution : instantiation
        p_nb_TL_Events = 0;

        foreach (Event_TL _TL_event in __Events_TL)
        {
            // un objet valide ?
            if (_TL_event.GO == null)
            {
                CommonDevTools.WARNING("objet event non défini : NULL");
                break;
            }
            // activer GO si inactif ? 
            if (_TL_event.GO.active == false)
                if (_TL_event.activer_GO_si_inactif)
                    _TL_event.GO.SetActive(true);

            // recherche des scripts évenements = components impémentant l'interface Interface_TL_Events 
            Interface_TL_Events[] __tabtle = _TL_event.GO.GetComponents<Interface_TL_Events>();
            for (int i = 0; i < __tabtle.Length; i++)
            {
                if (__tabtle[i].getDuration_TL_Event() <= 0f)
                {
                    CommonDevTools.WARNING("object " + _TL_event.GO.name + " event n° " + i + " durée invalide : IGNORE !");
                    continue; // on passe à l'évenement suivant
                }
                if (__tabtle[i].getStartTime_TL_Event() > dureeMax)
                {
                    CommonDevTools.WARNING("object " + _TL_event.GO.name + " event n° " + i + " Debut apres fin partie (>durée max) : IGNORE !");
                    continue; // on passe à l'évenement suivant
                }
                // evenement OK : ajouté 
                print("object " + _TL_event.GO.name + " event n° " + i + " ajouté !");

                p_Liste_TL_Events.Add(__tabtle[i]);
                p_nb_TL_Events++;
            }
        }


        // Methode 1
        ConstruireTimeLine();
    }

    public void Update()
    {
        if (ChronoDemarre && !ChronoEnPause)
        {
            chrono += Time.deltaTime;
            TestStopChrono();
            Piloter_Event_TL(); // Methode 1 & 2
        }
    }

    //*******************************************************************************************************//
    // à faire : 
    // Methode 1 : 
    // stocker tous les évenement dans une timeline (liste) d'évements à déclencher ou arrêter .... 
    // prétraitement plus difficile mais ensuite plus rapide ;-)
    // == construire une liste d'évenements à delcncher à partir de l'analyse de p_Liste_TL_Events
    // == ensuite à chaque frame teste s'il est temps de réaliser une opération sur l'évenement suivant
    // à prilégier car la TimeLine pourraît être un outils utile pour d'autres aspects du projet ... 
    // OU
    // Methode 2 : 
    // test à chaque frame des évenements à déclencher ou arrêter ... : plus simple et plus lent !! 
    // == parcourir p_Liste_TL_Events pour savoir si l'evenement doit être déclenché ou arrêté 
    // lent et peu généralisable mais peut répondre au pb immédiat 
    //*******************************************************************************************************//
    // Methode 1 & 2 
    enum TypeCdeEvent { START, STOP }
    struct struct_evt_TL
    {
        public float time_evt_TL;
        public TypeCdeEvent cdeEvt_TL;
        public Interface_TL_Events evt_TL;
        // constructeur
        public struct_evt_TL(float t, TypeCdeEvent o, Interface_TL_Events e) { time_evt_TL = t; cdeEvt_TL = o; evt_TL = e; }
    }

    // ci dessous sera utilisé pour trier la liste avec la méthode sort()
    static int Compare_Struct_Evt_TL(struct_evt_TL x, struct_evt_TL y)
    {
        return ((struct_evt_TL)x).time_evt_TL.CompareTo(((struct_evt_TL)y).time_evt_TL);
    }

    private List<struct_evt_TL> TimeLine; // la timeline = planification de toutes les commandes à exécuter pendant le temps du jeu, soit démarer soit arrêter n évenement
    private int indexTimeLine; // le prochain évenement à traiter 
    private int maxIndexTimeLine; // le dernier évenement à traiter 
    private struct_evt_TL prochaine_cde_event;

    /* sera appelée à chaque frame depuis Update()
    y a t'il un évenement de la timeline à déclencher en fonction de la valeur actuelle du chrono ?
    */
    private void Piloter_Event_TL()
    {
        if (indexTimeLine >= maxIndexTimeLine) return; // partie non terminée (chrono en cours) mais plus d'événement à déclencher 
                                                       //print("chrono " + chrono + " prochain event " + prochaine_cde_event.time_evt_TL);
                                                       //print("'''''''''''''''''''''''''''''''''''''''indexTimeLine " + indexTimeLine + " maxIndexTimeLine " + maxIndexTimeLine);
        //if (maxIndexTimeLine >= 0) print("chrono " + chrono + " nbEvents : " + maxIndexTimeLine);
        while (chrono > prochaine_cde_event.time_evt_TL)
        {
            if (prochaine_cde_event.cdeEvt_TL == TypeCdeEvent.START)
                prochaine_cde_event.evt_TL.start_TL_Event();
            else
                prochaine_cde_event.evt_TL.stop_TL_Event();

            indexTimeLine++;
            if (indexTimeLine >= maxIndexTimeLine) break;
            prochaine_cde_event = TimeLine[indexTimeLine];
        }
    }
    private void ConstruireTimeLine()
    {
        float __period = 0;

        if (TimeLine != null) TimeLine.Clear(); // vider la liste (si 2ème ou suivantes exécutions....
        else TimeLine = new List<struct_evt_TL>(); // 1ere exécution : instantiation
        p_nb_TL_Events = 0;

        foreach (Interface_TL_Events __evTL in p_Liste_TL_Events) // pour chaque évenement TL 
        {
            if (!(__evTL.isPerdiodic_TL_Event(out __period)))
            {
                // ajouter dans la timeline les ordres de début de l'évenement 
                TimeLine.Add(new struct_evt_TL(__evTL.getStartTime_TL_Event(), TypeCdeEvent.START, __evTL));
                // ajouter dans la timeline les ordres de fin de l'évenement 
                TimeLine.Add(new struct_evt_TL(__evTL.getStopTime_TL_Event(), TypeCdeEvent.STOP, __evTL));
            }
            else
            {
                for (int i = 0; (i * __period) < dureeMax; i++)
                {
                    // ajouter dans la timeline les ordres de début de l'évenement 
                    TimeLine.Add(new struct_evt_TL(__evTL.getStartTime_TL_Event() + (i * __period), TypeCdeEvent.START, __evTL));
                    // ajouter dans la timeline les ordres de fin de l'évenement 
                    TimeLine.Add(new struct_evt_TL(__evTL.getStopTime_TL_Event() + (i * __period) + __evTL.getDuration_TL_Event(), TypeCdeEvent.STOP, __evTL));
                }
            }
        }
        maxIndexTimeLine = TimeLine.Count;

        // TRIER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        TimeLine.Sort(Compare_Struct_Evt_TL);
    }
    //*******************************************************************************************************//



    public void StartChrono()
    {
        chrono = 0;
        ChronoDemarre = true;
        foreach (Interface_TL_Events evTL in p_Liste_TL_Events)
            evTL.TL_ChronoDemarre();
        indexTimeLine = 0;
        prochaine_cde_event = TimeLine[0]; // à décommenter une fois la structure timeline créée
    }
    private void StartChrono(float __delay) { Invoke("StartChrono", __delay); }
    private void ReStartChrono() { dureeJeu += chrono; StartChrono(); }
    private void PauseChrono()
    {
        ChronoEnPause = true;
        foreach (Interface_TL_Events evTL in p_Liste_TL_Events)
        {
            if (evTL.isPausable_TL_Event()) evTL.pause_TL_Event();
            evTL.TL_ChronoEnPause();
        }
    }
    private void ReprendreChrono()
    {
        ChronoEnPause = false;
        foreach (Interface_TL_Events evTL in p_Liste_TL_Events)
            evTL.TL_ChronoReprise();
    }

    private void TestStopChrono()
    {
        if (chrono > dureeMax)
        {
            FinDePartie();
            _MGR_SceneManager.Instance.FinDePartie(_MGR_SceneManager.FIN_DE_PARTIE.PERDU_CHRONO);
        }
    }
    public void FinDePartie()
    {
        ChronoDemarre = false;
        dureeJeu += chrono;
        foreach (Interface_TL_Events evTL in p_Liste_TL_Events)
            evTL.TL_ChronoArrete();
    }
}