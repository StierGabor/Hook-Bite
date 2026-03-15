using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public static class SaveSystem
{
    public static IEnumerator UpdateScore(
        int userId,
        int penz,
        int bot,
        int halak,
        int csalik,
        string bearerToken = null // ha később auth kell
    )
    {
        // FONTOS: {id} helyére a valós ID kerü
        string url = $"http://127.0.0.1:8000/api/users/{userId}/update-score";

        // Laravel validátorhoz illeszkedő JSON payload
        var payload = new Payload
        {
            penz = penz,
            bot = bot,
            halak = halak,
            csalik = csalik
        };

        string jsonData = JsonUtility.ToJson(payload);

        // PUT kérés JSON-nal
        var request = new UnityWebRequest(url, "PUT");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");

        // Ha később auth-otok van (Bearer token), itt küldd:
        if (!string.IsNullOrEmpty(bearerToken))
        {
            request.SetRequestHeader("Authorization", $"Bearer {bearerToken}");
        }

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"[SaveSystem] Sikeres PUT: {request.downloadHandler.text}");
        }
        else
        {
            // Laravel validation hibák esetén is 422-t kapsz → itt olvasható a hibatest
            Debug.LogError($"[SaveSystem] Hiba: {request.responseCode} | {request.error} | Body: {request.downloadHandler.text}");  
        }
    }

    // Külön típus, hogy JsonUtility biztosan tudja szerializálni
    [System.Serializable]
    private class Payload
    {
        public int penz;
        public int bot;
        public int halak;
        public int csalik;
    }
}