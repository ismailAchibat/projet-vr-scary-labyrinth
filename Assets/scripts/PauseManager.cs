using UnityEngine;
using TMPro; // INDISPENSABLE pour modifier les textes

public class PauseManager : MonoBehaviour
{
    [Header("UI - Les Textes à modifier")]
    public GameObject menuPauseUI;
    public TMP_Text texteTimer;       // Le titre "3min..."
    public TMP_Text textePourcentage; // "20% completed"
    public TMP_Text textePortes;      // "0 doors completed"
    public TMP_Text texteCles;        // "0 key collected"

    [Header("Référence Joueur")]
    public Transform cameraJoueur;

    private bool jeuEnPause = false;
    private GameTimer donneesJeu; // Pour accéder au timer et aux clés

    void Start()
    {
        // On cherche le GameTimer automatiquement au début
        donneesJeu = FindObjectOfType<GameTimer>();
    }

    void Update()
    {
        // Touche M ou Echap pour tester
        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (jeuEnPause) ReprendreJeu();
            else MettreEnPause();
        }

        // Si le jeu est en pause, on met à jour le timer en direct
        if (jeuEnPause)
        {
            MettreAJourInterface();
        }
    }

    public void MettreEnPause()
    {
        jeuEnPause = true;
        menuPauseUI.SetActive(true);

        // Placer le menu devant le joueur
        if (cameraJoueur != null)
        {
            menuPauseUI.transform.position = cameraJoueur.position + cameraJoueur.forward * 1.0f;
            menuPauseUI.transform.LookAt(cameraJoueur);
            menuPauseUI.transform.Rotate(0, 180, 0); // Corriger l'inversion

            // Garder le menu droit (pas penché si on regarde en haut/bas)
            Vector3 euler = menuPauseUI.transform.rotation.eulerAngles;
            menuPauseUI.transform.rotation = Quaternion.Euler(0, euler.y, 0);
        }

        Time.timeScale = 0f; // Arrêter le temps
    }

    public void ReprendreJeu()
    {
        jeuEnPause = false;
        menuPauseUI.SetActive(false);
        Time.timeScale = 1f; // Reprendre le temps
    }

    void MettreAJourInterface()
    {
        if (donneesJeu == null) return;

        // 1. Mettre à jour le TITRE (Timer)
        float minutes = Mathf.FloorToInt(donneesJeu.tempsRestant / 60);
        float secondes = Mathf.FloorToInt(donneesJeu.tempsRestant % 60);
        if (texteTimer != null)
            texteTimer.text = string.Format("{0}min {1:00}sec left", minutes, secondes);

        // 2. Mettre à jour les CLES
        if (texteCles != null)
            texteCles.text = donneesJeu.clesTrouvees + " key collected";

        // 3. Mettre à jour les PORTES
        if (textePortes != null)
            textePortes.text = donneesJeu.portesOuvertes + " doors completed";

        // 4. Mettre à jour le POURCENTAGE (Calcul simple : 1 clé = 25%)
        if (textePourcentage != null)
        {
            int p = (donneesJeu.clesTrouvees * 25) + (donneesJeu.portesOuvertes * 10);
            textePourcentage.text = p + "% completed";
        }
    }
}