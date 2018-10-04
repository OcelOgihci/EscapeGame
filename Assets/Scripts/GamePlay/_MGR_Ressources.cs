using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _MGR_Ressources : MonoBehaviour {

    private static _MGR_Ressources p_instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    public static _MGR_Ressources Instance { get { return p_instance; } }


    private static Stack<ObjetsRessource> p_inventory = new Stack<ObjetsRessource>();
    public static Stack<ObjetsRessource> Inventory
    {
        get { return p_inventory; }
    }

    private ObjetsRessource CurrentResource; 

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
        // DontDestroyOnLoad(gameObject);   par nécessaire ici car déja fait par script __DDOL sur l'objet _EGO_app qui recueille tous les mgr
       
    }

    public void AddObject(ObjetsRessource obj)
    {
        if(p_inventory.Count == _UI_Inventory.Instance.GetItems().Length)
            _UI_Inventory.Instance.AddSlot();
        p_inventory.Push(obj);
        CurrentResource = p_inventory.Peek();
    }

    public ObjetsRessource GetObject(int i)
    {
        return p_inventory.ToArray()[i];
    }

    public ObjetsRessource GetCurrentResource()
    {
        return CurrentResource;
    }

    public void ChangeCurrentResource(int index)
    {
        CurrentResource = Inventory.ToArray()[index];
        _UI_Inventory.Instance.ChangeCurrentItem(index);
    }


}
