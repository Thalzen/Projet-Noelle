using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoCanvas : MonoBehaviour
{

    
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            gameObject.SetActive(false);
        }
    }
}
