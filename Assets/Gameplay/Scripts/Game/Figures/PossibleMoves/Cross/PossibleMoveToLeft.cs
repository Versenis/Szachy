using Unity.Mathematics;
using Zenject;

public class PossibleMoveToLeft : PossibleMove
{
    Field[,] _fields;

    [Inject]
    void Construct(Fields fields)
    {
        _fields = fields._fields2D;
    }

    public override PossibleBasicMovementsModel Get(Team team, int2 point, PossibleBasicMovementsModel moves, int numberOfFields)
    {
        int index = point.x - 1;
        int endIndex = math.clamp(point.x - numberOfFields, 0, int.MaxValue);

        if (index != -1)
            for (int x = index; x >= endIndex; x--)
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
