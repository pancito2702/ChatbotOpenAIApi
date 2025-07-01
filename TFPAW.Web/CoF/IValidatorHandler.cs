namespace TFPAW.Web.CoF
{
    public interface IValidatorHandler
    {
        void SetNext(IValidatorHandler handler);
        string Handle(string message, IFormFileCollection? files);

    }
}
