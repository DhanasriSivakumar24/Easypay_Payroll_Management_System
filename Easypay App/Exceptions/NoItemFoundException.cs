namespace Easypay_App.Exceptions
{
    public class NoItemFoundException:Exception
    {
        public NoItemFoundException()
        {
            
        }
        public NoItemFoundException(string message):base(message)
        {
            Console.WriteLine("No Element Found in the Id");
        }
    }
}
