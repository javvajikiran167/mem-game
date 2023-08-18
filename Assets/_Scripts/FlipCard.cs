using System.Collections;
using UnityEngine;

public class FlipCard : MonoBehaviour
{
    public GameObject cardBack;
    public bool isCardFaceActive;

    // Start is called before the first frame update
    void Start()
    {
        isCardFaceActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FlipTheCard(!isCardFaceActive));
        }
    }


    private void Flip()
    {
        StartCoroutine(FlipTheCard(!isCardFaceActive));
    }


    private void DisplayFaceOrBack()
    {
        if (isCardFaceActive)
        {
            cardBack.SetActive(true);
            isCardFaceActive = false;
        }
        else
        {
            cardBack.SetActive(false);
            isCardFaceActive = true;
        }
    }


    IEnumerator FlipTheCard(bool shouldDisplayFaceOfCard)
    {
        yield return new WaitForSeconds(0.5f);
        float fromAngle = shouldDisplayFaceOfCard ? 0 : 180;
        float targetAngle = shouldDisplayFaceOfCard ? 180 : 0;

        float t = 0;
        bool uncovered = false;

        while (t < 1f)
        {
            t += Time.deltaTime * 2; ;

            float angle = Mathf.LerpAngle(fromAngle, targetAngle, t);
            transform.eulerAngles = new Vector3(0, angle, 0);
            Debug.Log("fromAngle: " + fromAngle+  ",tagetAngle: " + targetAngle+ ", angle: " + angle);
            if (((shouldDisplayFaceOfCard && (angle >= 90 && angle < 180)) ||
                (!shouldDisplayFaceOfCard && (angle > 270 && angle < 360))) && !uncovered)
            {
                uncovered = true;
                DisplayFaceOrBack();
            }
            yield return null;
        }
    }
}