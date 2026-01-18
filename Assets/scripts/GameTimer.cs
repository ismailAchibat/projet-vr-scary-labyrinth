using UnityEngine;
using TMPro; 


public class GameTimer : MonoBehaviour
{
    [Header("Progression")]
    public int clesTrouvees = 0;
    public int portesOuvertes = 0;
    public float tempsRestant = 180f; // 180 secondes = 3 minutes
    private bool chronometreActif = false;

    [Header("Glisser les objets ici")]
    public TextMeshProUGUI texteDuChrono; // Le texte qui d�file
    public GameObject ecranGameOver;      // L'�cran de fin
    public Transform cameraJoueur;        // La cam�ra VR

    void Start()
    {
        chronometreActif = true;
        // On s'assure que l'�cran Game Over est cach� au d�but
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

        // 1. Positionner l'�cran Game Over devant les yeux du joueur
        if (ecranGameOver != null && cameraJoueur != null)
        {
            ecranGameOver.SetActive(true);
            // On le place 2 m�tres devant la cam�ra
            ecranGameOver.transform.position = cameraJoueur.position + cameraJoueur.forward * 2.0f;
            // On le tourne vers le joueur
            ecranGameOver.transform.LookAt(cameraJoueur);
            // Correction de la rotation (sinon le texte est � l'envers)
            ecranGameOver.transform.Rotate(0, 180, 0);
        }

        // 2. Arr�ter le jeu (pause)
        Time.timeScale = 0;
    }

    // Fonction � appeler quand le joueur trouve la sortie
    public void Victoire()
    {
        chronometreActif = false;
        Debug.Log("GAGN� ! Le temps s'arr�te.");
    }

    // Gestion du coffre

    [Header("Inventaire")]
    public bool aLaCle = false; // Pour savoir si le joueur a la cl�

    // Fonction pour modifier le temps (Bonus ou Malus)
    public void ModifierTemps(float secondes)
    {
        tempsRestant += secondes;

        // Petit effet visuel dans la console
        if (secondes > 0) Debug.Log("BONUS ! +" + secondes + " secondes");
        else Debug.Log("PENALITE ! " + secondes + " secondes");
    }

    // Fonction pour ramasser la cl�
    public void RamasserCle()
    {
        aLaCle = true;
        Debug.Log("CLE RECUPEREE !");
    }
}