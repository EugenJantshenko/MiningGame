using Zenject;

public class SignalsInstaller : MonoInstaller
{
	#region Public Methods
	public override void InstallBindings()
	{
		if (!Container.HasBinding<SignalBus>())
		{
			SignalBusInstaller.Install(Container);
		}

		Container.DeclareSignal<AddCoinsSignal>();
		Container.DeclareSignal<FactoryUpgradePurchasedSignal>();
		Container.DeclareSignal<DozerUpgradePurchasedSignal>();
		Container.DeclareSignal<TruckUpgradePurchasedSignal>();
	}
	#endregion
}