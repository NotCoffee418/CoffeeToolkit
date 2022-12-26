namespace CoffeeToolkit.DependencyInjection;

/*
 * This class is made available under the Unlicense.
 * You are free to use, modify, and distribute the class as you see fit,
 * without any restrictions or conditions.
 * The rest of the application is not covered by this license.
 */
public class DependencyScopeAttribute : Attribute
{
    public DependencyScopeAttribute(InstanceScope scopeDef)
    {
        Scope = scopeDef;
    }

    public InstanceScope Scope { get; set; }
}