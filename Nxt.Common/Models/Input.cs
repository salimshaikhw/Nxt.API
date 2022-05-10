using Nxt.Common.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Nxt.Common.Models
{
    public abstract class Input<T> where T : Input<T>
    {
        private readonly ValidationContext _validationContext;
        private readonly T _entity;

        protected Input()
        {
            _entity = (T)this;
            _validationContext = new ValidationContext(_entity, null, null);
        }

        public virtual bool IsValid()
        {
            return Validator.TryValidateObject(_entity, _validationContext, null, true);
        }

        public virtual void Validate()
        {
            try
            {
                Validator.ValidateObject(_entity, _validationContext, true);
            }
            catch (ValidationException ex)
            {
                throw new ServiceException(ex.ValidationResult.ErrorMessage, ExceptionCodes.Validation);
            }
        }

        public T Clone()
        {
            return (T)MemberwiseClone();
        }
    }
}
