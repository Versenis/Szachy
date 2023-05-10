using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Button _createGame;
    [SerializeField] Button _joinGame;
    [SerializeField] Button _exit;

    [SerializeField] UnityTransport _transport;
    [SerializeField] TMP_InputField _inputField;

    void Start()
    {
        _createGame.onClick.AddListener(() =>
        {
            MyTeam.myTeam = Team.White;

            _transport.ConnectionData.Address = _inputField.text;

            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
        });

        _joinGame.onClick.AddListener(() =>
        {
            MyTeam.myTeam = Team.Black;

            _transport.ConnectionData.Address = _inputField.text;

            NetworkManager.Singleton.StartClient();
        });

        _exit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
