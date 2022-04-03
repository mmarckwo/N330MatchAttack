using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class CardBehavior : MonoBehaviour
{
    // test data.
    public int cardNumber = 0;
    public bool flipped = false;

	private string cardType;

    private Animation cardAnimation;
	private TextMeshPro cardText;
	
	private string currentAnimation;
    
	//Awake gets called on instantiation, unlike Start, which gets called on the next frame.
    private void Awake()
    {
        cardAnimation = GetComponent<Animation>();
        cardText = transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshPro>();
    }

    public void CardFlip()
    {
		
		if(!cardAnimation.IsPlaying(currentAnimation)){
			
			// flip the card if it is unflipped. unflip it if it is flipped.
			if (flipped){
				
				// card is unflipped.
				playAnim("CardUnflipAnim");   
				
			}else{
				
				// card is flipped.
				playAnim("CardFlipAnim");
				
			}
			
			flipped = !flipped;

			// log the card type for testing.
			Debug.Log(cardType);
			
		}
        
    }

    void playAnim(string anim)
    {
        
		cardAnimation.Play(anim);
		currentAnimation = anim;
		
    }
	
	public void setText(string text){
		
		cardText.SetText(text);
		// set the card type.
		cardType = text;
		
	}
	
}
