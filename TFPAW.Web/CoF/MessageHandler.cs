namespace TFPAW.Web.CoF
{
    public class MessageHandler : ValidatorHandle 
    {

        public override string Handle(string message, IFormFileCollection? files)
        {
            if (string.IsNullOrEmpty(message))
            {
                return "El mensaje no puede estar vacio";
            }

           return base.Handle(message, files);
        }

    }
}
