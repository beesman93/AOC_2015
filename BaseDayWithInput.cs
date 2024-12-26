#pragma warning disable IDE0130 // Namespace does not match folder structure -- it's extending the helper
namespace AoCHelper;
#pragma warning restore IDE0130

public abstract class BaseDayWithInput : BaseDay
{
    protected readonly string[] _input;
    public BaseDayWithInput()
    {
        _input = File.ReadAllLines(InputFilePath);
    }
}
