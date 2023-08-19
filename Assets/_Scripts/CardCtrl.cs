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
    GameCtrl gameCtrl;

    private void Awake()
    {
        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        cardButton.interactable = false;
        flipCard.OpenCard(() => { image.sprite = faces[faceIndex]; }, () => { gameCtrl.OnCardOpened(this); });
    }

    public void CloseCard()
    {
        flipCard.CloseCard(() => { image.sprite = back; }, () => { cardButton.interactable = true; });
    }

    public void OpenCardBeforeGameStart()
    {
        cardButton.interactable = false;
        flipCard.OpenCard(() => { image.sprite = faces[faceIndex]; }, null);
    }

    public void SetCard(GameCtrl gameCtrl, int faceIndex)
    {
        this.gameCtrl = gameCtrl;
        this.faceIndex = faceIndex;
        image.sprite = back;
    }
}