public class ParseUtil
{
    private static ParseUtil instance;

    public static ParseUtil Instance()
    {
        if (instance == null)
        {
            instance = new ParseUtil();
        }

        return instance;
    }

    // int로 Parse 가능하면 결과값을, 그렇지 않으면 -1을 반환 
    public int parseInt(string target)
    {
        int result;
        if (int.TryParse(target, out result))
            return result;
        else
            return -1;
    }

    public float parseFloat(string target)
    {
        float result;
        if (float.TryParse(target, out result))
            return result;
        else
            return -1f;
    }

}
