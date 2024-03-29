using iPractice.SharedKernel.Exceptions;

namespace iPractice.SharedKernel.BaseClasses
{
    public abstract class BaseEntity<TId>
    {
        public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();

        public TId Id { get; set; }

        protected DomainValidationException FieldException(string fieldName) {
            var exception = new DomainValidationException();
            exception.FieldException(fieldName);
            return exception;
        }
    }
}
