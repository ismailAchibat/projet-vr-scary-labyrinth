using UnityEngine;
using TMPro;

public class CoffreSurprise : MonoBehaviour
{
    private bool estOuvert = false;
    public TMP_Text texteResultat; // Compatible UI et 3D
    private Animator anim; // La référence au système d'animation

    void Start()
    {
        // On récupère automatiquement l'Animator sur le même objet
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

        // 1. Lancer l'animation (Si l'animateur existe)
        if (anim != null)
        {
            anim.SetTrigger("Ouvrir");
        }

        // 2. Gestion du Timer et Bonus
        GameTimer manager = FindObjectOfType<GameTimer>();
        if (manager == null) return;

        int tirage = Random.Range(0, 3);

        if (tirage == 0)
        {
            manager.ModifierTemps(20f);
            AfficherTexte("BONUS\n+20s", Color.green);
        }
        else if (tirage == 1)
        {
            manager.ModifierTemps(-20f);
            AfficherTexte("PIEGE !\n-20s", Color.red);
        }
        else
        {
            manager.RamasserCle();
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