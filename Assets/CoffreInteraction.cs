using UnityEngine;
using TMPro;

public class CoffreInteraction : MonoBehaviour
{
    // --- Variables d'affichage dans l'inspecteur ---
    public Animator anim;
    public string triggerOuverture = "Ouvrir";
    public TMP_Text texteResultat;
    public GameObject boutonAEnlever;

    private bool estDejaOuvert = false;

    // Fonction appelée par le bouton du Canvas (OnClick)
    public void ActionOuvrirCoffre()
    {
        if (estDejaOuvert) return;
        estDejaOuvert = true;

        // 1. Déclenche l'animation
        if (anim != null)
        {
            anim.SetTrigger(triggerOuverture);
        }

        // 2. Cache le bouton pour qu'on ne puisse plus cliquer
        if (boutonAEnlever != null)
        {
            boutonAEnlever.SetActive(false);
        }

        // 3. Calcul de la surprise
        DeterminerSurprise();
    }

    private void DeterminerSurprise()
    {
        // On cherche le script GameTimer dans la scène
        GameTimer manager = FindObjectOfType<GameTimer>();
        int tirage = Random.Range(0, 3);

        if (tirage == 0)
        {
            if (manager != null) manager.ModifierTemps(20f);
            AfficherMessage("BONUS\n+20s", Color.green);
        }
        else if (tirage == 1)
        {
            if (manager != null) manager.ModifierTemps(-20f);
            AfficherMessage("PIEGE !\n-20s", Color.red);
        }
        else
        {
            if (manager != null) manager.clesTrouvees += 1;
            AfficherMessage("CLE\nTROUVEE !", Color.yellow);
        }
    }

    private void AfficherMessage(string message, Color couleur)
    {
        if (texteResultat != null)
        {
            texteResultat.text = message;
            texteResultat.color = couleur;
            texteResultat.gameObject.SetActive(true);
        }
    }
}