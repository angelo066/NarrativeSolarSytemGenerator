using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using System;
using System.Text;
using System.IO;

/// <summary>
/// THIS CODE BELONGS TO THE USER: Unity Adventure in youtube. Extracted from his video: ¿Cómo usar ChatGPT en Unity? - OpenAI API
/// </summary>

public class ChatGpt_Communnication : MonoBehaviour
{
    //You can get your key here: https://platform.openai.com/api-keys
    [SerializeField] private string APIKey;
    [SerializeField] private string prompt;
    [SerializeField] private string result;

    private readonly string chatGPTUrlApi = "https://api.openai.com/v1/completions";

    RequestBodyChatGPT request;
    ResponseBodyChatGPT responseBodyChatGPT;

    public void sendRequest()
    {
        NetworkReachability reachability = Application.internetReachability;

        //if(reachability == NetworkReachability.NotReachable)
        //{
        //    Debug.Log("You need internet connection");
        //}
        //else if(reachability == NetworkReachability.ReachableViaCarrierDataNetwork || reachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        //{
        //    Debug.Log("You are connected!");
        //}

        result = string.Empty;

        request = new RequestBodyChatGPT();
        request.model = "text-davinci-003";
        request.prompt = prompt;
        request.max_tokens = 2048;
        request.temperature = 0;

        StartCoroutine(SendRequestAPI());
    }

    private IEnumerator SendRequestAPI()
    {
        string jsonData = JsonUtility.ToJson(request);
        Debug.Log(jsonData);
        byte[] rawData = Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest requestChatgpt = new UnityWebRequest(chatGPTUrlApi, "POST");

        requestChatgpt.uploadHandler = new UploadHandlerRaw(rawData);

        requestChatgpt.downloadHandler = new DownloadHandlerBuffer();

        //-H "Content-Type: application/json" \
        //-H "Authorization: Bearer $OPENAI_API_KEY" \

        requestChatgpt.SetRequestHeader("Content-Type", "application/json");
        requestChatgpt.SetRequestHeader("Authorization", "Bearer " + APIKey);

        result = "Loading...";


        yield return requestChatgpt.SendWebRequest();

        if (requestChatgpt.result == UnityWebRequest.Result.Success)
        {
            responseBodyChatGPT = JsonUtility.FromJson<ResponseBodyChatGPT>(requestChatgpt.downloadHandler.text);
            result = responseBodyChatGPT.choices[0].text;
        }
        else
        {
            result = "Error: " + requestChatgpt.result;
            Debug.Log("Response Code: " + requestChatgpt.responseCode);
            Debug.Log("Response: " + requestChatgpt.downloadHandler.text);

        }

        requestChatgpt.Dispose();
    }

    public void clear()
    {
        prompt = string.Empty;
        result = string.Empty;
    }

    //Necesary JSON to communicate with chatgpt
    [Serializable]
    public class RequestBodyChatGPT
    {
        public string model;
        public string prompt;
        public int max_tokens;
        public int temperature; //Determinism of the model
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    [Serializable]
    public class ResponseBodyChatGPT
    {
        public string id;
        public string @object;
        public int created;
        public string model;
        public string system_fingerprint;
        public List<Choice> choices;
        public Usage usage;

        [Serializable]
        public class Choice
        {
            public string text;
            public int index;
            public object logprobs;
            public string finish_reason;
        }
        [Serializable]
        public class Usage
        {
            public int prompt_tokens;
            public int completion_tokens;
            public int total_tokens;
        }
    }
}


