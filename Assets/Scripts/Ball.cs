using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    public AudioClip audioHit;
    public AudioClip audioWin;
    private bool playing = true;
    public GameObject MainCamera;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playing)
        {
            if (collision.gameObject.tag == "Finish")
            {
                playing = false;
                PlayerPrefs.SetInt("Current Level", SceneManager.GetActiveScene().buildIndex - 1);
                MainCamera = GameObject.Find("Main Camera");
                MainCamera.GetComponent<LevelSelect>().levelToLoad = "Level " + (SceneManager.GetActiveScene().buildIndex).ToString();
                MainCamera.GetComponent<AudioSource>().PlayOneShot(audioWin);
            }
            else if (collision.gameObject.tag == "Enemy")
            {
                Destroy(gameObject);
                MainCamera.GetComponent<AudioSource>().PlayOneShot(audioHit);
                //AudioSource.PlayClipAtPoint(audioHit, this.transform.position, 100);
                //Handheld.Vibrate();
            }
        }
    }

    //If ball falls off screen
    private void OnBecameInvisible()
    {
        if (playing)
        {
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(audioHit, this.transform.position, 100);
        }
    }
}