namespace MotiNet
{
    public class GenericError
    {
        public GenericError() { }

        public GenericError(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public string Code { get; set; }

        public string Description { get; set; }
    }
}
