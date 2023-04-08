[System.Serializable]
public class Cavern
{
    public int routeCnt;
    public int talkId;
    public string soundPosition;
    public int soundIndex;
    public float volume;
    public int soundIndex2;
    public float volume2;
    public bool isObject = false;
    public int objectIndex;
    public bool isSave;

    public Cavern prev = null;
    public Cavern[] next = null;

    private ParseUtil parseUtil = ParseUtil.Instance();

    public Cavern(string[] info)
    {
        routeCnt = parseUtil.parseInt(info[0]);
        if (info[1].Equals("T"))
        {
            talkId = parseUtil.parseInt(info[2]);
        }
      
        if (info[3].Equals("A"))
        {
            soundPosition = info[4];
            soundIndex = parseUtil.parseInt(info[5]);
            volume = parseUtil.parseFloat(info[6]);

            if (!info[7].Equals(""))
            {
                soundIndex2 = parseUtil.parseInt(info[7]);
                volume2 = parseUtil.parseFloat(info[8]);
            }
        }

        if (!info[9].Equals(""))
        {
            isObject = true;
            objectIndex = parseUtil.parseInt(info[10]);
        }

        isSave = info[11].Equals("S");
    }



    public void setPrevCarven(Cavern cavern)
    {
        prev = cavern;
    }

    public void setNextCarven(Cavern[] cavernList)
    {
        next = cavernList;
    }

    public Cavern back()
    {
        return prev;
    }

    public Cavern proceed(int idx)
    {
        return next[idx];
    }
}
