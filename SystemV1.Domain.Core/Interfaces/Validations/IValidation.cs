using FluentValidation;

namespace SystemV1.Domain.Core.Interfaces.Validations
{
    public interface IValidation<TEntity> : IValidator<TEntity>
    {
        public void RulesForAdd();

        public void RulesForUpdate();

        public void RulesForDelete();
    }
}