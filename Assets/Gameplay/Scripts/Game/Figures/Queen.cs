using Zenject;

public class Queen : Figure
{
    [Inject] PossibleDiagonalMovements _possibleDiagonalMovements;
    [Inject] PossibleMovementsAfterCross _possibleMovementsAfterCross;

    public override void PossibleMoves()
    {
        base.PossibleMoves();

        _possibleDiagonalMovements.Get(GetTeam, _point, _moves, 7);
        _possibleMovementsAfterCross.Get(GetTeam, _point, _moves, 7);
    }
}
