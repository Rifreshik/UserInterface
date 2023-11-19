using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Agreement_btn : MonoBehaviour
{
    public Toggle toggle;
    public Button button;


    public void SomeVoid(bool isOn)
    {
        if (isOn == true) {
            button.GetComponent<Button>().interactable = true;
        }
        else {
            button.GetComponent<Button>().interactable = false;
        }
    }  
}
