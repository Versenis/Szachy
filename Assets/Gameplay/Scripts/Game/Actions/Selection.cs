using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class Selection : NetworkBehaviour
{
    [Inject] ChangeParent _changeParent;
    [Inject] Rounds _rounds;
    [Inject] IsCheck _isCheck;
    [Inject] IsCheckmate _isCheckmate;

    public static event Action<Team> OnEndGame;

    Collider2D[] _collidersHit;

    List<Field> _moveCanBeMade;
    List<Field> _captures;

    void Start()
    {
        _collidersHit = new Collider2D[2];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_isCheckmate.Is())
                EndGameServerRpc();

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                Figure hitFigure = hit.collider.GetComponent<Field>().figure;

                if (_collidersHit[0] != null)
                {
                    _collidersHit[1] = hit.collider;

                    Field selected0 = _collidersHit[0].GetComponent<Field>();
                    Field selected1 = _collidersHit[1].GetComponent<Field>();

                    bool isSpecialAction = selected0.figure.IsSpecialAction(selected0, selected1);

                    if (isSpecialAction)
                    {
                        NextRound();
                    }
                    else if (selected1.figure && MyTeam.myTeam == selected1.figure.GetTeam)
                    {
                        selected1.figure.PossibleMoves();

                        _collidersHit[0] = selected1.GetComponent<Collider2D>();
                        Array.Clear(_collidersHit, 1, 1);
                    }
                    else
                    {
                        bool isByMeChess = _isCheck.Is(selected0, selected1);

                        if (!isByMeChess)
                        {
                            if (_moveCanBeMade.Contains(selected1))
                            {
                                ExecutionOfMove(selected0, selected1);
                                NextRound();
                            }
                            else if (_captures.Contains(selected1))
                            {
                                selected1.figure.DestroyServerRpc();
                                ExecutionOfMove(selected0, selected1);

                                NextRound();
                            }
                        }
                        else
                            Array.Clear(_collidersHit, 0, 2);
                    }
                }
                else if (hitFigure && hitFigure.GetTeam == MyTeam.myTeam)
                {
                    _collidersHit[0] = hit.collider;

                    hitFigure.PossibleMoves();
                    _moveCanBeMade = hitFigure._moves.moveCanBeMade;
                    _captures = hitFigure._moves.captures;

                }
            }
        }
    }

    void ExecutionOfMove(Field selected0, Field selected1)
    {
        _changeParent.ServerRpc(
            selected0.GetComponent<NetworkObject>(),
            selected1.GetComponent<NetworkObject>());

        if (selected0.figure is WhitePawn)
        {
            selected1.figure.PossibleMoves();
        }
        else if (selected0.figure is BlackPawn)
        {
            selected0.figure.GetComponent<BlackPawn>()._compensationSetParent = true;
            selected0.figure.PossibleMoves();
        }

        Array.Clear(_collidersHit, 0, 2);
    }

    void NextRound()
    {
        Array.Clear(_collidersHit, 0, 2);
        _rounds.NextRoundServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void EndGameServerRpc()
    {
        OnEndGame(Team.Black);
    }
}
