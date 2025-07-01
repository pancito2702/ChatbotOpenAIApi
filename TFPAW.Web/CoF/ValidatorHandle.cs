
namespace TFPAW.Web.CoF
{
    public class ValidatorHandle : IValidatorHandler
    {
        private IValidatorHandler? _nextHandler;
        public virtual string Handle(string message, IFormFileCollection? files)
        {

            string output = _nextHandler?.Handle(message, files) ?? string.Empty;

            if (!string.IsNullOrEmpty(output))
            {
                return output;
            }

         
          

            return output;
        }

        public void SetNext(IValidatorHandler handler)
        {
            _nextHandler = handler;
        }
    }
}
