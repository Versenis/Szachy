using Unity.Mathematics;
using Zenject;

public class PossibleDiagonalMovements
{
    [Inject] PossibleMoveToBottomLeft _possibleBottomLeft;
    [Inject] PossibleMoveToBottomRight _possibleBottomRight;
    [Inject] PossibleMoveToTopLeft _possibleTopLeft;
    [Inject] PossibleMoveToTopRight _possibleTopRight;

    public PossibleBasicMovementsModel Get(Team team, int2 point, PossibleBasicMovementsModel moves, int numberOfFields)
    {
        _possibleTopRight.Get(team, point, moves, numberOfFields);
        _possibleTopLeft.Get(team, point, moves, numberOfFields);

        _possibleBottomRight.Get(team, point, moves, numberOfFields);
        _possibleBottomLeft.Get(team, point, moves, numberOfFields);

        return moves;
    }
}
