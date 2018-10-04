using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INTERACTION_EnsembleCapsule : INTERACTION_CLICK_AND_PICK
{
    static int nbSpheresSelectionnees = 0;
    private const int NB_SPHERES_A_SELECTIONNER = 3;
    
    // cette fonction est définie ici à minima et peut être suffisante
    // ou peu nécessiter d'être à nouveau redéfinie ("overrided") dans la classe fille
    // possibilité de compléter ce code en commencant la méthodes redéfinie par base.Start(); ....
    /*override public void Start()
    {
        base.Start();
        boutton = BOUTTONS.LEFT;
        action_boutton = ACTIONS_BOUTTON.APPUI;
        action_detectee = false;

        layer_mask = LayerMask.GetMask("Pickable");   // ou GetMask("L1", "L2", ...); si plusieurs layers
        max_dist=Mathf.Infinity;
    }*/


    override public void Object_Picked()
    {
        nbSpheresSelectionnees++;
        if (nbSpheresSelectionnees >= NB_SPHERES_A_SELECTIONNER)
            Declencher_Etape_Suivante_Du_Scenario();

    }


    override public bool test_Mouse_Picking()
    {
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, max_dist, layer_mask))
        {
            if (hitInfo.transform.parent != null)           // ce script s'execute sur un EGO , parents des objets cibles
                if (hitInfo.transform.parent.gameObject == this.gameObject)
                {
                    //CommonDevTools.ERROR("nouvelle capsule " + nbSpheresSelectionnees+" - " + hitInfo.transform.gameObject.name, hitInfo.transform.gameObject);
                    hitInfo.transform.gameObject.SetActive(false);
                    return true;
                }
        }
        return false;

    }



    // Update is called once per frame
    override public void Update()
    {

        if (gameObject.layer == 9)
        {
            switch (action_boutton)
            {
                case ACTIONS_BOUTTON.APPUI:
                    action_detectee = Input.GetMouseButtonDown((int)boutton);
                    break;
                case ACTIONS_BOUTTON.RELACHE:
                    action_detectee = Input.GetMouseButtonUp((int)boutton);
                    break;
                case ACTIONS_BOUTTON.MAINTENU:
                    action_detectee = Input.GetMouseButton((int)boutton);
                    break;
            }

            if (action_detectee)
            {
                //print("Click apres ... " + nbSpheresSelectionnees);
                if (test_Mouse_Picking())
                    Object_Picked();
                else
                    CommonDevTools.DEBUG(descriptionAction); // en attendant meilleure UI....
            }
        }
    }
}

