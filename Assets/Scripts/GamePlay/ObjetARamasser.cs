using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetARamasser : MonoBehaviour
{
    public virtual void ActionObjetRamasse() { Destroy(gameObject); }
    public void ActionObjetRamasse(float _delai) { Invoke("ActionObjetRamasse", _delai); }
    public int IDRessourceNecessaire = -1;
    public virtual bool CheckRessource()
    {
        if (_MGR_Ressources.Instance.GetCurrentResource())
            return _MGR_Ressources.Instance.GetCurrentResource().ID == IDRessourceNecessaire;
        return false;
    }
}
