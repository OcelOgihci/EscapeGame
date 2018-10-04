using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INTERACTION_CAPSULES : INTERACTION_CLICK_AND_PICK
{

    public override void Object_Picked()
    {
        if (_MGR_Ressources.Instance.GetCurrentResource().ID == 2)
        {
            gameObject.GetComponentInParent<INTERACTION_EnsembleCapsule>().SendMessage("Object_Picked");
            Destroy(gameObject);
        }
    }

    public override void Update()
    {
        if (gameObject.layer == 9)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color32(0,0,255,0);
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
