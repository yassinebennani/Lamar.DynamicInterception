using System.Reflection;

namespace Lamar.DynamicInterception
{
    public interface IArgument
    {
        object Value { get; set; }

        ParameterInfo ParameterInfo { get; }

        ParameterInfo InstanceParameterInfo { get; }
    }
}