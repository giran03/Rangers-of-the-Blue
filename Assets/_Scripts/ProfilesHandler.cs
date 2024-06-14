using System;
using UnityEngine;
using UnityEngine.UI;

public class ProfilesHandler : MonoBehaviour
{
    [SerializeField] InputField inputField_name;
    [SerializeField] InputField inputField_age;
    string _name;
    int _age;

    private void Awake()
    {

    }

    private void Update()
    {
        
    }

    public void Button_CreateProfile() => CreateProfile(_name, _age);

    public void CreateProfile(string name, int age)
    {
        
    }

    public void PrintProfile()
    {
        // string jsonString = PlayerPrefs.GetString("profiles");
        // Debug.Log($"Profiles: {jsonString}");
    }

    [Serializable]
    private class ProfileEntry
    {
        public string name;
        public int age;
    }
}

