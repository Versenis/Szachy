using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public abstract class Figure : NetworkBehaviour
{
    [SerializeField] Team _team;

    [Inject] public PossibleBasicMovementsModel _moves { get; protected set; }

    [Inject] FiguresOnAChessboard _figuresOnAChessboard;

    protected int2 _point { get; private set; }
    protected Field[,] _fields { get; private set; }

    [Inject]
    void Construct(Fields fields)
    {
        _fields = fields._fields2D;

        _moves.moveCanBeMade = new List<Field>();
        _moves.captures = new List<Field>();
    }

    public virtual void PossibleMoves()
    {
        _moves.moveCanBeMade.Clear();
        _moves.captures.Clear();

        _point = IndexField(GetComponentInParent<Field>());
    }

    public virtual bool IsSpecialAction(Field selected0, Field selected1) { return false; }

    protected int2 IndexField(Field field)
    {
        Field isInTheField = field;

        int2 point = new int2(0, 0);

        for (int y = 0; y < _fields.GetLength(0); y++)
            for (int x = 0; x < _fields.GetLength(1); x++)
                if (_fields[x, y] == isInTheField)
                {
                    point.x = x;
                    point.y = y;

                    return point;
                }

        return point;
    }

    public Team GetTeam
    {
        get { return _team; }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc()
    {
        transform.parent = null;

        NetworkObject network = GetComponent<NetworkObject>();
        _figuresOnAChessboard.RemoveFigureClientRpc(network);
        network.Despawn();
    }
}
