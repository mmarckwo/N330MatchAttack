using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a class representing the internal state/logic of the card.
//note that this is not a MonoBehavior, i.e. it won't get attached to a game object. But CardManager (which uses CardState) will.
public class CardState{
	
	//the type of card it is.
	public enum CARD_TYPE{
		
		WIND,
		NULL,
		
	};
	
	//if we're iced, inked, or not.
	public enum COATING{
		
		NONE,
		ICED,
		INKED,
		
	};
	
	public CARD_TYPE cardType = CARD_TYPE.NULL;
	public COATING coating = COATING.NONE;
	
	//if we're chained/connected to the card right/down to us.
	//to check if we're chained to the card left of us, get the card to our left and check its chainedRight. Same deal for the card above us.
	public bool chainedRight = false;
	public bool chainedDown = false;
	
	//the game object that contains the models/textures/etc for the card object.
	public GameObject cardObject;
	public CardBehavior cardBehavior;
	
	//coordinates for where the card is on the grid.
	public int x; //horizontal
	public int z; //vertical
	
	public CardState(GameObject _cardObject){
		
		//set our game object.
		this.cardObject = _cardObject;
		this.cardBehavior = this.cardObject.transform.GetChild(0).gameObject.GetComponent<CardBehavior>();
		
	}
	
	public void updateVisualPosition(CardManager manager){
		
		//set visual position of card
		
		Vector3 cardCenter = manager.cardCenter.position;
		
		float xPosStart = cardCenter.x-((float)(manager.cardRows-1))*0.5f*manager.cardOffsetWidth;
		float zPosStart = cardCenter.z-((float)(manager.cardColumns-1))*0.5f*manager.cardOffsetHeight;
		
		float xPos = xPosStart + manager.cardOffsetWidth*this.x;
		float zPos = zPosStart + manager.cardOffsetHeight*this.z;
		
		this.cardObject.transform.position = new Vector3(xPos,cardCenter.y,zPos);
		
	}
	
	public void setVisuals(){
		
		switch(this.cardType){
			
			case(CARD_TYPE.WIND):{
				
				this.cardBehavior.setText("Wind");
				
				break;
				
			}
			case(CARD_TYPE.NULL):{
				
				this.cardBehavior.setText("Null");
				
				break;
				
			}
			
		}
		
	}
	
	
	public void setCoordinates(CardManager manager,int index){
		
		//from an index, set x and z coordinates
		
		this.x = index % manager.cardRows;
		this.z = index / manager.cardRows;
		
	}
	
}