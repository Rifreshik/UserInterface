using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Android;
using System.IO;

public class Levels : MonoBehaviour
{

    void Start(){
         if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

         if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
    }

    public void ChangeLevel(string x) {
        SceneManager.LoadScene(x);
    }

    public void ChangeLevelAfterRegistr() {
        if(File.Exists(Application.persistentDataPath + "/user.json")){
            SceneManager.LoadScene("Workflow");
        }
        else {
            SceneManager.LoadScene("Registration");
        }
        
    }
}
