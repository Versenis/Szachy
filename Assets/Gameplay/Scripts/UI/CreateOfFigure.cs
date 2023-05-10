using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CreateOfFigure : NetworkBehaviour
{
    [SerializeField] GameObject _figure;
    [Inject] FiguresOnAChessboard _figuresOnAChessboard;
    [Inject] DiContainer _diContainer;

    Field _field;
    GameObject _transformationUI;

    void Awake()
    {
        Pawn.OnPromotion += Promotion;
    }

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => Create());
    }

    void Promotion(Field field, GameObject transformationUI)
    {
        _field = field;
        _transformationUI = transformationUI;
    }

    void Create()
    {
        _transformationUI.SetActive(false);
        CreateServerRpc(_field.GetComponent<NetworkObject>());
    }

    [ServerRpc(RequireOwnership = false)]
    void CreateServerRpc(NetworkObjectReference fieldR)
    {
        fieldR.TryGet(out NetworkObject field);

        GameObject createdFigure = Instantiate(_figure);
        NetworkObject networkObject = createdFigure.GetComponent<NetworkObject>();
        networkObject.Spawn();

        ChangeParentServerRpc(networkObject, field);
    }

    [ServerRpc(RequireOwnership = false)]
    void ChangeParentServerRpc(NetworkObjectReference figure, NetworkObjectReference field)
    {
        figure.TryGet(out NetworkObject figureN);
        field.TryGet(out NetworkObject fieldN);

        figureN.TrySetParent(fieldN);

        FollowClientRpc(figure, field);
    }

    [ClientRpc]
    void FollowClientRpc(NetworkObjectReference figure, NetworkObjectReference field)
    {
        figure.TryGet(out NetworkObject figureN);
        field.TryGet(out NetworkObject fieldN);

        figureN.transform.localPosition = new Vector2(0, 0);

        Figure figureScript = figureN.GetComponent<Figure>();
        _diContainer.Inject(figureScript);
        fieldN.GetComponent<Field>().figure = figureScript;

        _figuresOnAChessboard.AddFigure(figureScript);
    }

    public override void OnNetworkDespawn()
    {
        Pawn.OnPromotion -= Promotion;
    }
}
