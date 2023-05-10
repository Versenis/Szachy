using System.Collections.Generic;
using Unity.Mathematics;
using Zenject;

public class PossibleMoveToTop : PossibleMove
{
    Field[,] _fields;

    [Inject]
    void Construct(Fields fields)
    {
        _fields = fields._fields2D;
    }

    public override PossibleBasicMovementsModel Get(Team team, int2 point, PossibleBasicMovementsModel moves, int numberOfFields)
    {
        int index = point.y + 1;
        int endIndex = math.clamp(point.y + numberOfFields, int.MinValue, _fields.GetLength(0) - 1);

        if (index != _fields.GetLength(0))
            for (int y = index; y <= endIndex; y++)
            {
                Field destination = _fields[point.x, y];
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

    public List<Field> Get(int2 point, List<Field> moves, int numberOfFields)
    {
        int index = point.y + 1;
        int endIndex = math.clamp(point.y + numberOfFields, int.MinValue, _fields.GetLength(0) - 1);

        if (index != _fields.GetLength(0))
            for (int y = index; y <= endIndex; y++)
            {
                Field destination = _fields[point.x, y];
                Figure figure = destination.figure;

                if (!figure)
                {
                    moves.Add(destination);
                    break;
                }
            }

        return moves;
    }
}
