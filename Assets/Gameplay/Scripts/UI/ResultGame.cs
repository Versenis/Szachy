using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultGame : NetworkBehaviour
{
    void Awake()
    {
        Rounds.OnEndGame += EndGameClientRpc;
        Selection.OnEndGame += EndGameClientRpc;
    }

    [ClientRpc]
    void EndGameClientRpc(Team lostTeam)
    {
        if (MyTeam.myTeam == lostTeam)
            transform.GetComponentInChildren<TextMeshProUGUI>().text = "Przegrana";

        gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }

    public override void OnNetworkDespawn()
    {
        Rounds.OnEndGame -= EndGameClientRpc;
    }
}
