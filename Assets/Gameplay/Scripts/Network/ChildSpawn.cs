using Unity.Netcode;
using UnityEngine;
using Zenject;

public class ChildSpawn : MonoBehaviour
{
    [SerializeField] GameObject _figure;
    [Inject] FiguresOnAChessboard _figuresOnAChessboard;
    [Inject] DiContainer _diContainer;

    Figure _script;

    void Start()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            GameObject figure = Instantiate(_figure);
            figure.GetComponent<NetworkObject>().Spawn();

            _script = figure.GetComponent<Figure>();
            _diContainer.Inject(_script);

            figure.GetComponent<NetworkObject>().TrySetParent(transform, false);
            transform.GetComponent<Field>().figure = _script;
        }
        else
        {
            _script = transform.GetChild(1).GetComponent<Figure>();
            _diContainer.Inject(_script);

            transform.GetComponent<Field>().figure = _script;
        }

        _figuresOnAChessboard.AddFigure(_script);

        Destroy(this);
    }
}
