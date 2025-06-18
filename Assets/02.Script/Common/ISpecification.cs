using UnityEngine;

public interface ISpecification <T>
{
    public bool IsStatisfiedBy(T value);
    public string ErrorMessage { get; }
}
