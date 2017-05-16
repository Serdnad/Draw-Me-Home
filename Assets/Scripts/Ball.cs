using UnityEngine;

public class Ball : MonoBehaviour
{
    private float fadeAlpha = -1;
    public AudioClip audioHit;
    public AudioClip audioWin;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Finish" && fadeAlpha == -1)
        {
            PlayerPrefs.SetInt("Current Level", Application.loadedLevel - 1);
            fadeAlpha = 0;
            AudioSource.PlayClipAtPoint(audioWin, this.transform.position, 100);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(audioHit, this.transform.position, 100);
            //Handheld.Vibrate();
        }
    }

    //If ball falls off screen
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(audioHit, this.transform.position, 100);
    }

    private void OnGUI()
    {
        var fadeBlack = Resources.Load("white") as Texture2D;
        GUI.color = new Color(0, 0, 0, fadeAlpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeBlack);

        if (fadeAlpha <= 1 && fadeAlpha != -1)
            fadeAlpha += 0.06f;
        else if (fadeAlpha >= 1f)
            Application.LoadLevel(Application.loadedLevel + 1);
    }
}