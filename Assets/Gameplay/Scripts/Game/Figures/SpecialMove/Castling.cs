using Unity.Mathematics;
using Unity.Netcode;
using Zenject;

public class Castling
{
    [Inject] ChangeParent _changeParent;
    [Inject] IsCheck _isCheck;

    Field[,] _fields;

    [Inject]
    void Catling(Fields fields)
    {
        _fields = fields._fields2D;
    }

    public bool IsCastling(Field king, int2 pointKing, Field rook, int2 pointRook)
    {
        if (king.figure is King && rook.figure is Rook)
        {
            if (pointRook.x < pointKing.x)  return LeftRook(king, pointKing, rook, pointRook);
            else                            return RightRook(king, pointKing, rook, pointRook);
        }

        return false;
    }

    bool LeftRook(Field king, int2 pointKing, Field rook, int2 pointRook)
    {
        bool isCan = true;
        bool isByMeChess = false;

        int kingsDestination = pointKing.x - 2;
        int rooksDestination = pointRook.x + 3;

        for (int x = pointKing.x - 1; x > pointRook.x; x--)
        {
            Field nextField = _fields[x, pointKing.y];
            Figure figure = nextField.figure;

            if (x > kingsDestination)
                isByMeChess = _isCheck.Is(king, nextField);

            if (isByMeChess || figure)
            {
                isCan = false;
                break;
            }
        }

        if (isCan)
        {
            _changeParent.ServerRpc(king.GetComponent<NetworkObject>(), _fields[kingsDestination, pointKing.y].GetComponent<NetworkObject>());
            _changeParent.ServerRpc(rook.GetComponent<NetworkObject>(), _fields[rooksDestination, pointRook.y].GetComponent<NetworkObject>());

            return true;
        }

        return false;
    }

    bool RightRook(Field king, int2 pointKing, Field rook, int2 pointRook)
    {
        bool isCan = true;
        bool isByMeChess = false;

        int kingsDestination = pointKing.x + 2;
        int rooksDestination = pointRook.x - 2;

        for (int x = pointKing.x + 1; x < pointRook.x; x++)
        {
            Field nextField = _fields[x, pointKing.y];
            Figure figure = nextField.figure;

            if (x < kingsDestination)
                isByMeChess = _isCheck.Is(king, nextField);

            if (isByMeChess || figure)
            {
                isCan = false;
                break;
            }
        }

        if (isCan)
        {
            _changeParent.ServerRpc(king.GetComponent<NetworkObject>(), _fields[kingsDestination, pointKing.y].GetComponent<NetworkObject>());
            _changeParent.ServerRpc(rook.GetComponent<NetworkObject>(), _fields[rooksDestination, pointKing.y].GetComponent<NetworkObject>());

            return true;
        }

        return false;
    }
}
