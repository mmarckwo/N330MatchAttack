using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClick : MonoBehaviour
{
    // get gamemanager script from gamemanager object.
    public GameManager gameManger;

    private string cardTypeRef;
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

                    // if the card isn't flipped, get the card type and send it to the game manager.
                    if(hit.collider.gameObject.GetComponent<CardBehavior>().flipped)
                    {
                        cardTypeRef = hit.collider.gameObject.GetComponent<CardBehavior>().cardType;
                        gameManger.CardChecker(cardTypeRef);
                    }
                }
            }
        }
    }
}
