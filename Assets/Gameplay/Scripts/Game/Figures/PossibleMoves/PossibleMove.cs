using Unity.Mathematics;

public abstract class PossibleMove
{
    public abstract PossibleBasicMovementsModel Get(Team team, int2 point, PossibleBasicMovementsModel moves, int numberOfFields);
}
