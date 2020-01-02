namespace Selene.Core
{
    public interface IClient
    {
        string Identity { get; set; }

        dynamic Variables { get; set; }
    }
}
