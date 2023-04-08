using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMapParser: MonoBehaviour
{
    
    private string[][][] caveMapInfo;
    private ParseUtil parseUtil = ParseUtil.Instance();



    public Cavern getRootCavern(TextAsset caveCsv)
    {
        caveMapInfo = parseCavernData(caveCsv);
        Cavern root = new Cavern(caveMapInfo[0][0]);
        Cavern[] child = composeCarvens(0, 0, root.routeCnt);
        root.setNextCarven(child);

        return root;
    }

    private Cavern[] composeCarvens(int stage, int start, int count)
    {
        List<Cavern> caverns = new List<Cavern>();
        int accumulate = 0; 
        
        for (int i = 0; i < start; i++)
        {
            accumulate += parseUtil.parseInt(caveMapInfo[stage][i][0]);
        }
        
        for (int s = accumulate; s < start + count; s++)
        {
            string[] cell = caveMapInfo[stage + 1][s];
            int routeCount = parseUtil.parseInt(cell[0]);

            if (routeCount >= 0)
            {
                Cavern cavern = new Cavern(cell);

                if (routeCount > 0 && routeCount < 999)
                    cavern.setNextCarven(composeCarvens(stage + 1, accumulate, cavern.routeCnt));

                caverns.Add(cavern);
                accumulate += cavern.routeCnt;
            }
        }
        return caverns.ToArray();
    }


    private string[][][] parseCavernData(TextAsset csv)
    {
        List<string> rowList = new List<string>();
  
        int rowLength = 6;
        int start = 11;

        string[] rows = csv.text.Split(new char[] { '\n' });
        
        for (int i = 0; i < rowLength; i++)
        {
            rowList.Add(rows[start + i]);
        }

        return parseStageData(rowList);
    }

    private string[][][] parseStageData(List<string> rowList)
    {
        List<string[][]> carvenDataList = new List<string[][]>();
        List<string[]> splitedRowList = new List<string[]>();
        int depth = -1;
        
        foreach (string row in rowList)
        {
            splitedRowList.Add(row.Split(new char[] { ',' }));
            if (depth < 0) depth = splitedRowList[0].Length;
        }
 
        for (int i = 0; i < depth; i++)
        {
            List<string[]> column = new List<string[]>();

            for (int r = 0; r < rowList.Count; r++)
            {
                column.Add(splitedRowList[r][i].Split(new char[] { '#' }));
            }

            carvenDataList.Add(column.ToArray());
        }
        return carvenDataList.ToArray();
    }

}
