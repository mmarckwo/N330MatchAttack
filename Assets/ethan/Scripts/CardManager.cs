using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour{
	
	public CardState[] cards;
	
	public int cardRows = 4;
	public int cardColumns = 4;
	
	public float cardOffsetWidth = 1.0f;
	public float cardOffsetHeight = 1.0f;
	
	public GameObject cardObject;
	public Transform cardCenter;
	
	void initializeCards(){
		
		//create CardState with corresponding GameObjects.
		
		//initial array of CardStates.
		int numberOfCards = cardRows*cardColumns;
		cards = new CardState[numberOfCards];
		
		//integer coordinates for cards.
		int x = 0;
		int z = 0;
		
		//floating point coordinates for card game objects.
		float xPosStart = cardCenter.position.x-((float)(cardRows-1))*0.5f*cardOffsetWidth;
		float zPosStart = cardCenter.position.z-((float)(cardColumns-1))*0.5f*cardOffsetHeight;
		
		float xPos = xPosStart;
		float zPos = zPosStart;
		
		for(int i = 0; i < numberOfCards; i++){
			
			//instantiate GameObject and CardState
			GameObject instantiated = Object.Instantiate(cardObject, new Vector3(xPos,cardCenter.position.y,zPos), Quaternion.identity);
			cards[i] = new CardState(instantiated,x,z);
			
			if((i & 0b1) != 0) cards[i].cardType = CardState.CARD_TYPE.WIND;
			
			//move to the next position.
			
			x++;
			
			if(x == cardColumns){
				
				x = 0;
				z++;
				
				xPos = xPosStart;
				zPos += cardOffsetHeight;
				
			}else{
				
				xPos += cardOffsetWidth;
				
			}
			
		}
		
	}
	
	void Start(){
		
		initializeCards();
		
	}
	
}