using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	
	public enum TURN_STATE{
		
		TURN_BEGIN,
		
		FLIP_ONE,
		FLIP_TWO,
		
		WAIT_ON_UNFLIP,
		
	};
	
	public enum ANIMATION_STATE{
		
		NOT_ANIMATING,
		
		FLIP_ONE,
		FLIP_TWO,
		
		UNFLIP_CARDS,
		
	};
	
	public TURN_STATE turnState;
	public ANIMATION_STATE animationState;
	
	public ParticleSystem windP;
	
	public CardState[] flippedCards = {null,null};
	int numberOfFlippedCards = 0;
	
	public int bulletCount = 1;
	public int health = 30;

	//todo: we should have a discussion about this.
	public CardManager cardManager;

    public void AddFlippedCard(CardState cardState)
    {

		flippedCards[numberOfFlippedCards] = cardState;

		numberOfFlippedCards++;
		
    }

	public void UnflipAllCards(){

		this.animationState = ANIMATION_STATE.UNFLIP_CARDS;
		this.turnState = TURN_STATE.WAIT_ON_UNFLIP;

		for(int i = 0; i < numberOfFlippedCards; i++){

			if(!flippedCards[i].matched){

				flippedCards[i].cardBehavior.PlayAnim("CardUnflipAnim");
				flippedCards[i].cardBehavior.flipped = false;

			}

		}
		
		numberOfFlippedCards = 0;
		
	}

	void DoMatchEffect(CardState cardState){

		switch(cardState.cardType){

			case(CardState.CARD_TYPE.NULL):
			case(CardState.CARD_TYPE.WILD):{

				//null and wild: do nothing
				break;

			}
			case(CardState.CARD_TYPE.WIND):{

				//wind: shuffle cards
				cardManager.shuffleCards();
				windP.Play();
				Debug.Log(windP.isPlaying);
				break;

			}
			case(CardState.CARD_TYPE.LIGHTNING):{

				//lightning: remove a random match.
				cardManager.RemoveRandomMatch();
				break;

			}
			case(CardState.CARD_TYPE.GUN):{
				
				//gun: give the player a bullet.
				bulletCount++;
				break;
				
			}

		};

	}

	void DoAllMatchEffects(){

		//process match effects of cards

		CardState[] cardQueue = new CardState[numberOfFlippedCards];
		System.Array.Copy(flippedCards,cardQueue,numberOfFlippedCards);

		for(int i = 0; i < numberOfFlippedCards; i++){

			if(cardQueue[i] == null) continue;

			if(cardQueue[i].matched){

				//we only process each effect once, regardless of how many cards we flipped.
				//so remove all the cardStates from further down the queue that have the same type as the one that was just matched.

				for(int j = i+1; j < numberOfFlippedCards; j++){

					if(cardQueue[i].cardType == cardQueue[j].cardType){

						cardQueue[j] = null;

					}

				}

				//then, process the match effect

				DoMatchEffect(cardQueue[i]);

			}

		}

	}

    void CheckMatch()
    {

		//checks for a match.
		//removes the matching cards if a match is found, and clears flipped cards if we've selected two without a match.

		bool matched = false;

		for(int i = 0; i < numberOfFlippedCards; i++){

			for(int j = i+1; j < numberOfFlippedCards; j++){

				if(flippedCards[i].IsMatch(flippedCards[j])){

					matched = true;

					flippedCards[i].matched = true;
					flippedCards[j].matched = true;

				}

			}

		}

		DoAllMatchEffects();
		
		if(matched){
			
			this.turnState = TURN_STATE.TURN_BEGIN;
			this.animationState = ANIMATION_STATE.NOT_ANIMATING;
			this.numberOfFlippedCards = 0;
			
		}else{
			
			this.health--;
			UnflipAllCards();

		}
		
		cardManager.FinalizeTurn();

    }

	public void ShootCard(GameObject card)
    {
		
		if(this.turnState != TURN_STATE.TURN_BEGIN) return;
		
		//called when the user attempts to shoot a card.
		//removes a card if there's enough bullets.
		
		if(bulletCount >= 1)
        {
			card.GetComponent<CardBehavior>().Remove();
			bulletCount -= 1;
			
			cardManager.FinalizeTurn();
			
        }
		
    }
	
	public bool PlayerCanInput(){
		
		return(this.turnState == TURN_STATE.TURN_BEGIN || this.turnState == TURN_STATE.FLIP_ONE);
		
	}
	
	public void AdvanceStateOnFlip(){
		
		switch(this.turnState){
			
			case(TURN_STATE.TURN_BEGIN):{
				
				this.turnState = TURN_STATE.FLIP_ONE;
				this.animationState = ANIMATION_STATE.FLIP_ONE;
				
				break;
				
			}
			case(TURN_STATE.FLIP_ONE):{
				
				this.turnState = TURN_STATE.FLIP_TWO;
				this.animationState = ANIMATION_STATE.FLIP_TWO;
				
				break;
				
			}
			
		}
			
	}

	public void advanceStateOnAnimationComplete(){
		
		switch(this.animationState){
			
			case(ANIMATION_STATE.NOT_ANIMATING):{
				
				//this.turnState = TURN_STATE.FLIP_ONE;
				Debug.Log("Warning: trying to complete a nonexistant animation???");
				
				break;
				
			}
			case(ANIMATION_STATE.FLIP_ONE):{
				
				this.animationState = ANIMATION_STATE.NOT_ANIMATING;
				
				break;
				
			}
			case(ANIMATION_STATE.FLIP_TWO):{
				
				CheckMatch();
				
				break;
				
			}
			case(ANIMATION_STATE.UNFLIP_CARDS):{
				
				this.turnState = TURN_STATE.TURN_BEGIN;
				this.animationState = ANIMATION_STATE.NOT_ANIMATING;
				
				break;
				
			}
			
		}
			
	}
	
	public void Start(){
		
		this.turnState = TURN_STATE.TURN_BEGIN;
		this.animationState = ANIMATION_STATE.NOT_ANIMATING;
		
	}
	
}