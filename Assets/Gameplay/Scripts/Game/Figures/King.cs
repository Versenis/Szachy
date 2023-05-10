using Unity.Mathematics;
using Zenject;

public class King : Figure
{
    [Inject] PossibleDiagonalMovements _possibleDiagonalMovements;
    [Inject] PossibleMovementsAfterCross _possibleMovementsAfterCross;

    [Inject] Castling _castling;
    [Inject] IsCheckmate _isCheckmate;

    int2 _primaryPoint;
    bool _isFirst;

    void Start()
    {
        if (GetTeam == Team.White)  _isCheckmate._whiteKing = GetComponentInParent<Field>();
        else                        _isCheckmate._blackKing = GetComponentInParent<Field>();

        _primaryPoint = _point;
        _isFirst = true;
    }

    public override void PossibleMoves()
    {
        if (_point.x != _primaryPoint.x && _point.y != _primaryPoint.y)
            _isFirst = false;

        base.PossibleMoves();

        _possibleDiagonalMovements.Get(GetTeam, _point, _moves, 1);
        _possibleMovementsAfterCross.Get(GetTeam, _point, _moves, 1);
    }

    public override bool IsSpecialAction(Field selected0, Field selected1)
    {
        int2 point0 = IndexField(selected0);
        int2 point1 = IndexField(selected1);

        if (_isFirst && _castling.IsCastling(selected0, point0, selected1, point1))
        {
            _isFirst = false;
            return true;
        }

        return false;
    }
}
