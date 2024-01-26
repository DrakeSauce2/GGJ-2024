using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDoor : MonoBehaviour
{
    [SerializeField] string levelName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ExitSequence());
        }
    }

    private IEnumerator ExitSequence()
    {
        StartCoroutine(Transition.Instance.CutOut());

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(levelName);
    }

}
