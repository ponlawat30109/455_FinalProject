using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class DatabaseConnection : MonoBehaviour
{
    public static DatabaseConnection instance;
    private WebSocket websocket;
    private string tempMessageString;

    [SerializeField] InputField loginUsernameInput;
    [SerializeField] InputField loginPasswordInput;
    [SerializeField] InputField usernameInput;
    [SerializeField] InputField passwordInput;
    [SerializeField] InputField playernameInput;
    [SerializeField] Dropdown heroSelect;

    [SerializeField] Button loginButton;
    [SerializeField] Button registerButton;

    [SerializeField] GameObject ErrPanel;
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject registerPanel;

    [HideInInspector] public string playerName;
    [HideInInspector] public string heroName;

    struct LoginData
    {
        public string username;
        public string password;
        public LoginData(string _username, string _password)
        {
            username = _username;
            password = _password;
        }
    }

    struct RegisterData
    {
        public string username;
        public string password;
        public string playername;
        public string hero;
        public RegisterData(string _username, string _password, string _playername, string _hero)
        {
            username = _username;
            password = _password;
            playername = _playername;
            hero = _hero;
        }
    }

    struct UserData
    {
        public string _id;
        public string username;
        public string password;
        public string playername;
        public string heroname;
        public UserData(string _id, string username, string password, string playername, string heroname)
        {
            this._id = _id;
            this.username = username;
            this.password = password;
            this.playername = playername;
            this.heroname = heroname;
        }
    }

    struct SocketEvent
    {
        public string eventName;
        public string data;
        public string status;

        public SocketEvent(string _eventName, string _data, string _status)
        {
            eventName = _eventName;
            data = _data;
            status = _status;
        }
    }

    private void Awake()
    {
        instance = this;
        // DontDestroyOnLoad(this);

        websocket = new WebSocket($"ws://127.0.0.1:5500/");
        websocket.OnMessage += OnMessage;
        websocket.Connect();

        loginButton.onClick.AddListener(Login);
        registerButton.onClick.AddListener(Register);
    }

    private void Update()
    {
        EventCheck();
    }

    void Login()
    {
        string username = loginUsernameInput.text;
        string password = loginPasswordInput.text;

        LoginData newLoginData = new LoginData(username, password);
        // newLoginData.username = username;
        // newLoginData.password = password;
        string toJSONStr = JsonUtility.ToJson(newLoginData);

        if (!(string.IsNullOrEmpty(username)) && !(string.IsNullOrEmpty(password)))
        {
            SocketEvent newSocketEvent = new SocketEvent("Login", toJSONStr, "");
            string jsonStr = JsonUtility.ToJson(newSocketEvent);
            websocket.Send(jsonStr);
        }
        else
        {
            ErrPanel.SetActive(true);
        }
    }

    void Register()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;
        string playername = playernameInput.text;
        string hero = heroSelect.options[heroSelect.value].text;

        RegisterData newRegisterData = new RegisterData(username, password, playername, hero);
        string toJSONStr = JsonUtility.ToJson(newRegisterData);

        if (!(string.IsNullOrEmpty(username)) && !(string.IsNullOrEmpty(password)) && !(string.IsNullOrEmpty(playername)) && !(string.IsNullOrEmpty(hero)))
        {
            if (websocket.ReadyState == WebSocketState.Open)
            {
                SocketEvent newSocketEvent = new SocketEvent("Register", toJSONStr, "");
                string jsonStr = JsonUtility.ToJson(newSocketEvent);
                websocket.Send(jsonStr);
            }
        }
        else
        {
            ErrPanel.SetActive(true);
        }
    }

    void EventCheck()
    {
        if (string.IsNullOrEmpty(tempMessageString) == false)
        {
            SocketEvent eventCheck = JsonUtility.FromJson<SocketEvent>(tempMessageString);

            switch (eventCheck.eventName)
            {
                case "Login":
                    if (eventCheck.status == "true")
                    {
                        loginPanel.SetActive(false);
                        mainMenuPanel.SetActive(true);

                        UserData newUserInfo = JsonUtility.FromJson<UserData>(eventCheck.data);
                        playerName = newUserInfo.playername;
                        heroName = newUserInfo.heroname;

                        Debug.Log($"{playerName} as {heroName}");
                    }
                    break;
                case "Register":
                    if (eventCheck.status == "true")
                    {
                        loginPanel.SetActive(true);
                        registerPanel.SetActive(false);
                        // mainMenuPanel.SetActive(true);
                    }
                    break;
                default: break;
            }

            tempMessageString = string.Empty;
        }
    }

    public void OnMessage(object sender, MessageEventArgs messageEventArgs)
    {
        tempMessageString = messageEventArgs.Data;
        Debug.Log(tempMessageString);
    }

    private void OnDestroy()
    {
        if (websocket != null)
        {
            websocket.Close();
        }
    }
}
