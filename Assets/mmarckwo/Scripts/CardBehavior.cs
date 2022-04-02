using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehavior : MonoBehaviour
{
    // test data.
    public string cardType = "fire";
    public int cardNumber = 0;
    public bool flipped = false;

    private Animation cardAnimation;
	
	private string currentAnimation;

    private void Start()
    {
        cardAnimation = GetComponent<Animation>();
    }

    public void CardFlip()
    {
		
		if(!cardAnimation.IsPlaying(currentAnimation)){
			
			// flip the card if it is unflipped. unflip it if it is flipped.
			if (flipped){
				
				// card is flipped.
				playAnim("CardFlipAnim");
				
			}else{
				
				// card is unflipped.
				playAnim("CardUnflipAnim");   
				
			}
			
			flipped = !flipped;
			
		}
        
    }

    void playAnim(string anim)
    {
        cardAnimation.Play(anim);
		currentAnimation = anim;
		
    }
}
