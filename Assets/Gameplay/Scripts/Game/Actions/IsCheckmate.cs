using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class IsCheckmate
{
    public Field _whiteKing { private get; set; }
    public Field _blackKing { private get; set; }

    [Inject] FiguresOnAChessboard _figuresOnAChessboard;
    [Inject] IsCheck _isCheck;

    public bool Is()
    {
        if (Check(_whiteKing) || Check(_blackKing))
            return true;

        return false;
    }

    bool Check(Field king)
    {
        Figure figure = king.figure;
        figure.PossibleMoves();

        if (CanKingMove(figure._moves.moveCanBeMade, king)) return false;
        else if (CanKingMove(figure._moves.captures, king)) return false;

        List<Figure> enemies;

        if (figure.GetTeam == Team.White)   enemies = KingAttackFigures(_figuresOnAChessboard._blackFigures);
        else                                enemies = KingAttackFigures(_figuresOnAChessboard._whiteFigures);

        if (enemies.Count == 1)
        {
            if (figure.GetTeam == Team.White)   return WhetherItIsPossibleToStopTheEnemy(_figuresOnAChessboard._whiteFigures, enemies[0]);
            else                                return WhetherItIsPossibleToStopTheEnemy(_figuresOnAChessboard._blackFigures, enemies[0]);
        }

        return false;
    }

    bool CanKingMove(List<Field> moves, Field king)
    {
        foreach (var move in moves.ToList())
            if (!_isCheck.Is(king, move))
                return true;

        return false;
    }

    List<Figure> KingAttackFigures(List<Figure> figures)
    {
        List<Figure> enemies = new List<Figure>();

        foreach (var figure in figures)
        {
            figure.PossibleMoves();

            foreach (var capture in figure._moves.captures)
                if (capture.figure is King)
                    enemies.Add(figure);
        }

        return enemies;
    }

    bool WhetherItIsPossibleToStopTheEnemy(List<Figure> myFigures, Figure enemy)
    {
        foreach (var figure in myFigures)
        {
            figure.PossibleMoves();

            foreach (var move in enemy._moves.moveCanBeMade)
                foreach (var enemyMove in figure._moves.moveCanBeMade)
                    if (move == enemyMove)
                        return true;

            foreach (var capture in figure._moves.captures)
                if (capture.figure == enemy)
                    return true;
        }

        return false;
    }
}
