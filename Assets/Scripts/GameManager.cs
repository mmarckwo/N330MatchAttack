using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public ParticleSystem windP;
	public CardState[] flippedCards = {null,null};
	int numberOfFlippedCards = 0;
	int bulletCount = 1;

	//todo: we should have a discussion about this.
	public CardManager cardManager;

    public void AddFlippedCard(CardState cardState)
    {

		flippedCards[numberOfFlippedCards] = cardState;

		numberOfFlippedCards++;

        CheckMatch();

    }

	public void UnflipAllCards(){

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

			case(CardState.CARD_TYPE.NULL):{

				//null: do nothing
				break;

			}
			case(CardState.CARD_TYPE.WIND):{

				//wind: shuffle cards
				cardManager.shuffleCards();
				windP.Play();
				Debug.Log(windP.isPlaying);
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

		cardManager.updateAllVisuals();

		if(matched){

			UnflipAllCards();

		}else if(numberOfFlippedCards >= 2){

			UnflipAllCards();

		}

    }

	public void ShootCard(GameObject card)
    {
		if(bulletCount >= 1)
        {
			Destroy(card);
			bulletCount -= 1;
        }
    }

}
