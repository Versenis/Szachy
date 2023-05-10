using System;
using Unity.Netcode;
using Zenject;

public class Rounds : NetworkBehaviour
{
    [Inject] Selection _selection;
    [Inject] IsCheckmate _isCheckmate;
    [Inject] SystemEnPassant _systemEnPassant;

    public static event Action<Team> OnEndGame;

    Team _currentTeam;

    void Start()
    {
        _currentTeam = Team.White;

        if (MyTeam.myTeam == Team.Black)
            _selection.enabled = false;
    }

    [ServerRpc(RequireOwnership = false)]
    public void NextRoundServerRpc()
    {
        NextRoundClientRpc();

        if (_isCheckmate.Is())
            OnEndGame(Team.White);

        _systemEnPassant.SystemEnPassantClientRpc(_currentTeam);
    }

    [ClientRpc]
    void NextRoundClientRpc()
    {
        if (_currentTeam == Team.Black)     _currentTeam = Team.White;
        else                                _currentTeam = Team.Black;

        Enabled();
    }

    void Enabled()
    {
        if (_currentTeam == Team.Black && MyTeam.myTeam == Team.Black)              _selection.enabled = true;
        else if (_currentTeam == Team.White && MyTeam.myTeam == Team.White)         _selection.enabled = true;
        else                                                                        _selection.enabled = false;
    }
}
