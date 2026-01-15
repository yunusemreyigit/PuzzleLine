using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    static public FileManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void saveToJSON(dataObject saveObject, string fileName)
    {
        string json = JsonUtility.ToJson(saveObject);
        writeToFile(savingPath(fileName), json);
    }
    public dataObject loadFromJSON(string fileName)
    {
        string content = readFromFile(savingPath(fileName));
        if (String.IsNullOrEmpty(content) || content == "{}")
        {
            return new dataObject();
        }
        dataObject data = JsonUtility.FromJson<dataObject>(content);
        Debug.Log("Loaded from :" + savingPath(fileName));
        return data;
    }
    private string savingPath(string file)
    {
        return Application.persistentDataPath + "/" + file;
    }
    private void writeToFile(string path, string content)
    {
        FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
        if (!File.Exists(path)) return;
        using (StreamWriter streamWriter = new StreamWriter(fileStream))
        {
            streamWriter.WriteLine(content);
            streamWriter.Close();
        }

    }
    private string readFromFile(string path)
    {
        if (!File.Exists(path))
        {
            Debug.Log("File not found :" + path);
            return "";
        }
        string data = File.ReadAllText(path);
        return data;
    }

}
public class dataObject
{
    public int xp = 0;
    public int level = 0;

    public dataObject(int xp = 0, int level = 0)
    {
        this.xp = xp;
        this.level = level;
    }
}
