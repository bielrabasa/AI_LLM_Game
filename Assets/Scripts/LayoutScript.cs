using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LayoutScript : MonoBehaviour
{
    public Material bmat;
    public Material umat;
    public Transform blockContainer;

    bool won = false;

    int uBlocks = 0;
    public int levelCharged = -1;

    LLM_Comunication llm;


    void Start()
    {
        llm = FindObjectOfType<LLM_Comunication>();

        won = false;
        LoadLevel(0);
    }

    private void LoadLevel(int num)
    {
        foreach (Transform child in blockContainer) 
        {
            Destroy(child.gameObject);
        }

        uBlocks = 0;

        var lines = transform.GetChild(num).GetComponent<LevelLayout>().lines;
        levelCharged = num;

        Vector3 pos = new Vector3(-15, 30, 0);
        foreach (string line in lines)
        {
            foreach (char c in line)
            {
                if (c == '.') pos.x += 1.5f;
                else if (c == '-') pos.x += 3f;
                else if ("BSU".Contains(c))
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = pos;
                    cube.transform.localScale = new Vector3(2.5f, 1.2f, 1);
                    cube.transform.parent = blockContainer;
                    pos.x += 3f;
                    cube.GetComponent<Renderer>().material = bmat;

                    if (c == 'B') cube.tag = "BLOCK";
                    else if (c == 'S') cube.tag = "BLOCK_S";
                    else if (c == 'U')
                    {
                        cube.tag = "BLOCK_U";
                        cube.GetComponent<Renderer>().material = umat;
                        uBlocks++;
                    }
                }
            }
            pos.x = -15;
            pos.y -= 2;
        }

        won = false;
    }

    private void Update()
    {
        if (won) return;

        if(blockContainer.childCount <= uBlocks)
        {
            foreach (Transform child in blockContainer)
            {
                if (child.CompareTag("BLOCK") || child.CompareTag("BLOCK_S")) return;
            }

            Win();
        }
    }

    void Win()
    {
        won = true;

        if(levelCharged + 1 == transform.childCount)
        {
            StartCoroutine(EndWaitBoss(true));
            return;
        }

        FindObjectOfType<BallMovement>().ResetBall(true);
        StartCoroutine(WaitForLoadLevel(levelCharged + 1));
    }

    IEnumerator WaitForLoadLevel(int num)
    {
        yield return new WaitForSeconds(2);
        LoadLevel(num);
    }

    public void Lose()
    {
        StartCoroutine(EndWaitBoss(false));
    }

    IEnumerator EndWaitBoss(bool win)
    {
        var bm = FindObjectOfType<BallMovement>();

        if (win) llm.BossTalk(bm.lives, 4);
        else llm.BossTalk(0, -1);
        
        bm.gameObject.SetActive(false);
        

        while (llm.isWriting) yield return null;

        SceneManager.LoadScene("MainScene");
    }

    public void RearangeBricks()
    {
        int vertical = Random.Range(0, 4);

        foreach (Transform child in blockContainer) 
        {
            if(vertical == 0 && vertical == 1)
            {
                float y = child.position.y;
                y += 2 * (23 - y); //mirror from horizontal center line
                child.position = new Vector3(child.position.x, y, 0);
            }
            else if(vertical == 2)
            {
                float x = child.position.x;
                x *= -1; //mirror from center
                child.position = new Vector3(x, child.position.y, 0);
            }
            else
            {
                float y = child.position.y;
                float x = child.position.x;
                y += 2 * (23 - y); //mirror from horizontal center line
                x *= -1; //mirror from center
                child.position = new Vector3(x, y, 0);
            }
        }
    }
}
