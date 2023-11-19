using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System.IO;
using UnityEngine.UI;

public class Workflow_info : MonoBehaviour
{
    DatabaseReference dbRef;
    private Database database;
    public Text Fio;
    public Text Score;
    private int PSCore;

    public class User{
        public string Fio;
        public string Email;
        public string PhoneNumber;
        public int Score;
        public bool[] MarksChecked;

        public User(string fio, string email, string phoneNumber, int score, bool[] MarksChecked)
        {   this.Fio = fio;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.Score = score;
            this.MarksChecked = MarksChecked;
        }
    }
   
    void Start()
    {  dbRef = FirebaseDatabase.DefaultInstance.RootReference;
       database = GetComponent<Database>();
       if (database == null)
        {   Debug.LogError("Компонент БазыДанных не найден на объекте " + gameObject.name);}
       
       if(File.Exists(Application.persistentDataPath + "/user.json")){
        string filepath = File.ReadAllText(Application.persistentDataPath + "/user.json");  // Its work with json!
        User local_user;
        local_user = JsonUtility.FromJson<User>(filepath);
        Fio.text = local_user.Fio;
        Score.text = local_user.Score.ToString();
              
                
              
                 




       }
       else {
        return;
       }
    }
public async void ScoreAdd(int i)
{
    string filepath = File.ReadAllText(Application.persistentDataPath + "/user.json");
    User local_user = JsonUtility.FromJson<User>(filepath);
  
    if (!local_user.MarksChecked[i])
    {
        DataSnapshot snapshot = await dbRef.Child("users").Child(local_user.PhoneNumber).Child("Score").GetValueAsync();
        int score = int.Parse(snapshot.Value.ToString());
         local_user.Score = score;

        local_user.Score = score + 10;
        local_user.MarksChecked[i] = true;
        Score.text = local_user.Score.ToString();

        File.WriteAllText(Application.persistentDataPath + "/user.json", JsonUtility.ToJson(local_user));

        await dbRef.Child("users").Child(local_user.PhoneNumber).Child("Score").SetValueAsync(local_user.Score.ToString());
        await dbRef.Child("users").Child(local_user.PhoneNumber).Child("MarksChecked").SetValueAsync(local_user.MarksChecked);
    }
    else
    {
        Debug.Log("It was scored");
    }
}


     



    public void ScoreAddRofl() {
            string filepath = File.ReadAllText(Application.persistentDataPath + "/user.json");  // Its work with json!
            User local_user;
            local_user = JsonUtility.FromJson<User>(filepath);

            local_user.Score = PSCore + 0 ;
            
      
        
    }




}
