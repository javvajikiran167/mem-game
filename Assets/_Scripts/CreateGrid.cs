using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateGrid : MonoBehaviour
{
    public GameObject gridParent;
    public GridLayoutGroup gridLayout;
    public GameObject gridItem;

    public int rows,columns;
    
    private List<CardHolder> gridItems;

    Queue<CardCtrl> openCards = new Queue<CardCtrl>();

    int score = 0;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameStatusText;

    private void Awake()
    {
        gridItems = new List<CardHolder>();
        gameStatusText.text = string.Empty;
    }

    #region Create Grid
    public void CreateGridM()
    {
        int totalItems = rows * columns;
        if (totalItems % 2 != 0)
        {
            Debug.Log("Can't Create Grid with given Row and Columns");
            return;
        }
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayout.constraintCount = rows;

        float spaceXY = 20; 
        float gridW = gridParent.GetComponent<RectTransform>().sizeDelta.x;
        float gridH = gridParent.GetComponent<RectTransform>().sizeDelta.y;

        float itemW = (gridW - (rows * spaceXY)) / rows;
        float itemH = (gridH - (columns * spaceXY)) / columns;
        float finalItemWH = Math.Min(itemW, itemH);
        gridLayout.cellSize = new Vector2(finalItemWH, finalItemWH);
        gridLayout.spacing = new Vector2(spaceXY, spaceXY);


        for (int i = 0; i < totalItems / 2; i++)
        {
            InitCard(i);
            InitCard(i);
        }

        gridItems.Shuffle();

        for (int i = 0; i < totalItems; i++)
        {
            gridItems[i].transform.SetParent(gridParent.transform);
            gridItems[i].cardCtrl.OpenCardBeforeGameStart();
        }
    }

    private void InitCard(int faceIndex)
    {
        GameObject item = Instantiate(gridItem);
        CardHolder cardHolder = item.GetComponent<CardHolder>();
        cardHolder.cardCtrl.SetCard(this, faceIndex);
        gridItems.Add(cardHolder);
    }
    #endregion


    public void StartGame()
    {
        for (int i = 0; i < rows * columns; i++)
        {
            gridItems[i].cardCtrl.CloseCard();
        }
    }


    public void SetOpenCard(CardCtrl cardCtrl)
    {
        AudioCtrl.instance.PlayCardFlip();
        openCards.Enqueue(cardCtrl);
        ValidateCardsMatched();

        ValidateGameEnd();
    }

    private void ValidateGameEnd()
    {
        if (score == (rows * columns) / 2)
        {
            AudioCtrl.instance.PlayGameOver();
            gameStatusText.text = "Game Over";
        }
    }

    public void ValidateCardsMatched()
    {
        if(openCards.Count >=2)
        {
            CardCtrl card1 = openCards.Dequeue();
            CardCtrl card2 = openCards.Dequeue();

            Debug.Log("card1: " + card1.faceIndex + ", card2: " + card2.faceIndex);
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

    private void AddScore()
    {
        score = score + 1;
        scoreText.text = "Score: " + score.ToString();
    }

   
}