using System;
using UnityEngine;
using UnityEngine.UI;

public class CreateGridCtrl : MonoBehaviour
{
    public GameObject gridParent;
    public GridLayoutGroup gridLayout;
    public GameObject gridItem;

    public CardHolder[] CreateGrid(int rows, int columns, GameCtrl gameCtrl)
    {
        int totalItems = rows * columns;
        if (totalItems % 2 != 0)
        {
            Debug.Log("Can't Create Grid with given Row and Columns");
            return null;
        }

        CardHolder[] gridItems = new CardHolder[totalItems];
        SetGridProperties(rows, columns);

        //Create 2 card of same face index
        for (int i = 0; i < totalItems / 2; i++)
        {
            gridItems[2*i] = CreateGridItem(i, gameCtrl);
            gridItems[2*i+1] = CreateGridItem(i, gameCtrl);
        }
        gridItems.Shuffle();

        for (int i = 0; i < totalItems; i++)
        {
            gridItems[i].transform.SetParent(gridParent.transform);
            gridItems[i].cardCtrl.OpenCardBeforeGameStart();
        }
        return gridItems;
    }

    public CardHolder[] CreateGridWithExistingState(int rows, int columns, GameCtrl gameCtrl, int[] faceIndexes)
    {
        int totalItems = rows * columns;
        CardHolder[] gridItems = new CardHolder[totalItems];
        SetGridProperties(rows, columns);

        //Create 2 card of same face index
        for (int i = 0; i < totalItems; i++)
        {
            int faceIndex = faceIndexes[i];

            if (faceIndex >= 0) {
                gridItems[i] = CreateGridItem(faceIndexes[i], gameCtrl);
            }
            else
            {
                gridItems[i] = CreateGridItem(0, gameCtrl);
                gridItems[i].cardCtrl.gameObject.SetActive(false);
            }

            gridItems[i].transform.SetParent(gridParent.transform);
        }

        return gridItems;
    }

    private void SetGridProperties(int rows, int columns)
    {
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
    }

    private CardHolder CreateGridItem(int faceIndex, GameCtrl gameCtrl)
    {
        GameObject item = Instantiate(gridItem);
        CardHolder cardHolder = item.GetComponent<CardHolder>();
        cardHolder.cardCtrl.SetCard(gameCtrl, faceIndex);
        return cardHolder;
    }
}