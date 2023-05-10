using System;
using UnityEngine;
using Unity.Netcode;
using Zenject;

public abstract class Pawn : Figure
{
    [HideInInspector] public bool _isFirst;
    [Inject] protected ChangeParent _changeParent { get; private set; }
    [Inject] IsCheck _isCheck;

    public static event Action<Field, GameObject> OnPromotion;

    protected GameObject _transformationUI { private get; set; }
    protected int _primaryY { get; private set; }

    void Start()
    {
        _primaryY = IndexField(GetComponentInParent<Field>()).y;
        _isFirst = true;
    }

    public override bool IsSpecialAction(Field selected0, Field selected1)
    {
        if (_point.x - 1 == IndexField(selected1).x)    return EnPassant(_point.x - 1, selected0, selected1);
        else                                            return EnPassant(_point.x + 1, selected0, selected1);
    }

    protected abstract bool IsFirstMove(int numberOfFields, Field selected0, Field selected1);

    bool EnPassant(int side, Field selected0, Field selected1)
    {
        if (side != -1 && side != 8)
        {
            Field destination = _fields[side, _point.y];
            Figure figure = destination.figure;

            if (figure)
            {
                Pawn pawn = figure.GetComponent<Pawn>();

                if (pawn && pawn._isFirst)
                    if (figure.GetTeam != GetTeam && !_isCheck.Is(selected0, selected1))
                    {
                        destination.figure.DestroyServerRpc();
                        _changeParent.ServerRpc(selected0.GetComponent<NetworkObject>(), selected1.GetComponent<NetworkObject>());

                        return true;
                    }
            }
        }

        return false;
    }

    protected void Promotion()
    {
        _transformationUI.SetActive(true);
        if (!NetworkManager.Singleton.IsHost)   OnPromotion(_fields[_point.x, _point.y - 1], _transformationUI);
        else                                    OnPromotion(_fields[_point.x, _point.y], _transformationUI);


        DestroyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void IsFirstMovePawnServerRpc(NetworkObjectReference networkR, bool isFirst)
    {
        IsFirstMovePawnClientRpc(networkR, isFirst);
    }

    [ClientRpc]
    void IsFirstMovePawnClientRpc(NetworkObjectReference networkR, bool isFirst)
    {
        networkR.TryGet(out NetworkObject network);
        network.GetComponent<Pawn>()._isFirst = isFirst;
    }
}
