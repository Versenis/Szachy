using System.Collections.Generic;
using Unity.Netcode;

public class FiguresOnAChessboard : NetworkBehaviour
{
    public List<Figure> _blackFigures { get; private set; }
    public List<Figure> _whiteFigures { get; private set; }

    FiguresOnAChessboard()
    {
        _blackFigures = new List<Figure>();
        _whiteFigures = new List<Figure>();
    }

    public void AddFigure(Figure figure)
    {
        if (figure.GetTeam == Team.White)   _whiteFigures.Add(figure);
        else                                _blackFigures.Add(figure);
    }

    [ClientRpc]
    public void RemoveFigureClientRpc(NetworkObjectReference networkR)
    {
        networkR.TryGet(out NetworkObject network);
        Figure figure = network.GetComponent<Figure>();

        if (figure.GetTeam == Team.White)   _whiteFigures.Remove(figure);
        else                                _blackFigures.Remove(figure);
    }
}
