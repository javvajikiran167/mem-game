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
    public int faceIndex;


    public void OnClick()
    {
        image.sprite = image.sprite == back ? faces[0] : back;
    }

    public void SetCard(int faceIndex)
    {
        image.sprite = faces[faceIndex];
    }
}