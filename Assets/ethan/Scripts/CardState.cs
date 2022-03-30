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
	
	//coordinates for where the card is on the grid.
	public int x; //horizontal
	public int z; //vertical
	
	public CardState(GameObject _cardObject, int _x, int _z){
		
		//set our game object.
		this.cardObject = _cardObject;
		
	}
	
}