using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnitySignalR;

namespace CSignalr.Demo
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        [SerializeField] private GameObject holder;
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TMP_InputField textInput;
        [SerializeField] private Button send;
        [SerializeField] private TextMeshProUGUI text;

        private SignalRHandler signalRHandler;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Setup(SignalRHandler _signalRHandler)
        {
            holder.SetActive(true);
            signalRHandler = _signalRHandler;
        }

        private void OnEnable()
        {
            send.onClick.AddListener(SendMessage);
            SignalREvents.HandleReceivedMessage += ShowMessage;
        }

        private void OnDisable()
        {
            send.onClick.RemoveListener(SendMessage);
            SignalREvents.HandleReceivedMessage -= ShowMessage;
        }

        private void SendMessage()
        {
            signalRHandler.SendMessage(nameInput.text,textInput.text);
        }

        private void ShowMessage(MessageData _messageData)
        {
            text.text += $"{_messageData.Sender}: {_messageData.Message}\n";
        }
    }
}