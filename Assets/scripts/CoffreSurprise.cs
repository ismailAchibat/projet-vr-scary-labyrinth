using UnityEngine;
using TMPro; // Pour afficher le résultat au dessus du coffre

public class CoffreSurprise : MonoBehaviour
{
    private bool estOuvert = false;
    public TMP_Text texteResultat; // Optionnel : pour afficher "+20s" au dessus du coffre

    // Cette fonction se déclenche quand la main ou le joueur touche le coffre
    private void OnTriggerEnter(Collider other)
    {
        // On vérifie que c'est le joueur et que le coffre n'est pas déjà ouvert
        if (other.CompareTag("Player") && !estOuvert)
        {
            OuvrirCoffre();
        }
    }

    void OuvrirCoffre()
    {
        estOuvert = true;

        // 1. Trouver le GameManager pour agir sur le jeu
        GameTimer manager = FindObjectOfType<GameTimer>();
        if (manager == null) return;

        // 2. Tirage au sort (0, 1 ou 2)
        int tirage = Random.Range(0, 3); // Le 3 est exclu, donc ça donne 0, 1 ou 2

        if (tirage == 0)
        {
            // Bonus : +20 secondes
            manager.ModifierTemps(20f);
            AfficherTexte("BONUS\n+20s", Color.green);
        }
        else if (tirage == 1)
        {
            // Malus : -20 secondes
            manager.ModifierTemps(-20f);
            AfficherTexte("PIEGE !\n-20s", Color.red);
        }
        else
        {
            // Cadeau : La clé
            manager.RamasserCle();
            AfficherTexte("CLE\nTROUVEE !", Color.yellow);
        }

        // Ici, tu pourras ajouter une animation d'ouverture plus tard
        // GetComponent<Animator>().SetTrigger("Ouvrir");
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