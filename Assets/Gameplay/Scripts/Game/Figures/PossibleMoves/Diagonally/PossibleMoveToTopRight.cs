using System.Collections.Generic;
using Unity.Mathematics;
using Zenject;

public class PossibleMoveToTopRight : PossibleMove
{
    Field[,] _fields;

    [Inject]
    void Construct(Fields fields)
    {
        _fields = fields._fields2D;
    }

    public override PossibleBasicMovementsModel Get(Team team, int2 point, PossibleBasicMovementsModel moves, int numberOfFields)
    {
        int fewerMovements;

        int indexX = point.x + 1;
        int indexY = point.y + 1;

        if (indexX > indexY)    fewerMovements = indexX;
        else                    fewerMovements = indexY;

        if (fewerMovements != _fields.GetLength(0))
        {
            int endIndex = math.clamp(numberOfFields - 1, int.MinValue, _fields.GetLength(0) - 1 - fewerMovements);

            for (int i = 0; i <= endIndex; i++)
            {
                Field destination = _fields[indexX + i, indexY + i];
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
        }

        return moves;
    }

    public List<Field> Get(Team team, int2 point, List<Field> moves, int numberOfFields)
    {
        int fewerMovements;

        int indexX = point.x + 1;
        int indexY = point.y + 1;

        if (indexX > indexY)    fewerMovements = indexX;
        else                    fewerMovements = indexY;

        if (fewerMovements != _fields.GetLength(0))
        {
            int endIndex = math.clamp(numberOfFields - 1, int.MinValue, _fields.GetLength(0)  - 1 - fewerMovements);

            for (int i = 0; i <= endIndex; i++)
            {
                Field destination = _fields[indexX + i, indexY + i];
                Figure figure = destination.figure;

                if (figure != null && figure.GetTeam != team)
                {
                    moves.Add(destination);
                    break;
                }
            }
        }

        return moves;
    }
}
