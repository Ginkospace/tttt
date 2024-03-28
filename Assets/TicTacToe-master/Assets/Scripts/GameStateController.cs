//https://github.com/ivuecode/
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

[System.Serializable]
public class NewTileList
{
    public List<Text> newTileList;
}

public class GameStateController : MonoBehaviour
{
    // disclaimer: this C# file is huge (in Associate Degree student coder perspective), therefore we contracted a lot of "if" statements for better reading and overall design.

    [Header("Icon and Tile")]
    public InputField[] playInputName;  // old name function
    public Image[] playIcon; // Icon   // can have a lot player (+AI)
    public Sprite[] tilePlay;         // tile state
    public Sprite tileNone;          // air tile (separate from old code "tilePlay")
    public Text winText;            // print win and win who (old function)

    [Header("State")]
    public GameObject endGamePage;     // if win, page print win who
    public GameObject[] game3DObject; // shit code when delete the game die
    public bool gameNow3D = true;    // see 3D or 2D
    public bool thisTurnWin;        // see now win or not
    public bool[] whoIsBot;        // list of who is the bot
    public Color restPlayColor;   // blacken those aren't their turn 
    public Color playingColor;   // good color to player that in their turn
    public int iDimensions;     // see how many dimension (old code because now only 2D and 3D)
    public int[] tilePerD;     // tilePerD[] now not used, every dimensions have the same only 3 tiles
    public int whoPlayFirst;  // old code (see X/O who the first)
    public int playerCount;  // see how many player (because include 0, thus "1" is 2 players)
    
    
    [Header("3D shit list")]
    public NewTileList[] newTileList; // old code
    public Text[] tileLeftList;      // left all tiles
    public Text[] tileList;         // old middle all tiles
    public Text[] tileRightList;   // right all tiles
    public Button[] tileLeftButton;      // left all tiles
    public Button[] tileButton;         // old middle all tiles
    public Button[] tileRightButton;   // right all tiles
    
    [Header("Don't alter internal")] // change through gaming
    public string playingTurn;      // internal track now who turn
    public string[] playerName;    // internal set display name
    public int moveCount;         // internal move counter
    public int gameEndCount;     // count how many ends
    public int iWhoPlayNow;     // see who play now
    public bool botHitWall;
    public int botMove2Drandom;


    public TileController tileController; // because we need BotMove
    public static GameStateController Instance; // useless

