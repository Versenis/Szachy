using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerMenu : MonoBehaviour
{
    [SerializeField] Button _createGame;
    [SerializeField] Button _joinGame;
    [SerializeField] Button _exit;

    void Start()
    {
        _createGame.onClick.AddListener(() =>
        {
            MyTeam.myTeam = Team.White;

            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
        });

        _joinGame.onClick.AddListener(() =>
        {
            MyTeam.myTeam = Team.Black;

            NetworkManager.Singleton.StartClient();
            NetworkManager.Singleton.SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
        });

        _exit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
