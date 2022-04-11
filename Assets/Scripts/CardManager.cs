using System.Collections;
using System.Collections.Generic;
using static System.Tuple;
using UnityEngine;

public class CardManager : MonoBehaviour{
	
	public GameManager gameManager;
	
	public CardState[] cards;
	
	public GameObject cardObject;
	
	public Transform cardCenter;
	
	public int cardRows;
	public int cardColumns;
	
	public float cardOffsetWidth;
	public float cardOffsetHeight;
	
	public Transform discardCenter;
	
	public int discardRows;
	public int discardColumns;
	
	public float discardOffsetWidth;
	public float discardOffsetHeight;
	
	public int nextDiscardIndex; //where in the discard pile should the next marched/discarded card go?
	
	public CardState.CARD_TYPE[] startingTypes;
	
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
			//cards[i].cardType = CardState.CARD_TYPE.FREEZE;
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
	
	public List<System.Tuple<int,int>> FindAllMatches(){
		
		List<System.Tuple<int,int>> pairs = new List<System.Tuple<int,int>>();
		
		for(int i = 0; i < cards.Length; i++){
			
			if(cards[i].matched) continue;
			
			for(int j = i+1; j < cards.Length; j++){
				
				if(!cards[j].matched && cards[j].IsMatch(cards[i]) && gameManager.PlayerCanInput(cards[i],true) && gameManager.PlayerCanInput(cards[j],true)){
					
					System.Tuple<int,int> newPair = new System.Tuple<int,int>(i,j);
					
					pairs.Add(newPair);
					
				}
				
			}
			
		}
		
		return(pairs);
		
	}
	
	public System.Tuple<int,int> FindRandomMatch(){
		
		List<System.Tuple<int,int>> pairs = FindAllMatches();
		
		if(pairs.Count > 0){
			
			int matchIndex = Random.Range(0,pairs.Count-1);
			
			System.Tuple<int,int> chosenPair = pairs[matchIndex];
			/*cards[chosenPair.Item1].matched = true;
			cards[chosenPair.Item2].matched = true;*/
			
			return(chosenPair);
			
		}
		
		return(null);
		
	}
	
	public void RemoveRandomMatch(){
		
		System.Tuple<int,int> chosenPair = FindRandomMatch();
		
		if(chosenPair != null){
			
			cards[chosenPair.Item1].matched = true;
			cards[chosenPair.Item2].matched = true;
			
		}
		
	}
	
	public void FinalizeTurn(){
		
		for(int i = 0; i < cards.Length; i++){
			
			if(cards[i].matched && cards[i].discardIndex == -1){
				
				cards[i].discardIndex = this.nextDiscardIndex;
				this.nextDiscardIndex++;
				
				cards[i].coating = CardState.COATING.NONE;
				
			}
			
		}
		
		//todo: check if all cards are cleared here
		
		//then update visuals
		
		updateAllVisuals();
		
		//prepare next turn
		
		gameManager.PrepareTurn();
		
	}
	
	/*public void CountActiveCards(){
		
		int count = 0;
		
		for(int i = 0; i < cards.Length; i++){
			
			if(cards[i] != null && !cards[i].matched) count++;
			
		}
		
		Debug.Log(count);
		
	}*/
	
	public void Restart(){
		
		this.nextDiscardIndex = 0;
		initializeCards();
		shuffleCards();
		updateAllVisuals();
		
		gameManager.Restart();
		
	}
	
	void Start(){
		
		Restart();
		
	}
	
}