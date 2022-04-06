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
		WILD,
		LIGHTNING,
		GUN,
		
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
	
	public bool matched = false;
	public int discardIndex = -1; //where it is in the discard pile, or -1
	
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
		this.cardBehavior.cardState = this;
		
	}
	
	public void setPositionInGrid(Vector3 center, int numRows, int numColumns, float width, float height, int _x, int _z){
		
		float xPosStart = center.x-((float)(numColumns-1))*0.5f*width;
		float zPosStart = center.z-((float)(numRows-1))*0.5f*height;
		
		float xPos = xPosStart + width*_x;
		float zPos = zPosStart + height*_z;
		
		this.cardObject.transform.position = new Vector3(xPos,center.y,zPos);
		
	}
	
	public void updateVisualPosition(CardManager manager){
		
		//set visual position of card
		
		if(this.matched){
			
			int discardX = this.discardIndex % manager.discardColumns;
			int discardY = this.discardIndex / manager.discardColumns;
			
			setPositionInGrid(manager.discardCenter.position,manager.discardRows,manager.discardColumns,manager.discardOffsetWidth,manager.discardOffsetHeight,discardX,discardY); 
			
			this.cardObject.transform.eulerAngles = new Vector3(0.0f,0.0f,0.0f);
			this.cardObject.transform.GetChild(0).eulerAngles = new Vector3(0.0f,0.0f,180.0f);
			
			
		}else{
			
			/*Vector3 cardCenter = manager.cardCenter.position;
			
			float xPosStart = cardCenter.x-((float)(manager.cardRows-1))*0.5f*manager.cardOffsetWidth;
			float zPosStart = cardCenter.z-((float)(manager.cardColumns-1))*0.5f*manager.cardOffsetHeight;
			
			float xPos = xPosStart + manager.cardOffsetWidth*this.x;
			float zPos = zPosStart + manager.cardOffsetHeight*this.z;
			
			this.cardObject.transform.position = new Vector3(xPos,cardCenter.y,zPos);*/
			
			setPositionInGrid(manager.cardCenter.position,manager.cardRows,manager.cardColumns,manager.cardOffsetWidth,manager.cardOffsetHeight,this.x,this.z); 
			
		}
		
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
			case(CARD_TYPE.WILD):{
				
				this.cardBehavior.setText("Wild");
				
				break;
				
			}
			case(CARD_TYPE.LIGHTNING):{
				
				this.cardBehavior.setText("Lightning");
				
				break;
				
			}
			case(CARD_TYPE.GUN):{
				
				this.cardBehavior.setText("Gun");
				
				break;
				
			}
			
		}
		
	}
	
	public void setCoordinates(CardManager manager,int index){
		
		//from an index, set x and z coordinates
		
		this.x = index % manager.cardRows;
		this.z = index / manager.cardRows;
		
	}
	
	public bool IsMatch(CardState other){
		
		if(other.cardType == this.cardType) return(true);
		if(other.cardType == CARD_TYPE.WILD) return(true);
		if(this.cardType == CARD_TYPE.WILD) return(true);
		
		return(false);
		
	}
	
}