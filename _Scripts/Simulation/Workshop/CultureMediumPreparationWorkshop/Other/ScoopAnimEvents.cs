using UnityEngine;

public class ScoopAnimEvents : MonoBehaviour
{
    Transform content;

    ParticleSystem particle;

    private void Awake()
    {
        content = transform.Find("Content");
        particle = transform.Find("ParticleSystem").GetComponent<ParticleSystem>();
    }
    private void HideContent()
    {
        content.gameObject.SetActive(false);
    }
    private void ShowContent()
    {
        content.gameObject.SetActive(true);
    }
    private void PlayParticle()
    {
        particle.Play();
    }
    private void StopParticle()
    {
        particle.Stop();
    }
}
