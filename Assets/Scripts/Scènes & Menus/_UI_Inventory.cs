using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class _UI_Inventory : MonoBehaviour
{
    private static _UI_Inventory p_instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    public static _UI_Inventory Instance { get { return p_instance; } }
    private Canvas canvas;
    private Items[] items;

    public Items[] GetItems() { return items; }
    public Items CurrentItem;
    void Start()
    {
        updateItems();
        if (items.Length != 0)
            CurrentItem = items[0];
        canvas = gameObject.GetComponentInParent<Canvas>();
    }

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


    public void updateImages()
    {
        updateItems();
        HorizontalLayoutGroup[] slots = gameObject.GetComponentsInChildren<HorizontalLayoutGroup>();
        for (int i = 0; i < slots.Length; ++i)
            items[i] = slots[i].gameObject.GetComponentInChildren<Items>();
        for (int i = 0; i < _MGR_Ressources.Inventory.Count; ++i)
        {
            Sprite image = _MGR_Ressources.Inventory.ToArray()[i].image;
            
            
            Text text = slots[i].GetComponentInChildren<Text>();
            text.text = _MGR_Ressources.Inventory.ToArray()[i].text;
            items[i].GetComponentsInChildren<Image>()[1].sprite = image;
            items[i].GetComponentsInChildren<Image>()[0].color = CurrentItem == items[i] ? Color.yellow : Color.white;
            slots[i].GetComponentInChildren<Text>().enabled = CurrentItem == slots[i].GetComponentInChildren<Items>();
        }
    }

    public void toggleDisplay()
    {
        canvas.enabled = !canvas.enabled;
        updateImages();
    }

    public void ChangeCurrentItem(int index)
    {
        CurrentItem = items[index];
        updateImages();
    }

    public void AddSlot()
    {
        
        GameObject slot = Resources.Load<GameObject>("Prefab/Slot");
        Instantiate(slot, transform);
        updateItems();
        if (CurrentItem == null)
            ChangeCurrentItem(0);

    }

    private void updateItems()
    {
        items = gameObject.GetComponentsInChildren<Items>();
    }
}
