namespace CoffeeToolkit.DependencyInjection;

/*
 * This class is made available under the Unlicense.
 * You are free to use, modify, and distribute the class as you see fit,
 * without any restrictions or conditions.
 * The rest of the application is not covered by this license.
 */
public enum InstanceScope
{
    Undefined = 0,
    PerDependency = 1,
    Single = 2,
    Lifetime = 3,
    PerRequest = 4,
}