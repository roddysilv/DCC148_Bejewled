using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController board;
    public List<Sprite> sprites = new List<Sprite>();
    private GameObject[,] pecas;
    public GameObject peca;
    private int quantidade_x = 22;
    private int quantidade_y = 12;
    //private bool trocar_peca;

    // Start is called before the first frame update
    void Start()
    {
        board = GetComponent<GameController>();
         Board(
            peca.GetComponent<SpriteRenderer>().bounds.size.x,
            peca.GetComponent<SpriteRenderer>().bounds.size.y
        );
    }

    // Update is called once per frame
    void Update() { }

    private void  Board(float  x_desloc, float  y_desloc)
    {
        pecas = new GameObject[quantidade_x, quantidade_y];
        float X = -8.15f;
        float Y = -4.25f;
        Sprite[] aux = new Sprite[quantidade_y];
        Sprite aux_sprite = null;
        for (int x = 0; x < quantidade_x; x++)
        {
            for (int y = 0; y < quantidade_y; y++)
            {
                GameObject aux_peca = Instantiate(
                    peca,
                    new Vector3(X + ( x_desloc * x), Y + ( y_desloc * y), 0),
                    peca.transform.rotation
                );
                pecas[x, y] = aux_peca;
                aux_peca.transform.parent = transform;
                List<Sprite> novas_pecas = new List<Sprite>();
                novas_pecas.AddRange(sprites);
                novas_pecas.Remove(aux[y]);
                novas_pecas.Remove(aux_sprite);
                Sprite newSprite = novas_pecas[Random.Range(0, novas_pecas.Count)];
                aux_peca.GetComponent<SpriteRenderer>().sprite = newSprite;
                aux[y] = newSprite;
                aux_sprite = newSprite;
            }
        }
    }

    private Sprite GeraPeca(int x, int y)
    {
        List<Sprite> novas_pecas = new List<Sprite>();
        novas_pecas.AddRange(sprites);
        if (x > 0)
        {
            novas_pecas.Remove(pecas[x - 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (x < quantidade_x - 1)
        {
            novas_pecas.Remove(pecas[x + 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (y > 0)
        {
            novas_pecas.Remove(pecas[x, y - 1].GetComponent<SpriteRenderer>().sprite);
        }
        return novas_pecas[Random.Range(0, novas_pecas.Count)];
    }

    private IEnumerator GeraPecaVazio(int x, int y)
    {
        List<SpriteRenderer> aux = new List<SpriteRenderer>();
        int vazio = 0;
        for (int i = y; i < quantidade_y; i++)
        {
            SpriteRenderer aux2 = pecas[x, i].GetComponent<SpriteRenderer>();
            if (aux2.sprite == null)
            {
                vazio++;
            }
            aux.Add(aux2);
        }
        for (int i = 0; i < vazio; i++)
        {
            yield return new WaitForSeconds(0f);
            for (int j = 0; j < aux.Count - 1; j++)
            {
                aux[j].sprite = aux[j + 1].sprite;
                aux[j + 1].sprite = GeraPeca(x, quantidade_y - 1);
            }
        }
    }

    public IEnumerator EhVazio()
    {
        for (int x = 0; x < quantidade_x; x++)
        {
            for (int y = 0; y < quantidade_y; y++)
            {
                if (pecas[x, y].GetComponent<SpriteRenderer>().sprite == null)
                {
                    yield return StartCoroutine(GeraPecaVazio(x, y));
                    break;
                }
            }
        }
        for (int x = 0; x < quantidade_x; x++)
        {
            for (int y = 0; y < quantidade_y; y++)
            {
                pecas[x, y].GetComponent<PieceController>().AuxRemove();
            }
        }
    }
}
