using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    Image buttonImage;

    private void Start()
    {
        buttonImage = GetComponent<Image> ();
    }

    public void SetSelected(bool selected)
    {
        if (selected) {
            buttonImage.color = new Color(0.8f, 0.8f, 0.8f);
        }else {
            buttonImage.color = Color.white;
        }
    }
}
