using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClick : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if(hit.transform.tag == "Card")
                {
                    hit.collider.gameObject.GetComponent<CardBehavior>().CardFlip();

                    // runs if the card isn't flipped. logic might be useful?
                    /*if(hit.collider.gameObject.GetComponent<CardBehavior>().flipped)
                    {
                        // do something with this?
                    }*/
                }
            }
        }
    }
}
