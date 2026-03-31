using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public static class SaveSystem
{
    public static IEnumerator UpdateScore(
        int userId,
        int gold,
        int rod,
        int bream,
        int catfish,
        int ray,
        int octopus,
        int lure,
        string bearerToken = null // ha később auth kell
    )
    {
        // FONTOS: {id} helyére a valós ID kerü
        string url = $"http://127.0.0.1:8000/api/users/{userId}/update-score";

        // Laravel validátorhoz illeszkedő JSON payload
        var payload = new Payload
        {
            gold = gold,
            rod = rod,
            bream = bream,
            catfish = catfish,
            ray = ray,
            octopus = octopus,
            lure = lure
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
            // Laravel validation hibák esetén is 422-t kapsz → itt olvasható a errortest
            Debug.LogError($"[SaveSystem] error: {request.responseCode} | {request.error} | Body: {request.downloadHandler.text}");  
        }
    }

    // Külön típus, hogy JsonUtility biztosan tudja szerializálni
    [System.Serializable]
    private class Payload
    {
        public int gold;
        public int rod;
        public int bream;
        public int catfish;
        public int ray;
        public int octopus;
        public int lure;
    }
}