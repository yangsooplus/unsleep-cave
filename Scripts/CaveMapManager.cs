using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMapManager : MonoBehaviour
{
    public TextAsset caveCsv;
    public GameObject backButton;
    public CaveMapRenderer caveMapRenderer;
    private Stack<Cavern> stack = new Stack<Cavern>();
    private Cavern rootCavern;
    public Cavern currentCavern;


    private void Start()
    {
        rootCavern = new CaveMapParser().getRootCavern(caveCsv);
        currentCavern = rootCavern;
        caveMapRenderer.renderCavern(currentCavern);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero, 0f);

            if (hit.collider != null)
            {
                Hole hole = hit.collider.gameObject.GetComponent<Hole>();
                if (hole != null)
                {
                    (hole as CaveClickable).onClick(this);
                }
            }
        }
    }

    public void proceed(int routeIndex)
    {
        if (caveMapRenderer.moving) return;

        stack.Push(currentCavern);
        currentCavern = currentCavern.next[routeIndex];
        
        caveMapRenderer.proceed(currentCavern);

        if (!backButton.activeSelf) backButton.SetActive(true);

        if (currentCavern.routeCnt == 999)
        {
            backButton.SetActive(false);
        }
    }

    public void back()
    {
        if (caveMapRenderer.moving) return;

        currentCavern = stack.Pop();
        caveMapRenderer.back(currentCavern);

        if (stack.Count == 0) backButton.SetActive(false);
    }

    public void returnLastPoint()
    {
        if (stack.Count == 0 || stack.Peek().isSave) 
            return;

        while (!stack.Peek().isSave)
        {
            stack.Pop();
        }
        currentCavern = stack.Pop();
        caveMapRenderer.renderCavern(currentCavern);

        if (stack.Count == 0) 
            backButton.SetActive(false);

    }

}
