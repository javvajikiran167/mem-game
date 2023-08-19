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
        gridItems = createGridCtrl.CreateGrid(rows, columns, this);
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
                AddScore();
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

    private void AddScore()
    {
        score = score + 1;
        scoreText.text = "Score: " + score.ToString();
    }


    private void ResetVars()
    {
        score = 0;
        gameStatusText.text = string.Empty;
    }
}
