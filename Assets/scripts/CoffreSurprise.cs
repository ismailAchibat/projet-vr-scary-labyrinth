using UnityEngine;
using TMPro;

public class CoffreSurprise : MonoBehaviour
{
    private bool estOuvert = false;
    public TMP_Text texteResultat;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !estOuvert)
        {
            OuvrirCoffre();
        }
    }

    void OuvrirCoffre()
    {
        estOuvert = true;

        // 1. Lancer l'animation
        if (anim != null)
        {
            anim.SetTrigger("Ouvrir");
        }

        // 2. Trouver le Manager
        GameTimer manager = FindObjectOfType<GameTimer>();
        if (manager == null) return;

        // 3. Tirage au sort
        int tirage = Random.Range(0, 3);

        if (tirage == 0)
        {
            // Bonus temps
            manager.ModifierTemps(20f);
            AfficherTexte("BONUS\n+20s", Color.green);
        }
        else if (tirage == 1)
        {
            // Malus temps
            manager.ModifierTemps(-20f);
            AfficherTexte("PIEGE !\n-20s", Color.red);
        }
        else
        {

            // On ajoute +1 au compteur de clés du manager
            manager.clesTrouvees += 1;

            AfficherTexte("CLE\nTROUVEE !", Color.yellow);
        }
    }

    void AfficherTexte(string message, Color couleur)
    {
        if (texteResultat != null)
        {
            texteResultat.text = message;
            texteResultat.color = couleur;
            texteResultat.gameObject.SetActive(true);
        }
    }
}