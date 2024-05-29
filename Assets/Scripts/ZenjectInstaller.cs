using UnityEngine;
using Zenject;

public class ZenjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.Bind<GameSettings>().AsSingle();
        Container.Bind<ShopItem>().AsCached();
    }
}