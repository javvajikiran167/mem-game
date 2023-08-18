using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardCtrl : MonoBehaviour
{
    public FlipCard flipCard;

    public Sprite[] faces;
    public Sprite back;
    public Image image;
    public Button cardButton;
    public int faceIndex;
    CreateGrid gridCtrl;

    private void Awake()
    {
        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        cardButton.interactable = false;
        flipCard.OpenCard(() => { image.sprite = faces[faceIndex]; }, () => { gridCtrl.SetOpenCard(this); });
    }

    public void CloseCard()
    {
        flipCard.CloseCard(() => { image.sprite = back; }, () => { cardButton.interactable = true; });
    }

    public void SetCard(CreateGrid gridCtrl, int faceIndex)
    {
        this.gridCtrl = gridCtrl;
        this.faceIndex = faceIndex;
        image.sprite = back;
    }
}