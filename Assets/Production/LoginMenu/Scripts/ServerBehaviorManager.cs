using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Security.Cryptography;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ServerBehaviorManager : MonoBehaviour
{

    private static ServerBehaviorManager instance;

    public static ServerBehaviorManager Instance    
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ServerBehaviorManager>();
            }
            return instance;
        }
    }
    /*******************************************************************************************************************/
    public InputField formUsername;
    public InputField formPassword;
    public InputField registerFormEmail;
    public InputField registerFormUsername;
    public InputField registerFormPassword;
    public InputField registerFormPassValidate;
    public GameObject errorMessageMenu;
    public Text errorMessageText;

    public string _authURL;
    public string _getURL;
    public string _postURL;
    public string _postDataURL;

    [SerializeField]
    private GameObject levelLoader;

    /*******************************************************************************************************************/
    /*******************************************************************************************************************/
    public PlayerDataClass playerData = new PlayerDataClass();
    public PlayerStatsClass playerStatistics = new PlayerStatsClass();
    //public ScoresMainClass[] PlayerScores;
    /*******************************************************************************************************************/

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    #region Server Communication Subroutines
    IEnumerator _GetAuthorization(string _username, string _password)
    {
        string getUrl = _authURL;
       
        WWWForm authForm = new WWWForm();
        authForm.AddField("username", _username);
        authForm.AddField("password", _password);
        authForm.AddField("action", "authorize");
        UnityWebRequest www = UnityWebRequest.Post(getUrl,authForm);

        yield return www.Send();

        if (www.isError)
        {
            //Debug.Log(www.error);
            string errorReportingMessage = "Oops. Something went wrong. (error 0x000-Connection Error)";
            ShowErrorMessage(errorReportingMessage);
        }
        else
        {
            string jsnData = www.downloadHandler.text;
            //Debug.Log(jsnData);
            if (jsnData == "0x000")
            {
                string errorReportingMessage = "Player authentication Error - 0x000";
                ShowErrorMessage(errorReportingMessage);
            }
            else
            {
                playerData = JsonUtility.FromJson<PlayerDataClass>(jsnData);
                StartCoroutine(_GetPlayerStats(playerData.playerHash));
            }
            
        }
    }

    
    IEnumerator _GetPlayerStats(string playerHash)
    {
        string postURL = _authURL;
        Debug.Log("Getting Player Data");
        WWWForm playerStatsForm = new WWWForm();
        playerStatsForm.AddField("action", "get_statistics");
        playerStatsForm.AddField("playerhash", playerHash);
        UnityWebRequest www = UnityWebRequest.Post(postURL, playerStatsForm);
        yield return www.Send();

        if (www.isError)
        {
            Debug.Log(www.error);
            string errorReportingMessage = "Cannot contact Server";
            ShowErrorMessage(errorReportingMessage);
        }
        else
        {
            //Debug.Log("GET complete!");
            //Debug.Log(www.downloadHandler.text);
            string jsnData = www.downloadHandler.text;
            //Debug.Log("jsn: " + jsnData);
            if (jsnData == "")
            {
                string errorReportingMessage = "Oops. Something went wrong. (error 0x001PlayerStats)";
                ShowErrorMessage(errorReportingMessage);
            }
            else if (jsnData == "0x0PS")
            {
                string errorReportingMessage = "Oops. Something went wrong. (error 0x001PlayerStats)";
                ShowErrorMessage(errorReportingMessage);
            }
            else
            {
                playerStatistics = JsonUtility.FromJson<PlayerStatsClass>(jsnData);
                levelLoader.GetComponent<LevelLoader>().LoadLevel(1);
            }
            
        }
    }
    /*******************************************************************************************************************/
    /*IEnumerator _RetrieveAllHighScores()
    {
        string postURL = _getURL;
        Debug.Log("Getting Player Data");
        WWWForm playerScoresForm = new WWWForm();
        playerScoresForm.AddField("uni_action", "retrieveallscores");
        UnityWebRequest www = UnityWebRequest.Post(postURL, playerScoresForm);
        yield return www.Send();

        if (www.isError)
        {
            Debug.Log(www.error);
            string errorReportingMessage = "Cannot contact Server";
            ShowErrorMessage(errorReportingMessage);
        }
        else
        {
            //Debug.Log("GET complete!");
            //Debug.Log(www.downloadHandler.text);
            string jsnData = www.downloadHandler.text;
            //Debug.Log("jsn: " + jsnData);
            if (jsnData == "")
            {
                string errorReportingMessage = "Oops. Something went wrong. (error 0x001PlayerStats)";
                ShowErrorMessage(errorReportingMessage);
            }
            else if (jsnData == "0x0SC")
            {
                string errorReportingMessage = "Oops. Something went wrong. (error 0x001PlayerStats)";
                ShowErrorMessage(errorReportingMessage);
            }
            else
            {
                PlayerScores[0] = JsonUtility.FromJson<ScoresMainClass>(jsnData);
                //levelLoader.GetComponent<LevelLoader>().LoadLevel(1);
            }

        }
    }*/
    /*******************************************************************************************************************/
    IEnumerator _PostRegisterUser(string username, string password, string email, string playerHash)
    {
        PlayerDataClass postJsnData = new PlayerDataClass();
        postJsnData.username = username;
        postJsnData.password = password;
        postJsnData.email = email;
        postJsnData.playerHash = playerHash;
        string postJsnDataEncode = JsonUtility.ToJson(postJsnData);
        Debug.Log("Sending JSON: " + postJsnDataEncode);
        
        WWWForm form = new WWWForm();
        form.AddField("playerData", postJsnDataEncode);
        string postUrl = _postURL;
        UnityWebRequest www = UnityWebRequest.Post(postUrl, form);
        yield return www.Send();
        if (www.isError)
        {
            Debug.Log(www.error);
            string errorReportingMessage = "Cannot contact Authentication Server";
            ShowErrorMessage(errorReportingMessage);
        }
        else
        {
            Debug.Log("Form upload complete! "+www.downloadHandler.text);
        }
    }

    /*******************************************************************************************************************/

    IEnumerator _SendScoresToServer(string _username, string _playerHash, int _gold, int _kills, float _survTime)
    {
        WWWForm form = new WWWForm();
        form.AddField("uni_action", "setScores");
        form.AddField("uni_username", _username);
        form.AddField("uni_playerHash", _playerHash);
        form.AddField("uni_gold", _gold);
        form.AddField("uni_kills", _kills);
        form.AddField("uni_survivalTime", _survTime.ToString());

        string postUrl = _postDataURL;
        Debug.Log("Sending Scores to:" + postUrl + " data:"+_username +" "+_gold + " "+_kills);
        UnityWebRequest www = UnityWebRequest.Post(postUrl, form);
        yield return www.Send();
        if (www.isError)
        {
            Debug.Log(www.error);
            string errorReportingMessage = "Cannot contact Community Server";
            ShowErrorMessage(errorReportingMessage);
        }
        else
        {
            Debug.Log("Form upload complete! " + www.downloadHandler.text);
        }
    }
    #endregion

    #region Server Calling Functions
    public void SendUsernamePassword()
    {
        string encryptedPassword = EncryptPassword(formPassword.text);
        StartCoroutine(_GetAuthorization(formUsername.text, encryptedPassword));
    }

    public void RegisterUser()
    {
        string encryptedPassword = EncryptPassword(registerFormPassword.text);
        string newPlayerHash = CreatePlayerHash();
        StartCoroutine(_PostRegisterUser(registerFormUsername.text, encryptedPassword, registerFormEmail.text, newPlayerHash));
    }

    public void SendScores(string playerUsername, string playerHash, int gold, int kills, float survTime)
    {
        StartCoroutine(_SendScoresToServer(playerUsername, playerHash, gold, kills, survTime));
    }

    public void RetrieveAllHighScores()
    {

    }

    public void RetrievePlayerHighScores(string _playerHash)
    {

    }
    #endregion

    #region Generic Functions
    public string EncryptPassword(string passwordText)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(passwordText);
        SHA256Managed hashstring = new SHA256Managed();
        byte[] hash = hashstring.ComputeHash(bytes);
        string hashString = string.Empty;
        foreach (byte x in hash)
        {
            hashString += string.Format("{0:x2}", x);
        }
        //Debug.Log(hashString);
        return hashString;
    }

    private string GetTimeStamp(System.DateTime value)
    {
        return value.ToString("yyyyMMddHHmmssffff");
    }

    private string CreatePlayerHash()
    {
        string playerHash ="";
        string timestamp = GetTimeStamp(System.DateTime.Now);
        playerHash = registerFormUsername.text + "&" + timestamp.ToString();
        return playerHash;
    }

    private bool CompareStrings(string strA, string strB)
    {
        if (strA == strB)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ShowErrorMessage(string errorMessageString)
    {
        errorMessageMenu.SetActive(true);
        errorMessageText.text = errorMessageString;
    }
    #endregion
}


