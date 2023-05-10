public class Knight : Figure
{
    public override void PossibleMoves()
    {
        base.PossibleMoves();

        PossibleMove(-1, 2);
        PossibleMove(1, 2);

        PossibleMove(2, 1);
        PossibleMove(2, -1);

        PossibleMove(1, -2);
        PossibleMove(-1, -2);

        PossibleMove(-2, -1);
        PossibleMove(-2, 1);
    }

    void PossibleMove(int moveX, int moveY)
    {
        int possibleMovementX = _point.x + moveX;
        int possibleMovementY = _point.y + moveY;

        if (possibleMovementX >= 0 && possibleMovementX < _fields.GetLength(0) &&
            possibleMovementY >= 0 && possibleMovementY < _fields.GetLength(1))
        {
            Field destination = _fields[possibleMovementX, possibleMovementY];
            Figure figure = destination.figure;

            if (!figure)                            _moves.moveCanBeMade.Add(destination);
            else if (figure.GetTeam != GetTeam)     _moves.captures.Add(destination);
        }
    }
}
