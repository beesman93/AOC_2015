namespace AoCHelper;

public abstract class BaseDayWithInput : BaseDay
{
    protected readonly string[] _input;
    public BaseDayWithInput()
    {
        _input = File.ReadAllLines(InputFilePath);
    }
}
