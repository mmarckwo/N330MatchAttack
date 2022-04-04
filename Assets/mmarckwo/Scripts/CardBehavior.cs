using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class CardBehavior : MonoBehaviour
{
	
    public bool flipped = false;

	public CardState cardState;

	private GameManager gameManager;

    private Animation cardAnimation;
	private TextMeshPro cardText;
	
	private string currentAnimation = ""; //the name of the animation currently being played, or an empty string should there be no currently playing animation.
    
	//Awake gets called on instantiation, unlike Start, which gets called on the next frame.
    private void Awake()
    {
        cardAnimation = GetComponent<Animation>();
        cardText = transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshPro>();

		// get game manager reference as card is instantiated.
		GameObject gameManagerObject = GameObject.Find("Game Manager");
		gameManager = gameManagerObject.GetComponent<GameManager>();
		
    }

    public void CardFlip()
    {
		
		//todo: refactor flipping!
		
		//check if we've stopped animating.
		if(currentAnimation == ""){
			
			// flip the card if it is unflipped. unflip it if it is flipped.
			if (flipped){
				
				// card is unflipped.
				PlayAnim("CardUnflipAnim");   
				
			}else{
				
				// card is flipped.
				PlayAnim("CardFlipAnim");
				
			}
			
			flipped = !flipped;
			
		}
        
    }
	
	public void Update(){
		
		//check if we've stopped animating.
		if(currentAnimation != "" && !cardAnimation.IsPlaying(currentAnimation)){
			
			//set animation to empty
			currentAnimation = "";
			
			if(flipped){
				
				//Debug.Log("Flipped animation completed!");
				gameManager.AddFlippedCard(this.cardState);
				
			}else{
				
				//Debug.Log("Unflipped animation completed!");
				
			}
			
		}
		
	}

    public void PlayAnim(string anim)
    {
        
		//play animation, set currentAnimation
		cardAnimation.Play(anim);
		currentAnimation = anim;
		
		
    }
	
	public void setText(string text){
		
		//set the card text
		cardText.SetText(text);
		
	}
	
}
