# 공개용 잠이 안 와 동굴 퍼즐

### WebGL 빌드 페이지

https://yangsooplus.github.io/unsleep-cave/

- 스테레오 사운드를 지원하지 않습니다.
- 웹 환경에서 구동할 수 있도록 경량화하여 실제 게임과는 다릅니다.
- 실제 플레이는 아래 영상을 확인해주세요.

### 플레이 영상

[![플레이 영상](http://img.youtube.com/vi/i8Ix6eqlh7Y/0.jpg)](https://youtu.be/i8Ix6eqlh7Y)

## 코드 설명

### csv 데이터로 맵 구성하기

![9](https://user-images.githubusercontent.com/69582122/230713401-5b5f577e-5a6b-4359-bf7e-3b0ce13dab84.jpg)
![csv](https://user-images.githubusercontent.com/69582122/230713560-7fcb1f99-c0d8-43d7-bcf7-b9846dae527c.png)

- [csv](https://github.com/yangsooplus/unsleep-cave/blob/main/cave_table.csv)로 동굴 맵 데이터를 표현
- csv 한 칸은 `Carven` 객체 하나를 나타냄
- [CaveMapParser](https://github.com/yangsooplus/unsleep-cave/blob/main/Scripts/CaveMapParser.cs)에서 **DFS**로 csv를 탐색하여 `Cavern` **Tree**로 전체 맵을 구성

```C#
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
}
```

### 전진 / 후퇴하기

- [CaveMapManager](https://github.com/yangsooplus/unsleep-cave/blob/main/Scripts/CaveMapManager.cs)에서 전진할때마다 지나온 길(Cavern)을 Stack에 Push한다.
```C#

    public void proceed(int routeIndex) // routeIndex: 전진하려는 길의 인덱스
    {
        if (caveMapRenderer.moving) return;

        stack.Push(currentCavern); // 지나온 길을 push
        currentCavern = currentCavern.next[routeIndex]; // 현재 위치를 전진할 길로 이동 
        caveMapRenderer.proceed(currentCavern); // 화면 + 소리 갱신

        (생략)
    }

```

- 후퇴할 때에는 Stack에서 pop한 길로 이동한다.
```C#
    public void back()
    {
        if (caveMapRenderer.moving) return;

        currentCavern = stack.Pop();
        caveMapRenderer.back(currentCavern); // 화면 + 소리 갱신

        if (stack.Count == 0) backButton.SetActive(false); // 스택이 비었을 경우, 뒤로 후퇴하는 버튼을 숨긴다.
    }
 
```
- *중간포인트로* 버튼을 눌러 빨간색으로 표시된 시점으로 되돌아갈 수 있다.
![point](https://user-images.githubusercontent.com/69582122/230714124-46e68fde-1dcc-4fb6-9d9f-c57d48e16186.png)
```C#
     public void returnLastPoint()
    {
        if (stack.Count == 0 || stack.Peek().isSave) return; // 지나온 길이 없거나 이미 중간포인트에 있는 경우

        while (!stack.Peek().isSave) // 중간 포인트에 도달할 때까지 지나온 길을 pop한다.
        {
            stack.Pop();
        }
        currentCavern = stack.Pop(); // 중간포인트
        caveMapRenderer.renderCavern(currentCavern); // 화면 + 소리 갱신

        if (stack.Count == 0) backButton.SetActive(false); // 스택이 비었을 경우, 뒤로 후퇴하는 버튼을 숨긴다.
    }
```