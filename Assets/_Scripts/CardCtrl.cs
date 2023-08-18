using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardCtrl : MonoBehaviour
{
    public FlipCard flipCard;
    public TextMeshProUGUI cardLetter;

    public void SetCard(string value)
    {
        cardLetter.text = value;
    }
}
