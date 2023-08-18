using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        gridItems = new List<GameObject>();
    }

    private List<string> matchingItem = new List<string>()
    {
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
    };


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
            InitCard(matchingItem[i]);
            InitCard(matchingItem[i]);
        }

        gridItems.Shuffle();

        for (int i = 0; i < totalItems; i++)
        {
            gridItems[i].transform.SetParent(gridParent.transform);
        }

    }

    private void InitCard(string value)
    {
        GameObject item = Instantiate(gridItem);
        CardCtrl cardCtrl = item.GetComponent<CardCtrl>();
        cardCtrl.SetCard(value);
        gridItems.Add(item);
    }
}