using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCtrl : MonoBehaviour
{
    [SerializeField] private CreateGridCtrl createGridCtrl;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] public TextMeshProUGUI gameStatusText;
    [SerializeField] public int rows, columns;

    private CardHolder[] gridItems;
    Queue<CardCtrl> openCards = new Queue<CardCtrl>();
    int score = 0;


    public void CreateGrid()
    {
        ResetVars();

        string data = PlayerPrefs.GetString("GameState");
        GameState gameState = JsonUtility.FromJson<GameState>(data);

        if (gameState!= null && gameState.isInProgress)
        {
            gridItems = createGridCtrl.CreateGridWithExistingState(gameState.rows, gameState.columns, this, gameState.faces);
            SetScore(gameState.score);
        }
        else
        {
            gridItems = createGridCtrl.CreateGrid(rows, columns, this);
        }
    }


    public void StartGame()
    {
        for (int i = 0; i < rows * columns; i++)
        {
            gridItems[i].cardCtrl.CloseCard();
        }
    }
   
    public void OnCardOpened(CardCtrl cardCtrl)
    {
        AudioCtrl.instance.PlayCardFlip();
        openCards.Enqueue(cardCtrl);
        CheckCardsMatching();
        CheckForGameEnd();
    }

    public void CheckCardsMatching()
    {
        if (openCards.Count >= 2)
        {
            CardCtrl card1 = openCards.Dequeue();
            CardCtrl card2 = openCards.Dequeue();
            if (card1.faceIndex == card2.faceIndex)
            {
                card1.gameObject.SetActive(false);
                card2.gameObject.SetActive(false);
                AudioCtrl.instance.PlayCardsMatched();
                SetScore(score + 1);
            }
            else
            {
                card1.CloseCard();
                card2.CloseCard();
                AudioCtrl.instance.PlayCardsMismatched();
            }
        }
    }

    private void CheckForGameEnd()
    {
        if (score == (rows * columns) / 2)
        {
            AudioCtrl.instance.PlayGameOver();
            gameStatusText.text = "Game Over";
        }
    }

    private void SetScore(int m_score)
    {
        score = m_score;
        scoreText.text = "Score: " + score.ToString();
    }

    private void ResetVars()
    {
        score = 0;
        gameStatusText.text = string.Empty;
    }



    [Serializable]
    public class GameState
    {
        public int rows, columns;
        public bool isInProgress;
        public int[] faces; //if index is -ve, then that is already matched;
        public int score;
    }

    void OnApplicationQuit()
    {
        SaveGameState();
    }

    public void SaveGameState()
    {
        GameState gameState = new GameState();
        gameState.rows = rows;
        gameState.columns = columns;
        gameState.score = score;
        int itemsCount = rows * columns;
        gameState.faces = new int[itemsCount];
        bool isInProgress = false;
        for (int i = 0; i < itemsCount; i++)
        {
            if (gridItems[i].cardCtrl.isActiveAndEnabled)
            {
                gameState.faces[i] = gridItems[i].cardCtrl.faceIndex;
                isInProgress = true;
            }
            else
            {
                gameState.faces[i] = -1;
            }
        }

        gameState.isInProgress = isInProgress;

        PlayerPrefs.SetString("GameState", JsonUtility.ToJson(gameState));
    }
}
