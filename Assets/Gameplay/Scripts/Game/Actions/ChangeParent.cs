using Unity.Netcode;
using UnityEngine;
using Zenject;

public class ChangeParent : NetworkBehaviour
{
    [Inject] IsCheckmate _isCheckmate;

    [ServerRpc(RequireOwnership = false)]
    public void ServerRpc(NetworkObjectReference selected0, NetworkObjectReference selected1)
    {
        selected0.TryGet(out NetworkObject selected0N);
        selected1.TryGet(out NetworkObject selected1N);

        selected0N.transform.GetChild(1).GetComponent<NetworkObject>().TrySetParent(selected1N, false);

        FollowClientRpc(selected0, selected1);
    }

    [ClientRpc]
    void FollowClientRpc(NetworkObjectReference selected0, NetworkObjectReference selected1)
    {
        selected0.TryGet(out NetworkObject selected0N);
        selected1.TryGet(out NetworkObject selected1N);
        
        selected1N.transform.GetChild(1).localPosition = new Vector2(0, 0);

        Field field0 = selected0N.GetComponent<Field>();
        Field field1 = selected1N.GetComponent<Field>();

        field1.figure = field0.figure;
        field0.figure = null;

        if (field1.figure is King)
        {
            if (field1.figure.GetTeam == Team.White)    _isCheckmate._whiteKing = field1;
            else                                        _isCheckmate._blackKing = field1;
        }
    }
}
