using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour{
	
	public CardState[] cards;
	
	public int cardRows = 4;
	public int cardColumns = 4;
	
	public float cardOffsetWidth = 1.0f;
	public float cardOffsetHeight = 1.5f;
	
	public GameObject cardObject;
	public Transform cardCenter;
	
	public CardState.CARD_TYPE[] startingTypes = {
		
		CardState.CARD_TYPE.NULL,
		CardState.CARD_TYPE.NULL,
		CardState.CARD_TYPE.NULL,
		CardState.CARD_TYPE.NULL,
		CardState.CARD_TYPE.NULL,
		CardState.CARD_TYPE.NULL,
		CardState.CARD_TYPE.NULL,
		CardState.CARD_TYPE.NULL,
		
		CardState.CARD_TYPE.WIND,
		CardState.CARD_TYPE.WIND,
		CardState.CARD_TYPE.WIND,
		CardState.CARD_TYPE.WIND,
		CardState.CARD_TYPE.WIND,
		CardState.CARD_TYPE.WIND,
		CardState.CARD_TYPE.WIND,
		CardState.CARD_TYPE.WIND,
		
	};
	
	public void initializeCards(){
		
		//create CardState with corresponding GameObjects.
		
		//initial array of CardStates.
		int numberOfCards = cardRows*cardColumns;
		cards = new CardState[numberOfCards];
		
		for(int i = 0; i < numberOfCards; i++){
			
			//instantiate GameObject and CardState
			GameObject instantiated = Object.Instantiate(cardObject, new Vector3(0.0f,0.0f,0.0f), Quaternion.identity);
			cards[i] = new CardState(instantiated);
			
			cards[i].cardType = startingTypes[i];
			cards[i].setCoordinates(this,i);
			
		}
		
	}
	
	public void shuffleCards(){
		
		//use Fischer-Yates shuffle algorithm.
		
		int numberOfCards = cardRows*cardColumns;
		
		for(int i = 0; i < numberOfCards-1; i++){
			
			int swapWith = Random.Range(i,numberOfCards-1);
			
			CardState temp = cards[i];
			cards[i] = cards[swapWith];
			cards[swapWith] = temp;
			
			cards[i].setCoordinates(this,i);
			
		}
		
	}
	
	public void updateAllVisuals(){
		
		//update visual poisition of cards.
		
		int numberOfCards = cardRows*cardColumns;
		
		for(int i = 0; i < numberOfCards; i++){
			
			cards[i].updateVisualPosition(this);
			cards[i].setVisuals();
			
		}
		
	}
	
	void Start(){
		
		initializeCards();
		shuffleCards();
		updateAllVisuals();
		
	}
	
}