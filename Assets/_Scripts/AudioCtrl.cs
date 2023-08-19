using UnityEngine;

public class AudioCtrl : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip cardFlip, cardsMatched, cardsMismatch, gameOver;


    public static AudioCtrl instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void PlayCardFlip()
    {
        PlayClip(cardFlip);
    }

    public void PlayCardsMatched()
    {
        PlayClip(cardsMatched);
    }

    public void PlayCardsMismatched()
    {
        PlayClip(cardsMismatch);
    }

    public void PlayGameOver()
    {
        PlayClip(gameOver);
    }

    private void PlayClip(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
