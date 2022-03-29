using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehavior : MonoBehaviour
{
    // test data.
    public string cardType = "fire";
    public int cardNumber = 0;
    public bool FlipCard = false;
    // can't flip while card is flipping.
    private bool flipping = false;

    private Animation cardAnimation;

    private void Start()
    {
        cardAnimation = GetComponent<Animation>();
    }

    public void CardFlip()
    {

        // flip the card if it is unflipped. unflip it if it is flipped.
        if (FlipCard == false && flipping == false)
        {
            // card is flipped.
            flipping = true;
            StartCoroutine(playAnim("CardFlipAnim"));
        } 
        else if (FlipCard == true && flipping == false)
        {
            // card is unflipped.
            flipping = true;
            StartCoroutine(playAnim("CardUnflipAnim"));   
        }
        
    }

    IEnumerator playAnim(string anim)
    {
        cardAnimation.Play(anim);

        while (cardAnimation.IsPlaying(anim))
        {
            yield return null;
        }

        // negate itself.
        FlipCard = !FlipCard;
        flipping = false;

        Debug.Log("done flipping");
        Debug.Log("Card flipped?: " + FlipCard);
    }
}
