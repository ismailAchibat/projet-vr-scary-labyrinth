using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; // INDISPENSABLE pour la VR

public class PauseManager : MonoBehaviour
{
    [Header("Contrôles VR")]
    public InputActionProperty boutonMenu; // La référence au bouton de la manette

    [Header("UI - Les Textes")]
    public GameObject menuPauseUI;
    public TMP_Text texteTimer;
    public TMP_Text textePourcentage;
    public TMP_Text textePortes;
    public TMP_Text texteCles;

    [Header("Référence Joueur")]
    public Transform cameraJoueur;

    private bool jeuEnPause = false;
    private GameTimer donneesJeu;

    void Start()
    {
        donneesJeu = FindObjectOfType<GameTimer>();
    }

    void Update()
    {
        // On vérifie le clavier (P) OU le bouton de la manette VR
        // "WasPressedThisFrame" veut dire : est-ce qu'on vient d'appuyer dessus à cet instant ?
        bool boutonVRPresse = boutonMenu.action != null && boutonMenu.action.WasPressedThisFrame();

        if (Input.GetKeyDown(KeyCode.P) || boutonVRPresse)
        {
            if (jeuEnPause) ReprendreJeu();
            else MettreEnPause();
        }

        if (jeuEnPause)
        {
            MettreAJourInterface();
        }
    }

    public void MettreEnPause()
    {
        jeuEnPause = true;
        menuPauseUI.SetActive(true);

        if (cameraJoueur != null)
        {
            menuPauseUI.transform.position = cameraJoueur.position + cameraJoueur.forward * 1.5f;
            menuPauseUI.transform.LookAt(cameraJoueur);
            menuPauseUI.transform.Rotate(0, 180, 0);

            Vector3 euler = menuPauseUI.transform.rotation.eulerAngles;
            menuPauseUI.transform.rotation = Quaternion.Euler(0, euler.y, 0);
        }

        // Note : En VR, le Time.timeScale arrête les monstres mais permet de bouger la tête
        Time.timeScale = 0f;
    }

    public void ReprendreJeu()
    {
        jeuEnPause = false;
        menuPauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void MettreAJourInterface()
    {
        if (donneesJeu == null) return;

        float minutes = Mathf.FloorToInt(donneesJeu.tempsRestant / 60);
        float secondes = Mathf.FloorToInt(donneesJeu.tempsRestant % 60);


        // 1. Le Timer (ex: 2min 15s restantes)
        if (texteTimer != null)
            texteTimer.text = string.Format("{0}min {1:00}s restantes", minutes, secondes);

        // 2. Les Clés (ex: 1 clé trouvée)
        if (texteCles != null)
            texteCles.text = donneesJeu.clesTrouvees + " clé(s) trouvée(s)";

        // 3. Les Portes (ex: 2 portes ouvertes)
        if (textePortes != null)
            textePortes.text = donneesJeu.portesOuvertes + " porte(s) ouverte(s)";

        // 4. Le Pourcentage (ex: 25% terminé)
        if (textePourcentage != null)
        {
            int p = (donneesJeu.clesTrouvees * 25) + (donneesJeu.portesOuvertes * 10);
            textePourcentage.text = p + "% terminé";
        }
    }
}