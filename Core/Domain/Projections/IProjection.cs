namespace Core.Domain.Projections
{
    public interface IProjection
    {
        void When(object e);
    }
}