using System.IO;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem INSTANCE = null;
    private string filePath;
    void Start()
    {
        if (INSTANCE != null){
            Destroy(gameObject);
        }
        INSTANCE = this;
        filePath = Path.Combine(Application.persistentDataPath, "saveData.json");
    }
    
    /// <summary>
    /// Saves Current progrees of player into the game data
    /// </summary>
    /// <param name="data">GameData with the loaded data points</param>
    public void SaveGameData(GameData data){
        string jsonData = JsonUtility.ToJson(data);

        // Adding hashing security to serialised data
        string hash = ComputeSHA256Hash(jsonData);

        // Combine data with hash so that comparisons can be made
        string saveData = jsonData + "\n" + hash;
        
        // Write JSON data to file path
        File.WriteAllText(filePath, jsonData);
    }

    /// <summary>
    /// Loads game data from save file and unpackages it
    /// 
    /// NOTE: Checks if data has been tempered with out of play
    /// </summary>
    /// <returns>Unpackaged Data Format</returns>
    public GameData LoadGameData(){
        if (!File.Exists(filePath)){
            Debug.Log("Unable to find the file path, Save file may not have been created yet");
            return null;
        }

        // Read in file
        string jsonData = File.ReadAllText(filePath);

        // Split Data with Hashed data
        string[] parts = jsonData.Split('\n');

        if (parts.Length == 2){
            string saveData = parts[0];
            string savedHash = parts[1];

            string computedHash = ComputeSHA256Hash(saveData);

            if (computedHash == savedHash){
                return JsonUtility.FromJson<GameData>(saveData);
            }
            else {
                Debug.LogError("Saved Data has been tempered with!");
            }
        }
        else{
            Debug.LogError("Something is wrong with the save format");
        }

        return null;        
    }

    /// <summary>
    /// Compute SHA256 hash of an input
    /// </summary>
    /// <param name="input">Input to be hashed</param>
    /// <returns>Hashed Output</returns>
    private string ComputeSHA256Hash(string input){
        using (SHA256 sha256 = SHA256.Create()){
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            StringBuilder builder = new StringBuilder();

            foreach (byte b in hashBytes){
                builder.Append(b.ToString("x2")); // Convert to hexadecimal
            }

            return builder.ToString();
        }
    }
}
