using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class ButtonController : MonoBehaviour
{
    public InputField fio;
    public InputField email;
    public InputField phoneNumber;
    public bool[] CheckMarks = new bool[]{false, false, false, false, false, false, false, false, false, false};
    private Database database;
   void Start()
    {   database = GetComponent<Database>();
        if (database == null)
        {   Debug.LogError("Компонент БазыДанных не найден на объекте " + gameObject.name);}
    }

   public void SaveDataButton()
    {   if (fio == null || email == null || phoneNumber == null)
        {   Debug.LogError("Не все поля настроены в инспекторе.");
            return;}
        else {
            database.SaveData(fio.text, email.text, phoneNumber.text);
        }
    }



}
