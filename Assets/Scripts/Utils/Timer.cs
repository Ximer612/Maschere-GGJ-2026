
public class Timer
{
    private float counter, timer;
    private bool shouldTick, shouldLoop;
    public bool isEnd;
    public bool ShouldTick { get => shouldTick; }

    public Timer(float inTimer, bool shouldLoop = false, bool shouldTick = true)
    {
        timer = inTimer;
        counter = inTimer;
        this.shouldTick = shouldTick;
        this.shouldLoop = shouldLoop;
        isEnd = false;
    }

    public bool Tick(float deltaTime)
    {
        if (!shouldTick)
            return false;

        counter -= deltaTime;

        if (counter < 0)
        {
            if(shouldLoop)
            {
                Reset();
            }
            else
            {
                isEnd = true;
                shouldTick = false;
            }

            return true;
        }

        return false;
    }

    public void Reset()
    {
        counter = timer;
        shouldTick = true;
        isEnd = false;
    }

    public void Set(float newTimer)
    {
        timer = newTimer;
    }

    public void SetShouldTick(bool shouldTick)
    {
        this.shouldTick = shouldTick;
    }

    public float GetCounter() => counter;
    public float GetTimer() => timer;
}
