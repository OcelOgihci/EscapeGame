using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersoController : MonoBehaviour
{

    private float speed = 10.0f;
    
    private int CurrentIndex = 0;

    private bool pause = false;
    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        float translation = Input.GetAxis("Vertical") * speed;
        float straffe = Input.GetAxis("Horizontal") * speed;
        translation *= Time.deltaTime;
        straffe *= Time.deltaTime;
        transform.Translate(straffe, 0, translation);

        if (Input.GetKeyDown("escape"))
            pause = !pause;
        if (Input.GetKeyDown(KeyCode.LeftShift))
            speed += 10;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            speed -= 10;
        if (Input.GetKeyDown(KeyCode.Space))
            this.GetComponent<Rigidbody>().AddForce(0, 10, 0);
        if (Input.GetKeyDown(KeyCode.F1))
            _UI_Inventory.Instance.toggleDisplay();
        if(Input.GetKeyDown(KeyCode.Alpha1))
            _MGR_Ressources.Instance.ChangeCurrentResource(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            _MGR_Ressources.Instance.ChangeCurrentResource(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            _MGR_Ressources.Instance.ChangeCurrentResource(2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            _MGR_Ressources.Instance.ChangeCurrentResource(3);
        if(Input.GetAxis("Mouse ScrollWheel") < 0f && CurrentIndex < _MGR_Ressources.Inventory.Count-1)
            _MGR_Ressources.Instance.ChangeCurrentResource(++CurrentIndex);
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && CurrentIndex > 0)
            _MGR_Ressources.Instance.ChangeCurrentResource(--CurrentIndex);

        Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;

        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;


    }

    void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    void OnCollisionStay(Collision collision)
    {
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}




