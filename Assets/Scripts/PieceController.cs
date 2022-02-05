using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    private static PieceController anterior = null;
    private SpriteRenderer  sprite_aux;
    private bool comb3 = false;
    private bool selecionou = false;
    private Vector2[] prox_pecas = new Vector2[] {  Vector2.up,
													Vector2.down,
													Vector2.left,
													Vector2.right };

    // Start is called before the first frame update
    void Start()
    {
         sprite_aux = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() { }

    public void Troca(SpriteRenderer sprite)
    {
        if ( sprite_aux.sprite == sprite.sprite)
        {
            return;
        }
        Sprite aux_sprite = sprite.sprite;
        sprite.sprite =  sprite_aux.sprite;
         sprite_aux.sprite = aux_sprite;
    }

    private void Selecionar()
    {
        selecionou = true;
		sprite_aux.color = new Color(.5f, .5f, .5f, 1.0f);
        anterior = gameObject.GetComponent<PieceController>();
    }

    private void Deselecionar()
    {
        selecionou = false;
        sprite_aux.color = Color.white;
        anterior = null;
    }

    void OnMouseDown()
    {
        if (selecionou)
        {
            Deselecionar();
        }
        else
        {
            if (anterior == null)
            {
                Selecionar();
            }
            else
            {
                if (GetProx().Contains(anterior.gameObject))
                {
                    Troca(anterior. sprite_aux);
                    anterior.AuxRemove();
                    anterior.Deselecionar();
                    AuxRemove();
                }
                else
                {
                    anterior.GetComponent<PieceController>().Deselecionar();
                    Selecionar();
                }
            }
        }
    }

    private List<GameObject> GetProx()
    {
        List<GameObject> aux_vet = new List<GameObject>();
        for (int i = 0; i < prox_pecas.Length; i++)
        {
            aux_vet.Add(AuxGetProx(prox_pecas[i]));
        }
        return aux_vet;
    }

    private GameObject AuxGetProx(Vector2 vet)
    {
        RaycastHit2D  rc = Physics2D.Raycast(transform.position, vet);
        if ( rc.collider != null)
        {
            return  rc.collider.gameObject;
        }
        return null;
    }

    public void AuxRemove()
    {
        if ( sprite_aux.sprite == null){
            return;
		}
        RemoveComb(new Vector2[2] { Vector2.left, Vector2.right });
        RemoveComb(new Vector2[2] { Vector2.up, Vector2.down });
        if (comb3)
        {
             sprite_aux.sprite = null;
            comb3 = false;
            StopCoroutine(GameController.board.EhVazio());
            StartCoroutine(GameController.board.EhVazio());
        }
    }

    private List<GameObject> AchaComb(Vector2 vet)
    {
        List<GameObject> aux_vet = new List<GameObject>();
        RaycastHit2D  rc = Physics2D.Raycast(transform.position, vet);
        while (
             rc.collider != null &&  rc.collider.GetComponent<SpriteRenderer>().sprite ==  sprite_aux.sprite
        )
        {
            aux_vet.Add( rc.collider.gameObject);
             rc = Physics2D.Raycast( rc.collider.transform.position, vet);
        }
        return aux_vet;
    }

    private void RemoveComb(Vector2[]  vet)
    {
        List<GameObject> aux_vet = new List<GameObject>();
        for (int i = 0; i <  vet.Length; i++)
        {
            aux_vet.AddRange(AchaComb( vet[i]));
        }
        if (aux_vet.Count >= 2)
        {
            for (int i = 0; i < aux_vet.Count; i++)
            {
                aux_vet[i].GetComponent<SpriteRenderer>().sprite = null;
            }
            comb3 = true;
        }
    }
}
