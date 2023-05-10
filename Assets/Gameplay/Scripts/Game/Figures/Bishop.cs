using Zenject;

public class Bishop : Figure
{
    [Inject] PossibleDiagonalMovements _possibleDiagonalMovements;

    public override void PossibleMoves()
    {
        base.PossibleMoves();

        _possibleDiagonalMovements.Get(GetTeam, _point, _moves, 7);
    }
}
