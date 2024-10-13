using Newtonsoft;
using Newtonsoft.Json;
using UnityEditor.Rendering;
using UnityEngine;

public class FileManager
{
    static public T LoadData<T>(string fileName)
    {
        string jsonFile = Resources.Load("Jsons/" + fileName).ToString();
        T data = JsonConvert.DeserializeObject<T>(jsonFile);
        return data;
    }
}