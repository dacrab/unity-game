using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [Header("Scene Loading Settings")]
    [SerializeField] private GameObject magicStone; // The magic stone collider
    [SerializeField] private SpriteRenderer indicator; // The sprite renderer
    [SerializeField] private ParticleSystem myParticleSystem; // The particle system
    [SerializeField] private UIManager uiManager; // Reference to the UIManager

    private bool isNearMagicStone = false;

    void Update()
    {
        if (isNearMagicStone && Input.GetKeyDown(KeyCode.E))
        {
            StartLoadingNextLevel();
        }
    }

    private void StartLoadingNextLevel()
    {
        indicator.enabled = false;
        myParticleSystem.Play();
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        uiManager.ShowLoadingScreen(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            uiManager.UpdateLoadingImage(progress);

            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        uiManager.ShowLoadingScreen(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isNearMagicStone = true;
            indicator.enabled = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isNearMagicStone = false;
            indicator.enabled = false;
        }
    }
}