    private void Start() // when start the game
    {
        iWhoPlayNow = whoPlayFirst;
        playingTurn = whoPlayFirst.ToString(); // who play first = his turn
        foreach (Image icons in playIcon) { icons.color = restPlayColor; } // all player rest and wait to play

        for (int i = 0; i < playerCount; i++) // for all player
        {   if (playingTurn == i.ToString()) { playIcon[i].color = playingColor; }              // if the player is "whoPlayFirst" then play first
            playInputName[i].onValueChanged.AddListener(delegate { OnPlayerNameChanged(i); }); // add listener in name input fields and invoke when name altered.
            playerName[i] = playInputName[i].text;                                            // Set default value to what inputField text is
        } // should have bug if the playingTurn is not number

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) { RestartGame(); } // Restart Game
        if (Input.GetKeyDown(KeyCode.D)) { Game3Dto2D(); if (gameNow3D) { gameNow3D = false; } else gameNow3D = true; }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha0)) { SceneManager.LoadScene(10); }
            if (Input.GetKeyDown(KeyCode.Alpha1)) { SceneManager.LoadScene(11); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { SceneManager.LoadScene(12); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { SceneManager.LoadScene(13); }
            if (Input.GetKeyDown(KeyCode.Alpha4)) { SceneManager.LoadScene(14); }
            if (Input.GetKeyDown(KeyCode.Alpha5)) { SceneManager.LoadScene(15); }
            if (Input.GetKeyDown(KeyCode.Alpha6)) { SceneManager.LoadScene(16); }
            if (Input.GetKeyDown(KeyCode.Alpha7)) { SceneManager.LoadScene(17); }
            if (Input.GetKeyDown(KeyCode.Alpha8)) { SceneManager.LoadScene(18); }
            if (Input.GetKeyDown(KeyCode.Alpha9)) { SceneManager.LoadScene(19); }
        }
        else if (Input.GetKey(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.Alpha0)) { SceneManager.LoadScene(20); }
            if (Input.GetKeyDown(KeyCode.Alpha1)) { SceneManager.LoadScene(21); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { SceneManager.LoadScene(22); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { SceneManager.LoadScene(23); }
            if (Input.GetKeyDown(KeyCode.Alpha4)) { SceneManager.LoadScene(24); }
            if (Input.GetKeyDown(KeyCode.Alpha5)) { SceneManager.LoadScene(25); }
            if (Input.GetKeyDown(KeyCode.Alpha6)) { SceneManager.LoadScene(26); }
            if (Input.GetKeyDown(KeyCode.Alpha7)) { SceneManager.LoadScene(27); }
            if (Input.GetKeyDown(KeyCode.Alpha8)) { SceneManager.LoadScene(28); }
            if (Input.GetKeyDown(KeyCode.Alpha9)) { SceneManager.LoadScene(29); }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha0)) { SceneManager.LoadScene(0); }
            if (Input.GetKeyDown(KeyCode.Alpha1)) { SceneManager.LoadScene(1); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { SceneManager.LoadScene(2); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { SceneManager.LoadScene(3); }
            if (Input.GetKeyDown(KeyCode.Alpha4)) { SceneManager.LoadScene(4); }
            if (Input.GetKeyDown(KeyCode.Alpha5)) { SceneManager.LoadScene(5); }
            if (Input.GetKeyDown(KeyCode.Alpha6)) { SceneManager.LoadScene(6); }
            if (Input.GetKeyDown(KeyCode.Alpha7)) { SceneManager.LoadScene(7); }
            if (Input.GetKeyDown(KeyCode.Alpha8)) { SceneManager.LoadScene(8); }
            if (Input.GetKeyDown(KeyCode.Alpha9)) { SceneManager.LoadScene(9); }
            if (Input.GetKeyDown(KeyCode.Alpha0)) { SceneManager.LoadScene(0); }
        }

        if (Input.GetKeyDown(KeyCode.B)) { BotModeToWhoPlayNow(iWhoPlayNow); }
    }

    public void EndTurn()
    {
        thisTurnWin = false;
        /*switch (iDimensions)
        {   case 1: break;
            case 2: if (moveCount >= 9) { GameOver("Draw"); } else { CheckTurnOld(tileList); break; }
            case 3: if (moveCount >= 27) { GameOver("Draw"); } else { CheckTurnOld3D(); break; }
            case 4: if (moveCount >= 81) { GameOver("Draw"); } else { CheckTurnOld4D(); break; }
            case 5: if (moveCount >= 243) { GameOver("Draw"); } else { CheckTurnOld5D(); break; }
            case 6: break;
        }*/ // not used

        // tilePerD[] now not used, every dimensions have the same only 3 tiles
        if (++moveCount >= Math.Pow(3, iDimensions)/*||(!gameNow3D && moveCount >= 9)*/) { GameOver("Draw"); } // if full then draw
        else { CheckTurnOld3D(); } // now only check3D

        if (thisTurnWin == true) { GameOver(playingTurn); thisTurnWin = false; }  
        else { ChangeTurn(); }
    }

    /*
    we are now using
    [0][1][2]
    [3][4][5]
    [6][7][8]
    for our tic tac toe
    */

    public void CheckTurnOld(Text[] tileList)
    {   for (int i = 0; i<3; i++) { // check 3 vertical [0][3][6]s
            if (tileList[i].text == playingTurn && tileList[i + 3].text == playingTurn && tileList[i + 6].text == playingTurn) { thisTurnWin = true; break; } 
        }
        for (int i = 0; i < 8; i+=3) { // check 3 horizontal [0][1][2]s
            if (tileList[i].text == playingTurn && tileList[i + 1].text == playingTurn && tileList[i + 2].text == playingTurn) { thisTurnWin = true; break; } 
        }
        if (tileList[4].text == playingTurn) // middle have, then check X ("\" and "/")
        {   if (tileList[0].text == playingTurn && tileList[8].text == playingTurn) { thisTurnWin = true; } // check [0][4][8] "\"
            else if (tileList[2].text == playingTurn && tileList[6].text == playingTurn) { thisTurnWin = true; } // check [2][4][6] "/"
        }  
        
        print("Check 2D end");
    } // brute-force checking is the only way it works consistently, I hate it too.

    /*if (moveCount >= 9) GameOver("D");
                else if (iDimensions == 2) {
                    if (tile2D[1, 1].text == playingTurn && ((tile2D[0, 0].text == playingTurn && tile2D[2, 2].text == playingTurn) || (tile2D[0, 2].text == playingTurn && tile2D[2, 0].text == playingTurn))) { GameOver(playingTurn);  }
                    for (int i = 0; i < tile2D.Length; i++) {
                        if (tile2D[i, 0].text == playingTurn && tile2D[i, 1].text == playingTurn && tile2D[i, 2].text == playingTurn) { GameOver(playingTurn); break; }
                        else if (tile2D[0, i].text == playingTurn && tile2D[1, i].text == playingTurn && tile2D[2, i].text == playingTurn) { GameOver(playingTurn); break; } } }
                else ChangeTurn();*/ //useless CheckTurn // not used ancient shit code



    public void CheckTurnOld3D()
    {
        CheckTurnOld(tileLeftList);
        CheckTurnOld(tileList);
        CheckTurnOld(tileRightList);

        for (int i = 0; i < 9; i++) { // all itself (3D horizontal in all lines)
            if (tileLeftList[i].text == playingTurn && tileList[i].text == playingTurn && tileRightList[i].text == playingTurn) { thisTurnWin = true; break; }
        }

        for (int i = 0; i < 3; i++) // check the flat "X" in 3D
        {   if (tileLeftList[i].text == playingTurn && tileList[i + 3].text == playingTurn && tileRightList[i + 6].text == playingTurn) { thisTurnWin = true; break; }  // check [0][4][8] "\"
            else if (tileRightList[i].text == playingTurn && tileList[i + 3].text == playingTurn && tileLeftList[i + 6].text == playingTurn) { thisTurnWin = true; break; } // check [2][4][6] "/"
        }
        
        for (int i = 0; i < 8; i += 3) // check the lay "X" in 3D
        {   if (tileLeftList[i].text == playingTurn && tileList[i + 1].text == playingTurn && tileRightList[i + 2].text == playingTurn) { thisTurnWin = true; break; } // check [0][1][2] "\"
            else if (tileRightList[i].text == playingTurn && tileList[i + 1].text == playingTurn && tileLeftList[i + 2].text == playingTurn) { thisTurnWin = true; break; } // check [2][1][0] "/"
        }

        // if (tileList[4].text == playingTurn && ((tileLeftList[0].text == playingTurn && tileRightList[8].text == playingTurn) || (tileLeftList[2].text == playingTurn && tileRightList[6].text == playingTurn) || (tileLeftList[6].text == playingTurn && tileRightList[2].text == playingTurn) || (tileLeftList[8].text == playingTurn && tileRightList[0].text == playingTurn))) { thisTurnWin = true; }
        // up code same as code below

        if (tileList[4].text == playingTurn) // the 4 full 3D "X"
        {   if (tileLeftList[0].text == playingTurn && tileRightList[8].text == playingTurn) { thisTurnWin = true; }
            else if (tileLeftList[2].text == playingTurn && tileRightList[6].text == playingTurn) { thisTurnWin = true; }
            else if (tileLeftList[6].text == playingTurn && tileRightList[2].text == playingTurn) { thisTurnWin = true; }
            else if (tileLeftList[8].text == playingTurn && tileRightList[0].text == playingTurn) { thisTurnWin = true; }
        }

        print("Check 3D end");
    }

    /// <summary>
    /// Changes the internal tracker for whos turn it is
    /// </summary>
    public void ChangeTurn()
    {
        if (iWhoPlayNow == playerCount) { iWhoPlayNow = 0; playingTurn = "0"; } // if all player played then go back the player 0
        else { iWhoPlayNow++; playingTurn = iWhoPlayNow.ToString(); } // else next player 

        foreach (Image icons in playIcon) { icons.color = restPlayColor; } // all player rest
        playIcon[iWhoPlayNow].color = playingColor; // only the next player unrest (light up)


        /*playingTurn = (playingTurn == "X") ? "O" : "X";
        if (playingTurn == "X")
        {
            playIcon[1].color = playingColor;
            playIcon[0].color = restPlayColor;
        }
        else
        {
            playIcon[1].color = restPlayColor;
            playIcon[0].color = playingColor;
        }*/ // not used ancient old code



        if (whoIsBot[iWhoPlayNow] == true) { botHitWall = false; BotMove(); EndTurn(); } // bot move and check3D
        //if (botHitWall == true) { BotMove(); }
        //else { EndTurn(); }

    }

    public void BotMove()
    {
        botHitWall = false;
        botMove2Drandom = UnityEngine.Random.Range(0, 9); // 9 random 1

        if (gameNow3D)
        {
            switch (UnityEngine.Random.Range(0, 3)) // 3 random 1
            {
                case 0:
                    if (tileRightList[botMove2Drandom].text == "")
                    {
                        tileRightList[botMove2Drandom].text = GivePlayingTurn();
                        tileRightButton[botMove2Drandom].image.sprite = GiveTilePlay_iWhoPlayNow();
                        tileRightButton[botMove2Drandom].interactable = false;
                    } //else BotMove();
                    else botHitWall = true;
                    break;

                case 1:
                    if (tileList[botMove2Drandom].text == "")
                    {
                        tileList[botMove2Drandom].text = GivePlayingTurn();
                        tileButton[botMove2Drandom].image.sprite = GiveTilePlay_iWhoPlayNow();
                        tileButton[botMove2Drandom].interactable = false;
                    } //else BotMove();
                    else botHitWall = true;
                    break;
                case 2:
                    if (tileLeftList[botMove2Drandom].text == "")
                    {
                        tileLeftList[botMove2Drandom].text = GivePlayingTurn();
                        tileLeftButton[botMove2Drandom].image.sprite = GiveTilePlay_iWhoPlayNow();
                        tileLeftButton[botMove2Drandom].interactable = false;
                    } //else BotMove();
                    else botHitWall = true;
                    break;
            }
        } else if (tileList[botMove2Drandom].text == "")
        {
            tileList[botMove2Drandom].text = GivePlayingTurn();
            tileButton[botMove2Drandom].image.sprite = GiveTilePlay_iWhoPlayNow();
            tileButton[botMove2Drandom].interactable = false;
        } //else BotMove();
        else botHitWall = true;

        if (botHitWall == true) { BotMove(); }
        
    } // do you know how happy I am



    /// <summary>
    /// Called when the game has found a win condition or draw
    /// </summary>
    /// <param name="winningPlayer">X O D</param>
    private void GameOver(string winningPlayer)
    {
        gameEndCount++;
        if (winningPlayer == "Draw") { winText.text = "DRAW"; }
        else { winText.text = "Winner is player" + winningPlayer + "!"; }

        /*switch (winningPlayer)
        {
            case "Draw":
                winText.text = "DRAW"; break;
            case "X":
                winText.text = "Winner is " + playerName[0] + "!"; break;
            case "O":
                winText.text = "Winner is " + playerName[1] + "!"; break;
            default:
                winText.text = "Winner is " + winningPlayer + "!"; break;
        }*/
        endGamePage.SetActive(true);
        ToggleButtonState(false);
    }

    /// <summary>
    /// Restarts the game state
    /// </summary>
    public void RestartGame()
    {   SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); print("RestartGame");
        /*// Reset some gamestate properties
        moveCount = 0;
        playingTurn = whoPlayFirst;
        ToggleButtonState(true);
        endGamePage.SetActive(false);

        // Loop though all tiles and reset them
        *//*for (int i = 0; i < tile2D.Length; i++)
        {
            for (int j = 0; j < tile2D.Length; j++)
            {
                tile2D[i, j].GetComponentInParent<TileController>().ResetTile();
            }
        }*//*
        switch (iDimensions)
        {
            case 2:
                for (int i = 0; i < tileList.Length; i++)
                {
                    tileList[i].GetComponentInParent<TileController>().ResetTile();
                }
                break;

            case 3:
                for (int i = 0; i < tileList.Length; i++)
                {
                    tileLeftList[i].GetComponentInParent<TileController>().ResetTile();
                    tileList[i].GetComponentInParent<TileController>().ResetTile();
                    tileRightList[i].GetComponentInParent<TileController>().ResetTile();
                }
                break;

            case 4:
                break;
        }
*/// some bug IDK
    }


    private void ToggleButtonState(bool state) // buttons' pressable
    {
        /*for (int i = 0; i < tile2D.Length; i++)
        {
            for (int j = 0; j < tile2D.Length; j++)
            {
                tile2D[i, j].GetComponentInParent<Button>().interactable = state; 
            }
        }*/

        for (int i = 0; i < tileList.Length; i++)
        {
            tileList[i].GetComponentInParent<Button>().interactable = state;
        }
    }

    public void BotModeToWhoPlayNow(int iWhoPlayNow)
    {
        whoIsBot[iWhoPlayNow] = (whoIsBot[iWhoPlayNow] == false) ? true : false;
        // up code same as code below
        // if (whoIsBot[iWhoPlayNow] == false) { whoIsBot[iWhoPlayNow] = true; } else { whoIsBot[iWhoPlayNow] = false; }
    }

    public void OnPlayerNameChanged(int intWhoPlayer)
    {
        playerName[intWhoPlayer] = playInputName[intWhoPlayer].text; // callback for when the P1_textfield is updated. We just update the string for Player1
    }

    public string GivePlayingTurn() { return playingTurn; } // give playingTurn to TileController
    public Sprite GiveTilePlay_iWhoPlayNow() { return tilePlay[iWhoPlayNow]; } // give tilePlay[iWhoPlayNow] to TileController

    public void Game3Dto2D()
    {
        foreach (var shit in game3DObject)
        { if (gameNow3D) { shit.SetActive(false); } else { shit.SetActive(true); }}
    }
}
