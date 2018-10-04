using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjetsRessource : ObjetARamasser
{
    public Sprite image;
    public bool picked = false;

    [Multiline]
    public String text;

    public int ID = -1;
    private _MGR_Ressources inventaire = _MGR_Ressources.Instance;
    
    public override void ActionObjetRamasse()
    {
        if (!picked)
        {
            inventaire.AddObject(this);
            _UI_Inventory.Instance.updateImages();
            //gameObject.layer = 1;
            picked = true;
            gameObject.SetActive(false);
            enabled = false;
        }


        //Destroy(gameObject);
    }
}
