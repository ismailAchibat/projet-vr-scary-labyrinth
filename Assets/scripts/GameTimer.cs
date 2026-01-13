using UnityEngine;
using TMPro; // Nécessaire pour le texte

public class GameTimer : MonoBehaviour
{
    public float tempsRestant = 180f; // 180 secondes = 3 minutes
    private bool chronometreActif = false;

    [Header("Glisser les objets ici")]
    public TextMeshProUGUI texteDuChrono; // Le texte qui défile
    public GameObject ecranGameOver;      // L'écran de fin
    public Transform cameraJoueur;        // La caméra VR

    void Start()
    {
        chronometreActif = true;
        // On s'assure que l'écran Game Over est caché au début
        if (ecranGameOver != null) ecranGameOver.SetActive(false);
    }

    void Update()
    {
        if (chronometreActif)
        {
            if (tempsRestant > 0)
            {
                tempsRestant -= Time.deltaTime;
                AfficherTemps(tempsRestant);
            }
            else
            {
                tempsRestant = 0;
                chronometreActif = false;
                LancerGameOver();
            }
        }
    }

    void AfficherTemps(float temps)
    {
        temps += 1;
        float minutes = Mathf.FloorToInt(temps / 60);
        float seconds = Mathf.FloorToInt(temps % 60);
        if (texteDuChrono != null)
            texteDuChrono.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void LancerGameOver()
    {
        Debug.Log("PERDU !");

        // 1. Positionner l'écran Game Over devant les yeux du joueur
        if (ecranGameOver != null && cameraJoueur != null)
        {
            ecranGameOver.SetActive(true);
            // On le place 2 mètres devant la caméra
            ecranGameOver.transform.position = cameraJoueur.position + cameraJoueur.forward * 2.0f;
            // On le tourne vers le joueur
            ecranGameOver.transform.LookAt(cameraJoueur);
            // Correction de la rotation (sinon le texte est à l'envers)
            ecranGameOver.transform.Rotate(0, 180, 0);
        }

        // 2. Arrêter le jeu (pause)
        Time.timeScale = 0;
    }

    // Fonction à appeler quand le joueur trouve la sortie
    public void Victoire()
    {
        chronometreActif = false;
        Debug.Log("GAGNÉ ! Le temps s'arrête.");
    }

    // Gestion du coffre

    [Header("Inventaire")]
    public bool aLaCle = false; // Pour savoir si le joueur a la clé

    // Fonction pour modifier le temps (Bonus ou Malus)
    public void ModifierTemps(float secondes)
    {
        tempsRestant += secondes;

        // Petit effet visuel dans la console
        if (secondes > 0) Debug.Log("BONUS ! +" + secondes + " secondes");
        else Debug.Log("PENALITE ! " + secondes + " secondes");
    }

    // Fonction pour ramasser la clé
    public void RamasserCle()
    {
        aLaCle = true;
        Debug.Log("CLE RECUPEREE !");
    }
}