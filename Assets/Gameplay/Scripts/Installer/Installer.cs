using Zenject;

public class Installer : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PossibleBasicMovementsModel>().AsTransient();

        PossibleMovementsAfterCross();
        PossibleDiagonalMovements();

        Container.Bind<IsCheck>().AsSingle();
        Container.Bind<IsCheckmate>().AsSingle();

        Container.Bind<Castling>().AsSingle();
    }

    void PossibleMovementsAfterCross()
    {
        Container.Bind<PossibleMovementsAfterCross>().AsSingle();

        Container.Bind<PossibleMoveToBottom>().AsSingle();
        Container.Bind<PossibleMoveToLeft>().AsSingle();
        Container.Bind<PossibleMoveToTop>().AsSingle();
        Container.Bind<PossibleMoveToRight>().AsSingle();
    }

    void PossibleDiagonalMovements()
    {
        Container.Bind<PossibleDiagonalMovements>().AsSingle();

        Container.Bind<PossibleMoveToBottomLeft>().AsSingle();
        Container.Bind<PossibleMoveToBottomRight>().AsSingle();
        Container.Bind<PossibleMoveToTopLeft>().AsSingle();
        Container.Bind<PossibleMoveToTopRight>().AsSingle();
    }
}
