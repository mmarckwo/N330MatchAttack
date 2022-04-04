using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // cards to check. will need to be adjusted for chained cards.
    private string[] CardList = {"", ""};
    //public ParticleSystem matchEffect;
    // -1 for index 0, 1 for index 1.
    private int slotCheck = -1;

    public void CardChecker(string text)
    {
        Debug.Log("Type is: " + text);

        if (slotCheck == -1)
        {
            CardList[0] = text;
            slotCheck *= -1;
        }
        else if(slotCheck == 1)
        {
            CardList[1] = text;
            slotCheck *= -1;
        }

        Debug.Log(CardList[0] + " " + CardList[1]);
        CheckMatch();


    }

    void CheckMatch()
    {
        // only run the check if cardlist has no empty strings.
        if(CardList[0] != "" && CardList[1] != "")
        {
            if (CardList[0] == CardList[1])
            {

                Debug.Log("Cards match!");
                ParticleSystem ps = GameObject.Find("TestP").GetComponent<ParticleSystem>();
                //ps.play();
                // empty out card list.
                CardList[0] = "";
                CardList[1] = "";
            }
            else
            {
                Debug.Log("Cards don't match.");

                // empty out card list.
                CardList[0] = "";
                CardList[1] = "";
            }
        }

    }
    //void Particalsystemplay(){
//if(CardList[0] != "" && CardList[1] != "")
        //{
          //matchEffect.play();

          //}
      //}
}
