using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Policy;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Collecteur : MonoBehaviour
{

    ObjetARamasser o;
    public string url = "images/crossSelect";
    private WWW www;
    //IEnumerator Start()
    //{
    //    // Start a download of the given URL
    //    using (www = new WWW(url))
    //    {
    //        // Wait for download to complete
    //        yield return www;

    //        // assign texture
    //        Renderer rend = GetComponent<Renderer>();
    //        rend.material.mainTexture = www.texture;
    //    }
    //}

    void Update()
    {
        if (o != null && Input.GetMouseButtonDown(0) && _MGR_Ressources.Inventory.Count < 10)
            o.ActionObjetRamasse();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
                other.gameObject.layer = 9;
                if (o == null)
                    o = other.gameObject.GetComponent<ObjetARamasser>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            other.gameObject.layer = 8;
        }

        o = null;

    }


}
