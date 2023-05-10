using Unity.Mathematics;
using Zenject;

public class Rook : Figure
{
    [Inject] PossibleMovementsAfterCross _possibleMovementsAfterCross;

    [Inject] Castling _castling;

    int2 _primaryPoint;
    bool _isFirst;

    void Start()
    {
        _primaryPoint = _point;
        _isFirst = true;
    }

    public override void PossibleMoves()
    {
        if (_point.x != _primaryPoint.x && _point.y != _primaryPoint.y)
            _isFirst = false;

        base.PossibleMoves();

        _possibleMovementsAfterCross.Get(GetTeam, _point, _moves, 7);
    }

    public override bool IsSpecialAction(Field selected0, Field selected1)
    {
        int2 point0 = IndexField(selected0);
        int2 point1 = IndexField(selected1);

        if (_isFirst && _castling.IsCastling(selected1, point1, selected0, point0))
        {
            _isFirst = false;
            return true;
        }

        return false;
    }
}
