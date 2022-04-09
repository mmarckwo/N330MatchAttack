using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
		
		SHOW_PAIR,
		
	};
	
	public TURN_STATE turnState;
	public ANIMATION_STATE animationState;
	bool viewingDiscardPile = false;
	
	public ParticleSystem windP;
	
	public CardState[] flippedCards = {null,null};
	int numberOfFlippedCards = 0;
	
	// start with 1 bullet.
	public int bulletCount = 1;
	public float health = 10;
	private float maxHealth;
	public TextMeshProUGUI bulletCountText;

	[Header("HP/UI")]
	
	public GameObject gameCamera;
	private Vector3 gameCameraTargetPosition;
	public float gameCameraLerpSpeed;
	
	public Image healthBarFill;
	public Color goodHealth = new Color(69, 255, 137);
	public Color lowHealth = new Color(255, 0, 85);
	// higher lerp speed goes faster.
	public float healthLerpSpeed = 5;
	
	public Text discardButtonText;

	[Header("Sounds")]
	public AudioSource FlameSound;
	public AudioSource FreezeSound;
	public AudioSource WindSound;
	public AudioSource InkSound;
	public AudioSource NullSound;
	public AudioSource ThunderSound;
	public AudioSource ReloadSound;
	public AudioSource ShootSound;
	public AudioSource HurtSound;
	public AudioSource FlipSound;

	[Header("Card Manager")]
	public CardManager cardManager;

    private void Update()
    {
		
		HPLerp();
		
		CameraLerp();
		
    }

    public void AddFlippedCard(CardState cardState)
    {

		flippedCards[numberOfFlippedCards] = cardState;

		numberOfFlippedCards++;
		
    }

	public void UnflipUnmatchedCards(){

		FlipSound.Play();
		this.animationState = ANIMATION_STATE.UNFLIP_CARDS;
		this.turnState = TURN_STATE.WAIT_ON_UNFLIP;

		for(int i = 0; i < cardManager.cards.Length; i++){

			if(!cardManager.cards[i].matched && cardManager.cards[i].cardBehavior.flipped){

				cardManager.cards[i].cardBehavior.PlayAnim("CardUnflipAnim");
				cardManager.cards[i].cardBehavior.flipped = false;

			}

		}
		
		numberOfFlippedCards = 0;
		
	}

	public CardState findUnmatchedCard(){
		
		List<int> unmatchedCards = new List<int>();
		
		//ink: ink a random card.
		for(int i = 0; i < cardManager.cards.Length; i++){
			
			if(!cardManager.cards[i].matched){
				
				unmatchedCards.Add(i);
				
			}
			
		}
		
		if(unmatchedCards.Count > 0){
			
			 int index = Random.Range(0,unmatchedCards.Count-1);
			 
			 return(cardManager.cards[unmatchedCards[index]]);//.coating = CardState.COATING.INKED;
			
		}
		
		return(null);
		
	}
				
	
	void DoMatchEffect(CardState cardState){

		switch(cardState.cardType){

			case(CardState.CARD_TYPE.NULL):
			case(CardState.CARD_TYPE.WILD):{

				//null and wild: do nothing
				NullSound.Play();
				break;

			}
			case(CardState.CARD_TYPE.WIND):{

				//wind: shuffle cards
				WindSound.Play();
				cardManager.shuffleCards();
				windP.Play();
				Debug.Log(windP.isPlaying);
				break;

			}
			case(CardState.CARD_TYPE.LIGHTNING):{

				//lightning: remove a random match.
				ThunderSound.Play();
				cardManager.RemoveRandomMatch();
				break;

			}
			case(CardState.CARD_TYPE.GUN):{
				
				//gun: give the player a bullet.
				ReloadSound.Play();
				bulletCount++;
				UpdateBulletCount();
				break;
				
			}
			case(CardState.CARD_TYPE.FIRE):{
				
				//fire: show a random match.
				FlameSound.Play();
				System.Tuple<int,int> match = cardManager.FindRandomMatch();
				
				if(match != null){
					
					this.animationState = ANIMATION_STATE.SHOW_PAIR;
					
					this.cardManager.cards[match.Item1].cardBehavior.PlayAnim("CardFlipAnim");
					this.cardManager.cards[match.Item1].cardBehavior.flipped = true;
					this.cardManager.cards[match.Item2].cardBehavior.PlayAnim("CardFlipAnim");
					this.cardManager.cards[match.Item2].cardBehavior.flipped = true;
					
				}
				
				break;
				
			}
			case(CardState.CARD_TYPE.INK):{
				
				InkSound.Play();
				CardState card = findUnmatchedCard();
				
				if(card != null) card.coating = CardState.COATING.INKED;
				
				break;
				
			}
			case(CardState.CARD_TYPE.FREEZE):{
				
				FreezeSound.Play();
				CardState card = findUnmatchedCard();
				
				if(card != null) card.coating = CardState.COATING.ICED;
				
				break;
				
			}
			
		};

	}

	void DoAllMatchEffects(){

		//process match effects of cards
		
		bool removedIce = false;
		
		CardState[] cardQueue = new CardState[numberOfFlippedCards];
		System.Array.Copy(flippedCards,cardQueue,numberOfFlippedCards);

		for(int i = 0; i < numberOfFlippedCards; i++){

			if(cardQueue[i] == null) continue;

			if(cardQueue[i].matched){
				
				
				if(!removedIce){
					
					//remove ice before we process the first effect.
					RemoveIce();
					removedIce = true;
					
				}
				
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
	
	void RemoveIce(){
		
		for(int i = 0; i < cardManager.cards.Length; i++){
			
			if(cardManager.cards[i].coating == CardState.COATING.ICED){
				
				cardManager.cards[i].coating = CardState.COATING.NONE;
				
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
			
			if(this.animationState != ANIMATION_STATE.SHOW_PAIR){
				
				this.turnState = TURN_STATE.TURN_BEGIN;
				this.animationState = ANIMATION_STATE.NOT_ANIMATING;
				this.numberOfFlippedCards = 0;
				
			}
			
		}else{
			
			TakeDamage();
			UnflipUnmatchedCards();

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
			ShootSound.Play();
			card.GetComponent<CardBehavior>().Remove();
			bulletCount -= 1;
			bulletCountText.SetText(bulletCount.ToString());

			cardManager.FinalizeTurn();
			
			UpdateBulletCount();
			
        }
		
    }
	
	public bool PlayerCanInput(CardBehavior card){
		
		if(!(this.turnState == TURN_STATE.TURN_BEGIN || this.turnState == TURN_STATE.FLIP_ONE)) return(false);
		
		if(card.cardState.coating == CardState.COATING.ICED) return(false);
		
		return(true);
		
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
			case(ANIMATION_STATE.SHOW_PAIR):{
				
				UnflipUnmatchedCards();
				break;
				
			}
			
		}
			
	}
	
	public void Start(){
		
		this.turnState = TURN_STATE.TURN_BEGIN;
		this.animationState = ANIMATION_STATE.NOT_ANIMATING;

		// set max health value to starting hp.
		maxHealth = health;

		// initialize bullet count.
		UpdateBulletCount();
		
		//initialize camera target
		this.gameCameraTargetPosition = this.gameCamera.transform.position;

	}

	void TakeDamage()
    {

		HurtSound.Play();
		health--;

		// make the health bar red when the player is at low HP.
		if ((health / maxHealth <= .30) || (health == 1))
		{
			healthBarFill.color = lowHealth;
		}

		if (health == 0)
		{
			Debug.Log("You are dead.");
		}
	}

	void HPLerp()
    {
		// goes in Update() to animate lerp.
		// update health bar fill amount.
		healthBarFill.fillAmount = Mathf.Lerp(healthBarFill.fillAmount, (health / maxHealth), Time.deltaTime * healthLerpSpeed);
	}
	
	void CameraLerp()
	{
		
		float newX = Mathf.Lerp(gameCamera.transform.position.x,gameCameraTargetPosition.x,Time.deltaTime * gameCameraLerpSpeed);
		
		gameCamera.transform.position = new Vector3(newX,gameCamera.transform.position.y,gameCamera.transform.position.z);
		
		//gameCameraTargetPosition*gameCameraLerp + gameCamera.transform.position*(1.0f-gameCameraLerp);
		
	}
	
	void UpdateBulletCount()
    {
		string count = bulletCount.ToString();
		bulletCountText.SetText(count);
    }
	
	public void ShowDiscardButtonClicked(){
		
		if(!viewingDiscardPile){
			
			this.gameCameraTargetPosition = new Vector3(this.cardManager.discardCenter.position.x,gameCamera.transform.position.y,gameCamera.transform.position.z);
			this.discardButtonText.text = "Back";
			
		}else{
			
			this.gameCameraTargetPosition = new Vector3(0.0f,gameCamera.transform.position.y,gameCamera.transform.position.z);
			this.discardButtonText.text = "Show Discard";
			
		}
		
		viewingDiscardPile = !viewingDiscardPile;
		
	}
	
}