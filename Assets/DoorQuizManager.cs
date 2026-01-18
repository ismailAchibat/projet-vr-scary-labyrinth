using System.Collections;
using UnityEngine;

public class DoorQuizManager : MonoBehaviour
{
    [Header("References")]
    public GameObject ghost;
    public Animator ghostAnimator;
    public AudioSource scareSound;
    public Transform playerCamera;

    [Header("Door Settings")]
    public Animator doorAnimator;

    private bool isFollowing = false;

    public void SelectAnswer(bool isCorrect)
    {
        if (isCorrect)
        {
            doorAnimator.SetTrigger("openDoor");
            this.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(ScareAndFollowRoutine());
        }
    }

    IEnumerator ScareAndFollowRoutine()
    {
        // 1. Teleportation du ghost près du joueur
        Vector3 scarePos = playerCamera.position + (playerCamera.forward * 1.5f);

        scarePos.y = playerCamera.position.y; 
        ghost.transform.position = scarePos;
        
        // 2. lancer l'animation et le son de scare
        ghostAnimator.SetTrigger("doAttack");
        scareSound.Play();
        isFollowing = true;

        // 3. le ghost suit le joueur pendant 5 secondes
        float timer = 0;
        float stopDistance = 1.2f; 

        while (timer < 5f)
        {
            // Calculer où se trouve le joueur sur le sol
            Vector3 targetPos = new Vector3(playerCamera.position.x, ghost.transform.position.y, playerCamera.position.z);
            
            // Calculer la distance entre le ghost et le joueur
            float distanceToPlayer = Vector3.Distance(ghost.transform.position, targetPos);

            // Se déplacer uniquement si le ghost est plus éloigné que la distance d'arrêt
            if (distanceToPlayer > stopDistance)
            {
                ghost.transform.position = Vector3.MoveTowards(ghost.transform.position, targetPos, Time.deltaTime * 2.5f);
            }
            
            // Toujours regarder le joueur
            ghost.transform.LookAt(targetPos);
            
            timer += Time.deltaTime;
            yield return null;
        }

        // 4. Se cacher sous la carte
        isFollowing = false;
        ghost.transform.position = new Vector3(0, -50, 0); 
        Debug.Log("Ghost has retreated.");
    }
}