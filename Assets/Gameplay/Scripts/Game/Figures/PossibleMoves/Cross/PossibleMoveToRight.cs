using Unity.Mathematics;
using Zenject;

public class PossibleMoveToRight : PossibleMove
{
    Field[,] _fields;

    [Inject]
    void Construct(Fields fields)
    {
        _fields = fields._fields2D;
    }

    public override PossibleBasicMovementsModel Get(Team team, int2 point, PossibleBasicMovementsModel moves, int numberOfFields)
    {
        int index = point.x + 1;
        int endIndex = math.clamp(point.x + numberOfFields, int.MinValue, _fields.GetLength(0) - 1);

        if (index != _fields.GetLength(0))
            for (int x = index; x <= endIndex; x++)
            {
                Field destination = _fields[x, point.y];
                Figure figure = destination.figure;

                if (!figure)
                {
                    moves.moveCanBeMade.Add(destination);
                }
                else if (figure.GetTeam != team)
                {
                    moves.captures.Add(destination);
                    break;
                }
                else
                    break;
            }

        return moves;
    }
}
