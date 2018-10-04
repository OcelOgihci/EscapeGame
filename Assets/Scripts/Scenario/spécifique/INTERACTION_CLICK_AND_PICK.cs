using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INTERACTION_CLICK_AND_PICK : Action_Scenario_Etape
{

    protected enum BOUTTONS { LEFT, RIGHT, MIDDLE }
    protected enum ACTIONS_BOUTTON { APPUI, RELACHE, MAINTENU }
    protected BOUTTONS boutton;
    protected ACTIONS_BOUTTON action_boutton;
    protected bool action_detectee;
    protected Color32 couleurBase;
    protected float max_dist;   // max distance of ray cast
    protected int layer_mask;   // seuls les objets de ces layers seront testés pour l'intersection au raycast

    // cette fonction est définie ici à minima et peut être suffisante
    // ou peu nécessiter d'être à nouveau redéfinie ("overrided") dans la classe fille
    // possibilité de compléter ce code en commencant la méthodes redéfinie par base.Start(); ....
    override public void Start()
    {
        base.Start();
        if(gameObject.GetComponent<Renderer>().material)
            couleurBase = gameObject.GetComponent<Renderer>().material.color;
        boutton = BOUTTONS.LEFT;
        action_boutton = ACTIONS_BOUTTON.APPUI;
        action_detectee = false;

        layer_mask = LayerMask.GetMask("inEpuisette");   // ou GetMask("L1", "L2", ...); si plusieurs layers
        max_dist = Mathf.Infinity;


    }


    virtual public void Object_Picked()
    {
            Declencher_Etape_Suivante_Du_Scenario();
    }


    virtual public bool test_Mouse_Picking()
    {
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, max_dist, layer_mask))
        {
            //Debug.Log("Hit " + hitInfo.transform.gameObject.name);
            if (hitInfo.transform.gameObject == this.gameObject)
                return true;
        }
        return false;

    }


    // Update is called once per frame
    override public void Update()
    {
        if (gameObject.layer == 9)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color32(0, 0, 255, 0);

            Debug.Log("Click on 9");
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
                if (test_Mouse_Picking())
                    Object_Picked();
                else
                    CommonDevTools.DEBUG(descriptionAction); // en attendant meilleure UI....
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = couleurBase;

        }
    }
}

