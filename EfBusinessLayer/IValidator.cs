namespace EfBusinessLayer
{
    public interface IValidator<T>
    {
        bool Validate(T entity);
    }
}
