namespace CoffeeToolkit.DependencyInjection;

/*
 * This class is made available under the Unlicense.
 * You are free to use, modify, and distribute the class as you see fit,
 * without any restrictions or conditions.
 * The rest of the application is not covered by this license.
 */
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Scan all namespaces starting with an entry in partialNamespaces, and register all classes with their corresponding interface.
    /// This is an easy but limited solution and developers are encouraged to modify the code to suit their needs if they need more control.
    /// </summary>
    /// <param name="builder">Autofac Container Builder</param>
    /// <param name="partialNamespaces">All namespaces that start with a value in this array will be scanned.</param>
    public static void AutoRegister(this ContainerBuilder builder, params string[] partialNamespaces)
    {
        // Validate
        if (partialNamespaces == null || partialNamespaces.Length == 0)
            throw new ArgumentException("AutoRegister requires at least one namespace path to be specified.");
        
        // Get all applicable types
        partialNamespaces = partialNamespaces.Where(x => x.Length > 0).ToArray();
        var allTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetReferencedAssemblies())
            .UniqueBy(x => x.FullName)
            // Get all assemblies starting with anything from assemblyPaths
            .Where(a => partialNamespaces.Select(x => a.FullName.StartsWith(x)).Where(x => x).Any())
            .Select(a => Assembly.Load(a))
            .SelectMany(t => t.GetTypes())
            .ToList();

        foreach (string assemblyPath in partialNamespaces)
            foreach (Type t in allTypes)
            {
                // False if ineligible
                bool isIneligible = t.Namespace is null || // Defined namespace
                    !t.Namespace.StartsWith(assemblyPath) || // matches assemblyPath
                    !t.IsClass || t.IsAbstract || // is not interface
                    !t.GetInterfaces().Any(); // Only contine if it has any interfaces
                if (isIneligible)
                    continue;

                // Attempt to find corresponding interface
                var correspondingInterfaces = t.GetInterfaces()
                    .Where(i => i.Name == $"I{t.Name}");

                // False if no corresponding interface 
                if (correspondingInterfaces.Count() == 0)
                    continue;

                // Begin registration
                var typeRegistration = builder.RegisterType(t)
                    .As(correspondingInterfaces.First());

                // Complete registration with defined scope (if any)
                DependencyScopeAttribute? scope = t.GetCustomAttribute<DependencyScopeAttribute>();
                if (scope is null || scope.Scope == InstanceScope.Undefined) // Default
                    typeRegistration.InstancePerDependency();
                else 
                    switch (scope.Scope)
                    {
                        case InstanceScope.Single:
                            typeRegistration.SingleInstance();
                            break;
                        case InstanceScope.Lifetime:
                            typeRegistration.InstancePerLifetimeScope();
                            break;
                        case InstanceScope.PerRequest:
                            typeRegistration.InstancePerRequest();
                            break;
                        default:
                            throw new Exception($"AutoRegister does not know how to handle scope {scope.Scope} for {t.FullName}" );
                    }
            }
    }
}