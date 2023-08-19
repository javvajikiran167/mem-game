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

    public int totalItems;
    private List<CardCtrl> gridItems;

    Queue<CardCtrl> openCards = new Queue<CardCtrl>();

    int score = 0;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameStatusText;

    private void Awake()
    {
        gridItems = new List<CardCtrl>();
        gameStatusText.text = string.Empty;
    }

    #region Create Grid
    public void CreateGridM()
    {
        if (totalItems % 2 != 0)
        {
            Debug.Log("Can't Create Grid with given Row and Columns");
            return;
        }

        for (int i = 0; i < totalItems / 2; i++)
        {
            InitCard(i);
            InitCard(i);
        }

        gridItems.Shuffle();

        for (int i = 0; i < totalItems; i++)
        {
            gridItems[i].transform.SetParent(gridParent.transform);
            gridItems[i].OpenCardBeforeGameStart();
        }

        StartCoroutine(DisableLayout());
    }

    IEnumerator DisableLayout()
    {
        yield return new WaitForSeconds(1);
        gridLayout.enabled = false;

    }

    private void InitCard(int faceIndex)
    {
        GameObject item = Instantiate(gridItem);
        CardCtrl cardCtrl = item.GetComponent<CardCtrl>();
        cardCtrl.SetCard(this, faceIndex);
        gridItems.Add(cardCtrl);
    }
    #endregion


    public void StartGame()
    {
        for (int i = 0; i < totalItems; i++)
        {
            gridItems[i].CloseCard();
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
        if (score == totalItems / 2)
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
                Destroy(card1.gameObject);
                Destroy(card2.gameObject);
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