using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    [Header("Component References")]
    public GameStateController gameController; // call GameStateController
    public Button ticButton; // the box button we play on
    public Text internalText; // button's text

    /// <summary>
    /// Called everytime we press the button, we update the state of this tile.
    /// The internal tracking for whos position (the text component) and disable the button
    /// </summary>
    /// 
    public void UpdateTile()
    {
        internalText.text = gameController.GivePlayingTurn(); // find current players turn (string) and go in button's text
        ticButton.image.sprite = gameController.GiveTilePlay_iWhoPlayNow(); // find what now player icon
        ticButton.interactable = false; // no repress
        gameController.EndTurn(); // next turn
    }

    /*public void BotMove()
    {
        int botMove2Drandom = Random.Range(0, 9); // 9 random 1

        switch (Random.Range(0, 3)) // 3 random 1
        {
            case 0: 
                gameController.tileRightList[botMove2Drandom].text = internalText.text = gameController.GivePlayingTurn(); 
                gameController.tileRightButton[botMove2Drandom].image.sprite = gameController.GiveTilePlay_iWhoPlayNow();
                gameController.tileRightButton[botMove2Drandom].interactable = false; break;
            case 1: 
                gameController.tileList[botMove2Drandom].text = internalText.text = gameController.GivePlayingTurn(); 
                gameController.tileButton[botMove2Drandom].image.sprite = gameController.GiveTilePlay_iWhoPlayNow();
                gameController.tileButton[botMove2Drandom].interactable = false; break;
            case 2: 
                gameController.tileLeftList[botMove2Drandom].text = internalText.text = gameController.GivePlayingTurn(); 
                gameController.tileLeftButton[botMove2Drandom].image.sprite = gameController.GiveTilePlay_iWhoPlayNow();
                gameController.tileLeftButton[botMove2Drandom].interactable = false; break;
        }
        gameController.EndTurn();
    }*/

    public void ResetTile()
    {
        internalText.text = ""; // reset button's text
        ticButton.image.sprite = gameController.tileNone; // reset image
    }
}