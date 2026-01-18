using UnityEngine;
using TMPro;

public class VictoireManager : MonoBehaviour
{
    [Header("UI de Victoire")]
    public GameObject canvasGameWon;
    public TMP_Text texteMessage;

    [Header("Références")]
    public Transform cameraJoueur; 

    private bool aGagne = false;

    // FONCTION APPELÉE PAR LE BOUTON DE FIN (OnClick)
    public void DeclencherVictoire()
    {
        if (aGagne) return;
        aGagne = true;

        // 1. Arrêter le chronomètre via le GameTimer
        GameTimer manager = FindObjectOfType<GameTimer>();
        if (manager != null)
        {
            manager.Victoire(); // Utilise la fonction Victoire que tu as déjà dans ton script
        }

        // 2. Afficher et positionner l'écran de victoire comme le Game Over
        if (canvasGameWon != null && cameraJoueur != null)
        {
            canvasGameWon.SetActive(true);

            // Positionnement à 2 mètres devant la caméra (comme ton script GameTimer)
            canvasGameWon.transform.position = cameraJoueur.position + cameraJoueur.forward * 2.0f;
            
            // On le tourne vers le joueur
            canvasGameWon.transform.LookAt(cameraJoueur);
            
            // Correction de la rotation pour que le texte soit lisible (180°)
            canvasGameWon.transform.Rotate(0, 180, 0);

            if (texteMessage != null)
            {
                texteMessage.text = "FÉLICITATIONS !\nVOUS AVEZ GAGNÉ !";
            }
        }

        // 3. Arrêter le temps (Même logique que ton Game Over)
        Time.timeScale = 0;
        
        Debug.Log("VICTOIRE ! Écran affiché et temps mis en pause.");
    }
}