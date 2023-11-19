using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using Firebase.Database;

public class Log_out : MonoBehaviour
{
    public Text Fio;
    public Text Score;
    public Text CheckedMarks;
  
    public int ScoreON;
    



    public class User{
        public string Fio;
        public string Email;
        public string PhoneNumber;
        public int Score;
        public bool[] MarksChecked;
    }

        DatabaseReference dbRef;

    
    void Start(){
         dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        string filepath = File.ReadAllText(Application.persistentDataPath + "/user.json");  // Its work with json!
        User local_user = JsonUtility.FromJson<User>(filepath);
        Fio.text = local_user.Fio;
        Score.text= local_user.Score.ToString();
      


        int j = 0;
        for (int i=0; i < 10; i++){
            if(local_user.MarksChecked[i] == true){
                j++;
            }
        CheckedMarks.text = j.ToString();
        }
    }
    
    public void LogOutNow(){
        if(File.Exists(Application.persistentDataPath + "/user.json")){
        File.Delete(Application.persistentDataPath + "/user.json");
       }
       SceneManager.LoadScene("MainMenu");
    }




}

