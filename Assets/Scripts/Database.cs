using UnityEngine;
using Firebase.Database;
using UnityEngine.UI;
using System.IO;
public class Database : MonoBehaviour
{
    DatabaseReference dbRef;
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

    void Start(){
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SaveData(string fio, string email, string phoneNumber)
    {   if (dbRef == null)
        {   Debug.LogError("dbRef is null");
            return;}

        bool[] MarksChecked = new bool[]{false, false, false, false, false, false, false, false, false, false};
        
        User user = new User(fio, email, phoneNumber, 0, MarksChecked);
        string json = JsonUtility.ToJson(user);
        File.WriteAllText(Application.persistentDataPath + "/user.json", json);
        dbRef.Child("users").Child(phoneNumber).SetRawJsonValueAsync(json);
    }

    
}