/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotTic : MonoBehaviour
{
    public Text[] internalLeftText, internalText, internalRightText;
    public GameStateController gameController; // call GameStateController
    public TileController tileController; // call GameStateController
    

    public void BotMove() {
        int botMove2Drandom = Random.Range(0, 9); // 9 random 1

        switch (Random.Range(0, 3)) // 3 random 1
        {
            case 0: 
                gameController.tileRightList[botMove2Drandom].text = internalLeftText[botMove2Drandom].text = gameController.GivePlayingTurn(); 
                gameController.tileRightButton[botMove2Drandom].image.sprite = gameController.GiveTilePlay_iWhoPlayNow();
                gameController.tileRightButton[botMove2Drandom].interactable = false; break;
            case 1: 
                gameController.tileList[botMove2Drandom].text = internalText[botMove2Drandom].text = gameController.GivePlayingTurn(); 
                gameController.tileButton[botMove2Drandom].image.sprite = gameController.GiveTilePlay_iWhoPlayNow();
                gameController.tileButton[botMove2Drandom].interactable = false; break;
            case 2: 
                gameController.tileLeftList[botMove2Drandom].text = internalRightText[botMove2Drandom].text = gameController.GivePlayingTurn(); 
                gameController.tileLeftButton[botMove2Drandom].image.sprite = gameController.GiveTilePlay_iWhoPlayNow();
                gameController.tileLeftButton[botMove2Drandom].interactable = false; break;
        }
        gameController.EndTurn();
    }
}
*/