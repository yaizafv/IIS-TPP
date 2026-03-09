
namespace ForLoop;

public class ForLoop
{
    public static void ForLoop1(Func<int> initial, Func<bool> cond, Func<int> it, Action bd)
    {
        int i = -10;  //This intialization is necessary to compile but assigned value 
        var initialize = () => i = 0;
        var condition = () => i < 10;
        var iteration = () => i = i + 1;
        var body = () =>
        {
            //Do something 
            Console.WriteLine(i);
        };
        ForLoop1(initialize, condition, iteration, body);
    }
}
