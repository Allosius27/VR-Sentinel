using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonPause : MonoBehaviour
{
    public Color notSelectedColor, selectedColor;

    [SerializeField] UnityEvent onTrigger;

    private void Start()
    {
        //GetComponent<Image>().color = notSelectedColor;
    }

    public void Trigger()
    {
            onTrigger.Invoke();
    }
}
