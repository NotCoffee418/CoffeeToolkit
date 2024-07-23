namespace CoffeeToolkit.DependencyInjection.Autofac;

public static class AfDependencyInjectionExtensions
{
    /// <summary>
    /// Scan all namespaces starting with an entry in partialNamespaces, and register all classes with their corresponding interface.
    /// This is an easy but limited solution and developers are encouraged to modify the code to suit their needs if they need more control.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="partialNamespaces"></param>
    public static void AutoRegister(this ContainerBuilder builder, params string[] partialNamespaces)
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AutoRegister(partialNamespaces);
        builder.RegisterServiceCollection(serviceCollection);
    }

    /// <summary>
    /// Add Microsoft Dependency Injection services to the Autofac container builder.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="services"></param>
    public static void RegisterServiceCollection(this ContainerBuilder builder, IServiceCollection services)
    {
        builder.Populate(services);
    }
}
