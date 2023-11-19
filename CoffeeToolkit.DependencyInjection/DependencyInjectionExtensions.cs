using System.Reflection;

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
        MatchedInterface[] registerableDependencies = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetReferencedAssemblies())
            .UniqueBy(x => x.FullName)
            // Get all assemblies starting with anything from assemblyPaths
            .Where(a => partialNamespaces.Any(ns => a.FullName.StartsWith(ns)))
            .Select(a => Assembly.Load(a))
            .SelectMany(t => t.GetTypes())
            // Type filtering after assembly filtering
            .Where(t =>
                t.Namespace is not null && t.IsClass && !t.IsAbstract &&
                partialNamespaces.Any(ns => t.Namespace.StartsWith(ns)))
            .Select(t =>
            {
                // Get the interface that matches the class if any, null otherwise
                Type matchingInterface = t.GetInterfaces().Where(x => x.Name == $"I{t.Name}").FirstOrDefault();
                if (matchingInterface is null)
                    return null;

                // Decide scope
                DependencyScopeAttribute? scopeAttr = t.GetCustomAttribute<DependencyScopeAttribute>();
                InstanceScope scope = scopeAttr is null || scopeAttr.Scope == InstanceScope.Undefined 
                    ? InstanceScope.PerDependency : scopeAttr.Scope;

                return new MatchedInterface(t, matchingInterface, scope);
            })
            .OfType<MatchedInterface>() // null filter
            .ToArray();

        // Register all valid types
        foreach (MatchedInterface mi in registerableDependencies)
        {
            var typeRegistration = builder.RegisterType(mi.ClassType).As(mi.InterfaceType);
            switch (mi.Scope)
            {
                case InstanceScope.PerDependency:
                    typeRegistration.InstancePerDependency();
                    break;
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
                    throw new Exception($"AutoRegister does not know how to handle scope {mi.Scope} for {mi.ClassType.FullName}");
            }
        }
    }

    internal record MatchedInterface
    {
        internal MatchedInterface(Type classType, Type interfaceType, InstanceScope scope)
        {
            ClassType = classType;
            InterfaceType = interfaceType;
            Scope = scope;
        }
        internal Type ClassType { get; }
        internal Type InterfaceType { get; }
        internal InstanceScope Scope { get; }
    }
}