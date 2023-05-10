using Unity.Mathematics;
using Zenject;

public class PossibleMovementsAfterCross
{
    [Inject] PossibleMoveToBottom _possibleBottom;
    [Inject] PossibleMoveToLeft _possibleLeft;
    [Inject] PossibleMoveToRight _possibleRight;
    [Inject] PossibleMoveToTop _possibleTop;

    public PossibleBasicMovementsModel Get(Team team, int2 point, PossibleBasicMovementsModel moves, int numberOfFields)
    {
        _possibleRight.Get(team, point, moves, numberOfFields);
        _possibleLeft.Get(team, point, moves, numberOfFields);

        _possibleTop.Get(team, point, moves, numberOfFields);
        _possibleBottom.Get(team, point, moves, numberOfFields);

        return moves;
    }
}
