using Unity.Netcode;

public class SystemEnPassant : NetworkBehaviour
{
    public Pawn _whitePawn { private get; set; }
    public Pawn _blackPawn { private get; set; }

    [ClientRpc]
    public void SystemEnPassantClientRpc(Team currentTeam)
    {
        if (_whitePawn
            && _whitePawn.GetComponent<NetworkObject>().IsSpawned
            && currentTeam == Team.White)
        {
            CantEnPassant(_whitePawn.GetComponent<NetworkObject>());
            _whitePawn = null;
        }
        else if (_blackPawn 
            && _blackPawn.GetComponent<NetworkObject>().IsSpawned
            && currentTeam == Team.Black)
        {
            CantEnPassant(_blackPawn.GetComponent<NetworkObject>());
            _blackPawn = null;
        }
    }

    void CantEnPassant(NetworkObjectReference networkR)
    {
        networkR.TryGet(out NetworkObject network);
        Pawn pawn = network.GetComponent<Pawn>();

        pawn.IsFirstMovePawnServerRpc(pawn.GetComponent<NetworkObject>(), false);
    }
}
