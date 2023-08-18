using System;
using System.Collections;
using UnityEngine;

public class FlipCard : MonoBehaviour
{

    public IEnumerator FlipTheCard(bool shouldOpen, Action halfFlipCompleted, Action flipCompleted)
    {
        yield return new WaitForSeconds(0.5f);
        float fromAngle = shouldOpen ? 0 : 180;
        float targetAngle = shouldOpen ? 180 : 0;

        float t = 0;
        bool uncovered = false;

        while (t < 1f)
        {
            t += Time.deltaTime * 3;

            float angle = Mathf.LerpAngle(fromAngle, targetAngle, t);
            transform.eulerAngles = new Vector3(0, angle, 0);
            //Debug.Log("fromAngle: " + fromAngle+  ",tagetAngle: " + targetAngle+ ", angle: " + angle);
            if (((shouldOpen && (angle >= 90 && angle < 180)) ||
                (!shouldOpen && (angle > 270 && angle < 360))) && !uncovered)
            {
                uncovered = true;
                halfFlipCompleted();
            }
            yield return null;
        }

        flipCompleted();
    }

    public void OpenCard(Action halfFlipCompleted, Action flipCompleted)
    {
        StartCoroutine(FlipTheCard(true, halfFlipCompleted, flipCompleted));
    }

    public void CloseCard(Action halfFlipCompleted, Action flipCompleted)
    {
        StartCoroutine(FlipTheCard(false, halfFlipCompleted, flipCompleted));
    }
}