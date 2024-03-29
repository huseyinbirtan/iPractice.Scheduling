namespace iPractice.SharedKernel.Exceptions
{
    public class DomainValidationException : Exception
    {
        public DomainValidationException(string message) : base(message) { }

        public DomainValidationException()
        {

        }

        public void FieldException(string fieldName)
        {
            base.Source = $"Following field can not be 0 or empty: {fieldName}";
        }
    }
}
