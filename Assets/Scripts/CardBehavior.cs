using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class CardBehavior : MonoBehaviour
{
	
    public bool flipped = false;
	
	GameManager.ANIMATION_STATE animationState;

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
		
		if(this.cardState.matched) return;
		
		//todo: refactor flipping!
		
		//check if we've stopped animating.
		if(currentAnimation == ""){
			
			if(flipped) return;
			
			if(gameManager.PlayerCanInput()){
				
				gameManager.AdvanceStateOnFlip();
				
				//gameManager.turnState = GameManager.TURN_STATE.FLIP_ONE;
				//gameManager.animationState = GameManager.ANIMATION_STATE.FLIP_ONE;
				
				PlayAnim("CardFlipAnim");
				flipped = true;
				gameManager.AddFlippedCard(this.cardState);
				
				
			}
			
			/*if(gameManager.turnState ==  GameManager.TURN_STATE.FLIP_ONE && gameManager.playerCanInput()){
				
				gameManager.turnState = GameManager.TURN_STATE.FLIP_TWO;
				gameManager.animationState = GameManager.ANIMATION_STATE.FLIP_ONE;
				
				PlayAnim("CardFlipAnim");
				flipped = true;
				gameManager.AddFlippedCard(this.cardState);
				
				
			}*/
			
		}
        
    }
	
	public void Update(){
		
		//check if we've stopped animating.
		if(currentAnimation != "" && !cardAnimation.IsPlaying(currentAnimation)){
			
			//set animation to empty
			currentAnimation = "";
			
			if(this.animationState == gameManager.animationState){
				
				gameManager.advanceStateOnAnimationComplete();
				
			}
			
		}
		
	}

    public void PlayAnim(string anim)
    {
		
		this.animationState = gameManager.animationState;
		
		//play animation, set currentAnimation
		cardAnimation.Play(anim);
		currentAnimation = anim;
		
		
    }
	
	public void setText(string text){
		
		//set the card text
		cardText.SetText(text);
		
	}
	
	public void Remove(){
		
		this.cardState.matched = true;
		
	}
	
}
