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

    public int rows;
    public int columns;
    private List<GameObject> gridItems;

    Queue<CardCtrl> openCards = new Queue<CardCtrl>();

    int score = 0;

    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        gridItems = new List<GameObject>();
    }

    public void CreateGridM()
    {
        int totalItems = rows * columns;
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
        gridItems.Add(item);
    }

    public void SetOpenCard(CardCtrl cardCtrl)
    {
        openCards.Enqueue(cardCtrl);
        ValidateCardsMatched();
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
                AddScore();
            }
            else
            {
                card1.CloseCard();
                card2.CloseCard();
            }
        }
    }

    private void AddScore()
    {
        score = score + 1;
        scoreText.text = "Score: " + score.ToString();
    }
}