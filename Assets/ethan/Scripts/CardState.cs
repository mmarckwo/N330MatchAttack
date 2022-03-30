using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardState{
	
	public enum CARD_TYPE{
		
		WIND,
		NULL,
		
	};
	
	public enum COATING{
		
		NONE,
		ICED,
		INKED,
		
	};
	
	public CARD_TYPE cardType = CARD_TYPE.NULL;
	public COATING coating = COATING.NONE;
	
	public bool chainedRight = false;
	public bool chainedDown = false;
	
	public GameObject cardObject;
	
	public CardState(GameObject _cardObject){
		
		this.cardObject = _cardObject;
		
	}
	
}