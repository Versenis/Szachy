using Unity.Netcode;
using Zenject;

public class WhitePawn : Pawn
{
    [Inject] PossibleMoveToTop _possibleMoveToTop;
    [Inject] PossibleMoveToTopLeft _possibleMoveToTopLeft;
    [Inject] PossibleMoveToTopRight _possibleMoveToTopRight;

    [Inject] SystemEnPassant _systemEnPassant;
    [Inject] IsCheck _isCheck;

    [Inject]
    void Construct(WhiteTeamID whiteTeamId)
    {
        _transformationUI = whiteTeamId.gameObject;
    }

    public override void PossibleMoves()
    {
        base.PossibleMoves();

        if (_point.y == _fields.GetLength(0) - 1)
            Promotion();

        _possibleMoveToTop.Get(_point, _moves.moveCanBeMade, 1);
        _possibleMoveToTopLeft.Get(GetTeam, _point, _moves.captures, 1);
        _possibleMoveToTopRight.Get(GetTeam, _point, _moves.captures, 1);
    }

    public override bool IsSpecialAction(Field selected0, Field selected1)
    {
        base.IsSpecialAction(selected0, selected1);

        if (_isFirst)
            return IsFirstMove(2, selected0, selected1);

        return false;
    }

    protected override bool IsFirstMove(int numberOfFields, Field selected0, Field selected1)
    {
        if (_fields[_point.x, _primaryY + numberOfFields] == selected1)
        {
            int indexY = _primaryY + 1;
            bool isCan = true;

            for (int i = 0; i > numberOfFields; i++)
            {
                Field destination = _fields[_point.x, indexY + i];
                Figure figure = destination.figure;

                if (figure)
                {
                    isCan = false;
                    break;
                }
            }

            if (isCan && !_isCheck.Is(selected0, selected1))
            {
                _changeParent.ServerRpc(selected0.GetComponent<NetworkObject>(), selected1.GetComponent<NetworkObject>());
                IsFirstMovePawnServerRpc(GetComponent<NetworkObject>(), true);

                _systemEnPassant._whitePawn = this;
                return true;
            }
        }
        else
            IsFirstMovePawnServerRpc(GetComponent<NetworkObject>(), false);

        return false;
    }
}
