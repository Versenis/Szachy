using System.Collections.Generic;
using System.Linq;
using Zenject;

public class IsCheck
{
    [Inject] FiguresOnAChessboard _figuresOnAChessboard;

    public bool Is(Field selected0, Field selected1)
    {
        Figure memorizedFigure0 = selected0.figure;
        Figure memorizedFigure1 = selected1.figure;

        selected1.figure = selected0.figure;
        selected0.figure = null;

        bool isCheck;

        if (MyTeam.myTeam == Team.White)    isCheck = DiscoveredKing(_figuresOnAChessboard._blackFigures);
        else                                isCheck = DiscoveredKing(_figuresOnAChessboard._whiteFigures);

        selected0.figure = memorizedFigure0;
        selected1.figure = memorizedFigure1;

        if (isCheck)
            return true;

        return false;
    }

    bool DiscoveredKing(List<Figure> figures)
    {
        foreach (var figure in figures.ToList())
        {
            figure.PossibleMoves();

            foreach (var capture in figure._moves.captures)
                if (capture.figure is King)
                    return true;
        }

        return false;
    }
}
